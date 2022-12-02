using System;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRoomInfo
{
    public bool UpDoor;
    public bool DownDoor;
    public bool LeftDoor;
    public bool RightDoor;
    public bool IsStartRoom;
    public int MapPositionX;
    public int MapPositionY;
    public bool IsUsed;
    public bool Placed;
    public Map.RoomType RoomType;

    public enum WallDirection
    {
        DOWN,
        UP,
        LEFT,
        RIGHT
    }

    public List<WallDirection> ValidWalls = new()
        {
            WallDirection.UP,
            WallDirection.DOWN,
            WallDirection.LEFT,
            WallDirection.RIGHT
        };
    
    public SimpleRoomInfo(int x, int y, Map.RoomType roomType)
    {
        this.UpDoor = false;
        this.DownDoor = false;
        this.LeftDoor = false;
        this.RightDoor = false;
        this.IsStartRoom = false;
        this.MapPositionX = x;
        this.MapPositionY = y;
        this.RoomType = roomType;
    }

    public void OpenDoorInWall(WallDirection wall)
    {
        switch (wall)
        {
            case WallDirection.UP: UpDoor = true; break;
            case WallDirection.DOWN: DownDoor = true; break;
            case WallDirection.LEFT: LeftDoor = true; break;
            case WallDirection.RIGHT: RightDoor = true; break;
        }
    }
}

public class Map : MonoBehaviour
{
    private Helper Helper;
    public SimpleRoomInfo[,] LevelMap;
    private GameManager GameManager;
    public Vector3 CurrentMapPosition;
    public int mapLength = 25;
    public int MapHeight = 25;

    public enum RoomType
    {
        Standard, 
        Puzzle, 
        LoreRoom, 
        Prize, 
        Trap1, 
        Trap2, 
        Swarm
    }

    void Start()
    {
        Helper = GameObject.FindGameObjectWithTag("Helper").GetComponent<Helper>();
        LevelMap = new SimpleRoomInfo[mapLength, MapHeight];
        GameManager = Helper.GameManager;

        //Set start room
        FillMapWithRooms();
        CreatePathThroughRooms();
        SpawnRoomsInGameSpace();
    }

    /// <summary> Spawns a room into game space and open any valid doors </summary>
    public void SpawnRoom(int x, int y, Vector3 worldPos, int RoomNumber)
    {
        GameObject simpleRoom = Helper.SimpleRoomPrefab;
        GameObject currentRoom = Instantiate(simpleRoom, worldPos, Quaternion.identity);

        currentRoom.name = $"X:{x} Y:{y} Room {RoomNumber}";
        var _currentRoom = currentRoom.GetComponent<SimpleRoom>();
        _currentRoom.RoomType = LevelMap[x, y].RoomType.ToString();
        currentRoom.transform.parent = GameObject.Find("Rooms").transform;

        if (LevelMap[x, y].UpDoor) _currentRoom.OpenDoor("UP");
        if (LevelMap[x, y].DownDoor) _currentRoom.OpenDoor("DOWN");
        if (LevelMap[x, y].LeftDoor) _currentRoom.OpenDoor("LEFT");
        if (LevelMap[x, y].RightDoor) _currentRoom.OpenDoor("RIGHT");

        // If start room remove any enemies and spawn player
        if (RoomNumber == 0)
        {
            _currentRoom.RoomType = "StartRoom";
            _currentRoom.gameObject.AddComponent<StartRoom>();
        }

        // If end room remove any enemies and spawn mini boss and exit tile
        if (RoomNumber == GetTotalValidRooms() - 1)
        {
            _currentRoom.RoomType = "EndRoom";
            _currentRoom.gameObject.AddComponent<EndRoom>();
        }

        // Configure Standard room
        if (_currentRoom.RoomType == "Standard")
        {
            _currentRoom.gameObject.AddComponent<StandardRoom>();
        }

        // Configure Prize Room
        if (_currentRoom.RoomType == "Prize")
        {
            _currentRoom.gameObject.AddComponent<PrizeRoom>();
        }

        // Configure Trap Room
        if (_currentRoom.RoomType == "Trap1")
        {
            _currentRoom.gameObject.AddComponent<TrapRoom>();
        }

        if (_currentRoom.RoomType == "Trap2")
        {
            _currentRoom.gameObject.AddComponent<WallOfDeathRoom>();
        }

        // Configure Lore Room
        if (_currentRoom.RoomType == "LoreRoom")
        {
            _currentRoom.gameObject.AddComponent<LoreRooms>();
        }

        // Configure Puzzle Room
        if (_currentRoom.RoomType == "Puzzle")
        {
            _currentRoom.gameObject.AddComponent<PuzzleRooms>();
        }

        // Configure Puzzle Room
        if (_currentRoom.RoomType == "Swarm")
        {
            _currentRoom.gameObject.AddComponent<SwarmRooms>();
        }

        foreach (var w in _currentRoom.spawnedWallTiles)
        {
            // For some reason floor tiles were getting slightly changed when instanciated, for example something
            // with a y of 0.05 in the prefab would instantiated with  0.500001, this was breaking the conversion in 
            // SetTileSprite so this was the fix I came up with.
            var convertedX = float.Parse(w.transform.localPosition.x.ToString("0.##"));
            var convertedY = float.Parse(w.transform.localPosition.y.ToString("0.##"));
            w.GetComponent<Wall>().SetTileSprite(new Vector3(convertedX, convertedY, 0));
        }

        _currentRoom.SaveRoomLayoutToFile(x, y);

    }

