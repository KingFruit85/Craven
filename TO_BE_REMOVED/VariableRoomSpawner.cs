using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VariableRoomSpawner : MonoBehaviour
{
     const int X_MIN = 5;
     const int X_MAX = 15;
     const int Y_MIN = 5;
     const int Y_MAX = 15;

     public int _x;
     public int _y;

     public Vector3 placerLocation;
     public Vector2 roomDimentions;

     public List<GameObject> rooms = new List<GameObject>();
     public List<Vector2> dynamicRoomList = new List<Vector2>();
     public List<GameObject> wallTiles = new List<GameObject>();
     public List<GameObject> floorTiles = new List<GameObject>();
     

     public string currentDirection;

     List<string> directions = new List<string>{"up","down","left","right"};

     void Start()
     {
        // CreateDynamicRoomList(5);
        // ArrangeRooms();
        // BuildRoom();
        // randomPath();
        RandPath();
     }

    public Vector3 getDirection(List<string> dir)
    {
        string direction =  dir[Random.Range(0,directions.Count)];

        switch (direction)
         {
            default: Debug.Log("NO VALID DIRECTIONS TO SPAWN IN"); return new Vector3(0,0,0);
            case "up": currentDirection = "up"; return new Vector3(0,1,0); 
            case "down": currentDirection = "down"; return new Vector3(0,-1,0);    
            case "left": currentDirection = "left"; return new Vector3(-1,0,0);    
            case "right": currentDirection = "right"; return new Vector3(1,0,0);    
         }
    }

    public void RandPath()
    {
        int floorSize = 50;
        List<string> validDirections = directions;
        Vector3 spawnDirection = Vector3.zero;
        Vector3 currentPosition = new Vector3(0,0,0);
        bool[,][,] blocks = new bool[100,100][,];
        

        // Spawn start tile
         GameObject startTile = Instantiate(Resources.Load("GenericFloor"),currentPosition,Quaternion.identity) as GameObject;
         startTile.name = "startTile";

        for (int i = 0; i < floorSize; i++)
        {
            // Decide which direction to spawn next block
            spawnDirection = getDirection(validDirections);

            // Spawn new floor tile
            GameObject newTile = Instantiate(Resources.Load("GenericFloor"),currentPosition += spawnDirection,Quaternion.identity) as GameObject;

            // Check to see if placed on existing tile. If it is check other positions around source tile for a valid space
            for (int j = 0; j < 3; j++)
            {
                if (newTile.GetComponent<FloorTile>().isTouchingOtherCollider)
                {
                    Debug.Log("Shit i've collided");
                    // Destory tile
                    Destroy(newTile);
                    // Remove previous direction from valid direaction list 
                    validDirections.Remove(currentDirection);
                    // Get new spawn direction
                    spawnDirection = getDirection(validDirections);
                    // Try spawning again
                    newTile = Instantiate(Resources.Load("GenericFloor"),currentPosition += spawnDirection,Quaternion.identity) as GameObject;
                }
                else
                {
                    //space is valid
                    newTile.name = "tile" + i;
                    break;
                }
                newTile.name = "tile" + i;
                validDirections = directions;
            }
        }

    }
     public void randomPath()
     {
         int length = 5;
         Vector3 currentPosition = new Vector3(0,0,0);

         // Spawn start tile
         GameObject startTile = Instantiate(Resources.Load("GenericFloor"),currentPosition,Quaternion.identity) as GameObject;
         startTile.name = "startTile";

        // Need to check tile bools to see which spawn directions are valid
        // bool canSpawnNextTileUp = startTile.GetComponent<FloorTile>().upDetector;
        // bool canSpawnNextTileDown = startTile.GetComponent<FloorTile>().downDetector;
        // bool canSpawnNextTileLeft = startTile.GetComponent<FloorTile>().leftDetector;
        // bool canSpawnNextTileRight = startTile.GetComponent<FloorTile>().rightDetector;

        List<string> viableDirections = new List<string>();

        // if (!canSpawnNextTileUp) viableDirections.Add("Up");
        // if (!canSpawnNextTileDown) viableDirections.Add("Down");
        // if (!canSpawnNextTileLeft) viableDirections.Add("Left");
        // if (!canSpawnNextTileRight) viableDirections.Add("Right");
        
         for (int i = 0; i <= length; i++)
         {
             GameObject tile = null;
             switch (viableDirections[Random.Range(0,viableDirections.Count)])
             {
                case "Up": 
                    currentPosition += new Vector3(0,1,0);
                    tile = Instantiate(Resources.Load("GenericFloor"),currentPosition,Quaternion.identity) as GameObject;
                    tile.name = "tile"+i;
                    break;

                case "Down": 
                    currentPosition += new Vector3(0,-1,0);
                    tile = Instantiate(Resources.Load("GenericFloor"),currentPosition,Quaternion.identity) as GameObject;
                    tile.name = "tile"+i;
                    break;               

                case "Left": 
                    currentPosition += new Vector3(-1,0,0);
                    tile = Instantiate(Resources.Load("GenericFloor"),currentPosition,Quaternion.identity) as GameObject;
                    tile.name = "tile"+i;
                    break;      

                case "Right": 
                    currentPosition += new Vector3(1,0,0);
                    tile = Instantiate(Resources.Load("GenericFloor"),currentPosition,Quaternion.identity) as GameObject;
                    tile.name = "tile"+i;
                    break;                      
             }
            // canSpawnNextTileUp = tile.GetComponent<FloorTile>().upDetector;
            // canSpawnNextTileDown = tile.GetComponent<FloorTile>().downDetector;
            // canSpawnNextTileLeft = tile.GetComponent<FloorTile>().leftDetector;
            // canSpawnNextTileRight = tile.GetComponent<FloorTile>().rightDetector;
         }
     }


    public Vector2 GetRandomRoomDimentions()
    {
        int x = Random.Range(X_MIN,X_MAX);
        int y = Random.Range(Y_MIN,Y_MAX);
        _x = x;
        _y = y;
        
        return new Vector2(x,y);
    }

    public void CreateDynamicRoomList(int numberOfRooms)
    {
        dynamicRoomList = new List<Vector2>();
        
        for (int i = 0; i < numberOfRooms; i++)
        {
            dynamicRoomList.Add(GetRandomRoomDimentions());
        }
    }

    public void BuildRoom(Vector2 roomDimentions, Vector3 buildLocation)
    {
        GameObject room = new GameObject();
        room.name = "room_" + room.GetInstanceID();
        room.transform.parent = GameObject.Find("Rooms").transform;
        GameObject newTile = null;

        
        Vector3 placer = buildLocation;


        for (int i = (int)placer.y; i <= roomDimentions.y; i++)
        {
            for (int j = 0; j <= roomDimentions.x; j++)
            {
                newTile = Instantiate(Resources.Load("GenericFloor"),placer,Quaternion.identity) as GameObject;
                newTile.transform.parent = room.transform;
                floorTiles.Add(newTile);
                placer.x ++;
            }
            placer.x = buildLocation.x;
            placer.y ++;
        }

        // Add collider
        GameObject collider = new GameObject();
        collider.name = "collider";
        collider.transform.parent = room.transform;
        collider.transform.position = new Vector3((roomDimentions.x / 2), roomDimentions.y / 2,0);
        collider.AddComponent<BoxCollider2D>();
        collider.GetComponent<BoxCollider2D>().size = new Vector2(roomDimentions.x + 1, roomDimentions.y +1 );
        collider.GetComponent<BoxCollider2D>().offset = new Vector2(buildLocation.x,0f);
        collider.GetComponent<BoxCollider2D>().isTrigger = true;

        rooms.Add(room);


        

        // // Place the floor line by line iterating over the Y axis
        // for (int y = (int)placer.y; y <= (int)roomDimentions.y; y++)
        // {
        //     // Place tiles along the x axis
        //     for (int x = (int)placer.x; x <= (int)roomDimentions.x; x++)
        //     {
        //         // 
        //         if (placer.y == 1 || placer.y == roomDimentions.y)
        //         {
        //            newTile = Instantiate(Resources.Load("GenericWall"),placer,Quaternion.identity) as GameObject;
        //            newTile.transform.parent = room.transform;
        //            wallTiles.Add(newTile);
        //         }
        //         else if (placer.x == 1 || placer.x == roomDimentions.x)
        //         {
        //             newTile = Instantiate(Resources.Load("GenericWall"),placer,Quaternion.identity) as GameObject;
        //             newTile.transform.parent = room.transform;
        //             wallTiles.Add(newTile);
        //         }
        //         else
        //         {
        //             newTile = Instantiate(Resources.Load("GenericFloor"),placer,Quaternion.identity) as GameObject; 
        //             newTile.transform.parent = room.transform;
        //             floorTiles.Add(newTile);
        //         }

        //         placer.x ++;
        //     }

        //     // Reset placer value back to 1 and move to next Y row
        //     placer.x = 1;
        //     placer.y ++;
        // }


        // UpdateTileSprites(newTile,roomDimentions);

        

    }


    public void BuildRoom()
    {
        roomDimentions = GetRandomRoomDimentions();
        placerLocation = new Vector3(1,1,0);

        GameObject room = new GameObject();
        room.name = "room_" + room.GetInstanceID(); 
        GameObject newTile = null;
        
        // Place the floor line by line iterating over the Y axis
        for (int y = (int)placerLocation.y; y <= (int)roomDimentions.y; y++)
        {
            // Place tiles along the x axis
            for (int x = (int)placerLocation.x; x <= (int)roomDimentions.x; x++)
            {
                // 
                if (placerLocation.y == 1 || placerLocation.y == roomDimentions.y)
                {
                   newTile = Instantiate(Resources.Load("GenericWall"),placerLocation,Quaternion.identity) as GameObject;
                   newTile.transform.parent = room.transform;
                   wallTiles.Add(newTile);
                }
                else if (placerLocation.x == 1 || placerLocation.x == roomDimentions.x)
                {
                    newTile = Instantiate(Resources.Load("GenericWall"),placerLocation,Quaternion.identity) as GameObject;
                    newTile.transform.parent = room.transform;
                    wallTiles.Add(newTile);
                }
                else
                {
                    newTile = Instantiate(Resources.Load("GenericFloor"),placerLocation,Quaternion.identity) as GameObject; 
                    newTile.transform.parent = room.transform;
                    floorTiles.Add(newTile);
                }

                placerLocation.x ++;
            }

            // Reset placer value back to 1 and move to next Y row
            placerLocation.x = 1;
            placerLocation.y ++;
        }
        UpdateTileSprites(newTile,roomDimentions);

        foreach (var tile in wallTiles)
        {
            tile.GetComponent<Wall>().UpdateTile();
        }

    }

    void UpdateTileSprites(GameObject tile, Vector2 roomDimentions)
    {
        //Figures out what type of wall tile 
        foreach (var wall in wallTiles)
        {
            if(wall)
            {
                if (wall.transform.position.x == 1 && wall.transform.position.y == (int)roomDimentions.y)
                {
                    wall.GetComponent<Wall>().topLeftCorner = true;
                }
                else if (wall.transform.position.x == (int)roomDimentions.x && wall.transform.position.y == (int)roomDimentions.y)
                {
                    wall.GetComponent<Wall>().topRightCorner = true;
                }
                else if (wall.transform.position.x == 1 && wall.transform.position.y == 1)
                {
                    wall.GetComponent<Wall>().bottomLeftCorner = true;
                }
                else if (wall.transform.position.x == (int)roomDimentions.x && wall.transform.position.y == 1)
                {
                    wall.GetComponent<Wall>().bottomRightCorner = true;
                }
                else if (wall.transform.position.x == 1)
                {
                    wall.GetComponent<Wall>().leftWall = true;
                }
                else if (wall.transform.position.x == (int)roomDimentions.x)
                {
                    wall.GetComponent<Wall>().rightWall = true;
                }
                else if (wall.transform.position.y == 1)
                {
                    wall.GetComponent<Wall>().bottomWall = true;
                }
                else if (wall.transform.position.y == (int)roomDimentions.y)
                {
                    wall.GetComponent<Wall>().topWall = true;
                }

                wall.GetComponent<Wall>().UpdateTile();
            }
        }
    }

    public void ArrangeRooms()
    {
        Vector3 startLocation = new Vector3(0,0,0);
        var i = 50;

        // Spawn first room at origin
        BuildRoom(dynamicRoomList[0],startLocation);

        // Set a toggle that decides if new room spawns up/down/left/right 
        var spawnDirection = new string[4]{"up","down","left","right"};

        foreach (var room in dynamicRoomList)
        {
            //Skip first room
            if (room == dynamicRoomList[0]){
                continue;
            }
            
            // Get spawn direction
            var spawnLocation = spawnDirection[Random.Range(0,3)];
            Vector2 lastRoomPlacedDimentions;

            // Spawn the room
            switch (spawnLocation)
            {
                case "up":
                    // Set start location to top left corner of previous room
                    lastRoomPlacedDimentions = dynamicRoomList[i];
                    BuildRoom(dynamicRoomList[i],new Vector2(0,lastRoomPlacedDimentions.y));
                    break;

                case "down":
                    // Set start location the bottom of the last room - the hight of the new room
                    lastRoomPlacedDimentions = dynamicRoomList[i];
                    BuildRoom(dynamicRoomList[i],new Vector2(0,lastRoomPlacedDimentions.y - dynamicRoomList[i].y));
                    break;

                case "left":
                    // Set start location to bottom left corner of previous room - the width of the new room
                    lastRoomPlacedDimentions = dynamicRoomList[i];
                    BuildRoom(dynamicRoomList[i],new Vector2(lastRoomPlacedDimentions.x - dynamicRoomList[i].x,0));
                    break;

                case "right":
                    // Set start location to top left corner of previous room
                    lastRoomPlacedDimentions = dynamicRoomList[i];
                    BuildRoom(dynamicRoomList[i],new Vector2(0,lastRoomPlacedDimentions.y));
                    break;
            }
            i++;
        }

    }



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            for (int i = 0; i < rooms.Count; i++)
            {
                rooms[i].SetActive(false);
            }
            CreateDynamicRoomList(20);
            ArrangeRooms();
        }
    }
}