    public RoomType GetRandomRoomType()
    {
        var totalRoomTypes = Enum.GetNames(typeof(RoomType)).Length;
        return (RoomType)UnityEngine.Random.Range(0, totalRoomTypes);
    }

    /// <summary> Bulk fills the 2D map array with SimpleRoomInfoObjects </summary>
    public void FillMapWithRooms()
    {
        var specialRoomsAlreadyPlaced = new List<string>();

        for (int x = 0; x < LevelMap.GetLength(0); x++)
        {
            for (int y = 0; y < LevelMap.GetLength(1); y++)
            {
                if (LevelMap[x, y] == null || !LevelMap[x, y].Placed)
                {
                    var roomType = GetRandomRoomType();
                    if (specialRoomsAlreadyPlaced.Contains(roomType.ToString())) roomType = RoomType.Standard;

                    if (roomType != RoomType.Standard) specialRoomsAlreadyPlaced.Add(roomType.ToString());

                    LevelMap[x, y] = new SimpleRoomInfo(x, y, roomType)
                    {
                        Placed = true
                    };
                }
                CurrentMapPosition = new Vector3(x, y, 0);
            }
        }
    }

    /// <summary> Removes room exits that would lead out of bounds of the array  </summary>
    public void RemoveInvalidExits(SimpleRoomInfo room)
    {
        // If room is on the edge of map, remove doors that would lead off the map
        if (room.MapPositionY == 0) room.ValidWalls.Remove(SimpleRoomInfo.WallDirection.DOWN);
        if (room.MapPositionY == (LevelMap.GetLength(1) - 1)) room.ValidWalls.Remove(SimpleRoomInfo.WallDirection.UP);
        if (room.MapPositionX == 0) room.ValidWalls.Remove(SimpleRoomInfo.WallDirection.LEFT);
        if (room.MapPositionX == (LevelMap.GetLength(0) - 1)) room.ValidWalls.Remove(SimpleRoomInfo.WallDirection.RIGHT);
    }

    /// <summary> Opens a random valid door and the doorway of the adjacent room to link both rooms. Returns the adjacent room </summary>
    public SimpleRoomInfo OpenRandomValidDoor(SimpleRoomInfo room)
    {
        RemoveInvalidExits(room);

        // Return a random valid door
        var wall = room.ValidWalls[UnityEngine.Random.Range(0, room.ValidWalls.Count)];
        room.OpenDoorInWall(wall);
        // Open the adjacent rooms door
        SimpleRoomInfo connectingRoom = LevelMap[0, 0];
        var oppositeDoor = SimpleRoomInfo.WallDirection.UP;
        switch (wall)
        {
            case SimpleRoomInfo.WallDirection.UP:
                connectingRoom = LevelMap[room.MapPositionX, (room.MapPositionY + 1)];
                oppositeDoor = SimpleRoomInfo.WallDirection.DOWN;
                break;
            case SimpleRoomInfo.WallDirection.DOWN:
                connectingRoom = LevelMap[room.MapPositionX, (room.MapPositionY - 1)];
                oppositeDoor = SimpleRoomInfo.WallDirection.UP;
                break;
            case SimpleRoomInfo.WallDirection.LEFT:
                connectingRoom = LevelMap[(room.MapPositionX - 1), room.MapPositionY];
                oppositeDoor = SimpleRoomInfo.WallDirection.RIGHT;
                break;
            case SimpleRoomInfo.WallDirection.RIGHT:
                connectingRoom = LevelMap[(room.MapPositionX + 1), room.MapPositionY];
                oppositeDoor = SimpleRoomInfo.WallDirection.LEFT;
                break;
        }
        connectingRoom.OpenDoorInWall(oppositeDoor);

        return connectingRoom;

    }
    /// <summary> Creates a random path though the map </summary>
    public void CreatePathThroughRooms()
    {
        // Pick a random start room
        var currentRoom = LevelMap[UnityEngine.Random.Range(0, (LevelMap.GetLength(0) - 1)), UnityEngine.Random.Range(0, (LevelMap.GetLength(1) - 1))];
        currentRoom.IsStartRoom = true;

        // Need to track the current room and the next room
        for (int i = 0; i < (LevelMap.GetLength(0) * LevelMap.GetLength(1)); i++)
        {
            currentRoom.IsUsed = true;
            currentRoom = OpenRandomValidDoor(currentRoom);
        }
    }

    /// <summary> Converts the position in the 2D array to the game space </summary>
    public Vector3 ConvertMapPositionToWorldPosition(int x, int y)
    {
        return new Vector3((x * 10), (y * 10), 0);
    }

    public int GetTotalValidRooms()
    {
        int totalValidRooms = 0;

        for (int y = 0; y < LevelMap.GetLength(0); y++)
        {
            for (int x = 0; x < LevelMap.GetLength(1); x++)
            {
                if (LevelMap[x, y] != null && LevelMap[x, y].IsUsed)
                {
                    totalValidRooms++;
                }
            }
        }
        return totalValidRooms;
    }


    /// <summary> Iterates over the 2D map array and spawns any valid rooms into gamespace </summary>
    public void SpawnRoomsInGameSpace()
    {
        int roomNumber = 0;
        for (int y = 0; y < LevelMap.GetLength(0); y++)
        {
            for (int x = 0; x < LevelMap.GetLength(1); x++)
            {
                if (LevelMap[x, y] != null && LevelMap[x, y].IsUsed)
                {
                    var worldPos = ConvertMapPositionToWorldPosition(x, y);
                    SpawnRoom(x, y, worldPos, roomNumber);
                    roomNumber++;
                }
            }
        }
        GameManager.timeTillDeath = roomNumber * 10;
    }
}