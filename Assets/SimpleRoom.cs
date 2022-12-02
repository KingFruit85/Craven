using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SimpleRoom : MonoBehaviour
{
    public GameObject[] LeftDoorTiles;
    public GameObject LeftDoorEdgeTileTop;
    public Sprite LeftDoorEdgeTileTopSprite;
    public GameObject LeftDoorEdgeTileBottom;
    public Sprite LeftDoorEdgeTileBottomSprite;


    public GameObject[] RightDoorTiles;
    public GameObject RightDoorEdgeTileTop;
    public Sprite RightDoorEdgeTileTopSprite;
    public GameObject RightDoorEdgeTileBottom;
    public Sprite RightDoorEdgeTileBottomSprite;

    public GameObject[] UpDoorTiles;
    public GameObject UpDoorEdgeTileLeft;
    public Sprite UpDoorEdgeTileLeftSprite;
    public GameObject UpDoorEdgeTileRight;
    public Sprite UpDoorEdgeTileRightSprite;


    public GameObject[] DownDoorTiles;
    public GameObject DownDoorEdgeTileLeft;
    public Sprite DownDoorEdgeTileLeftSprite;
    public GameObject DownDoorEdgeTileRight;
    public Sprite DownDoorEdgeTileRightSprite;

    public GameObject EnemySpawner;
    public GameObject ExitTile;
    public GameObject[] SpawnableFloorTiles;
    public GameObject[] runeTiles;
    public GameObject[] pillarTiles;
    public GameObject puzzleChestSpawnLocation;

    public GameObject arrowTrap1Position;
    public GameObject arrowTrap2Position;
    public GameObject arrowTrap3Position;
    public GameObject arrowTrap4Position;

    public GameObject[] potentialTerminalLocation;
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;

    public List<GameObject> spawnedWallTiles = new List<GameObject>();
    public int safeTile = 0;

    public GameObject UpDoor;
    public GameObject DownDoor;
    public GameObject LeftDoor;
    public GameObject RightDoor;

    public string RoomType;
    public char[,] RoomContents = new char[10, 10];
    public int EnemyCount;

    /// <summary> 0 = floor, 1 = wall </summary>
    public void SetRoomContents()
    {
        for (int x = 0; x < RoomContents.GetLength(0); x++)
        {
            for (int y = 0; y < RoomContents.GetLength(1); y++)
            {
                if (x == 0 || y == 0 || x == RoomContents.GetLength(0) - 1 || y == RoomContents.GetLength(1) - 1)
                {
                    RoomContents[x, y] = 'X';
                }
                else
                {
                    RoomContents[x, y] = '░';
                }
            }
        }
    }

    public Vector3 ReturnTerminalSpawnLocation()
    {
        return potentialTerminalLocation[UnityEngine.Random.Range(0, potentialTerminalLocation.Length)].transform.position;
    }

    public void placeFloorTiles()
    {
        // Scan through the roomcontents array and place a floor tile on each index
        for (int x = 0; x < RoomContents.GetLength(0); x++)
        {
            for (int y = 0; y < RoomContents.GetLength(1); y++)
            {
                // The position should be the array position converted to world position using vector2int
                GameObject floorTile = Instantiate(Resources.Load("GenericFloor"), transform.position, Quaternion.identity) as GameObject;
            }
        }
    }

    public List<Vector2> GetAllTilesOfType(char type)
    {
        List<Vector2> tiles = new List<Vector2>();

        for (int x = 0; x < RoomContents.GetLength(0); x++)
        {
            for (int y = 0; y < RoomContents.GetLength(1); y++)
            {
                if (RoomContents[x, y] == type)
                {
                    tiles.Add(new Vector2(x, y));
                }
            }
        }

        return tiles;

    }

    public void setTileContent(int x, int y, char contents)
    {
        RoomContents[x, y] = contents;
    }

    public char GetTileContents(float x, float y)
    {
        try
        {
            return RoomContents[(int)x, (int)y];
        }
        catch (System.IndexOutOfRangeException)
        {
            Debug.Log($"Tried to get tile contents at = X:{x}, Y:{y}");
        }

        return 'F';

    }

    public void SaveRoomLayoutToFile(int xx, int yy)
    {
        using (var sw = new StreamWriter($"./MapData/X{xx}Y{yy}.txt"))
        {
            for (int y = 0; y < RoomContents.GetLength(0); y++)
            {
                for (int x = 0; x < RoomContents.GetLength(1); x++)
                {
                    sw.Write(RoomContents[x, y]);
                }
                sw.Write("\n");

            }
            sw.Flush();
            sw.Close();
        }
    }

    void Awake()
    {
        SetRoomContents();
    }

    public void SpawnExitTile()
    {
        var exitSquare = Instantiate(Resources.Load("ExitSquare_LOCKED") as GameObject, ExitTile.transform.position, Quaternion.identity, transform) as GameObject;
        exitSquare.name = "ExitSquare_LOCKED";
    }

    public void UnlockExitTile()
    {
        var exitSquare = Instantiate(Resources.Load("ExitSquare") as GameObject, GameObject.FindGameObjectWithTag("Exit").transform.position, Quaternion.identity) as GameObject;
        Destroy(GameObject.Find("ExitSquare_LOCKED"));
    }

    public void OpenDoor(string door)
    {
        switch (door)
        {
            case "UP":
                foreach (var tile in UpDoorTiles)
                { tile.SetActive(false); }
                UpDoorEdgeTileLeft.GetComponent<SpriteRenderer>().sprite = UpDoorEdgeTileLeftSprite;
                UpDoorEdgeTileRight.GetComponent<SpriteRenderer>().sprite = UpDoorEdgeTileRightSprite;
                UpDoor.SetActive(true);
                setTileContent(0, 4, '-');
                setTileContent(0, 5, '-');
                break;
            case "DOWN":
                foreach (var tile in DownDoorTiles)
                { tile.SetActive(false); }
                DownDoorEdgeTileLeft.GetComponent<SpriteRenderer>().sprite = DownDoorEdgeTileLeftSprite;
                DownDoorEdgeTileRight.GetComponent<SpriteRenderer>().sprite = DownDoorEdgeTileRightSprite;
                DownDoor.SetActive(true);
                setTileContent(9, 4, '-');
                setTileContent(9, 5, '-');
                break;
            case "LEFT":
                foreach (var tile in LeftDoorTiles)
                { tile.SetActive(false); }
                LeftDoorEdgeTileTop.GetComponent<SpriteRenderer>().sprite = LeftDoorEdgeTileTopSprite;
                LeftDoorEdgeTileBottom.GetComponent<SpriteRenderer>().sprite = LeftDoorEdgeTileBottomSprite;
                LeftDoor.SetActive(true);
                setTileContent(4, 0, '-');
                setTileContent(5, 0, '-');
                break;
            case "RIGHT":
                foreach (var tile in RightDoorTiles)
                { tile.SetActive(false); }
                RightDoorEdgeTileTop.GetComponent<SpriteRenderer>().sprite = RightDoorEdgeTileTopSprite;
                RightDoorEdgeTileBottom.GetComponent<SpriteRenderer>().sprite = RightDoorEdgeTileBottomSprite;
                RightDoor.SetActive(true);
                setTileContent(4, 9, '-');
                setTileContent(5, 9, '-');
                break;
        }
    }

    public Vector2 ArrayToWorldPOS(Vector2 pos)
    {
        if (pos.x == 0 & pos.y == 0) return new Vector2(-0.45f, 0.45f);
        if (pos.x == 1 & pos.y == 0) return new Vector2(-0.45f, 0.35f);
        if (pos.x == 2 & pos.y == 0) return new Vector2(-0.45f, 0.25f);
        if (pos.x == 3 & pos.y == 0) return new Vector2(-0.45f, 0.15f);
        if (pos.x == 4 & pos.y == 0) return new Vector2(-0.45f, 0.05f);
        if (pos.x == 5 & pos.y == 0) return new Vector2(-0.45f, -0.05f);
        if (pos.x == 6 & pos.y == 0) return new Vector2(-0.45f, -0.15f);
        if (pos.x == 7 & pos.y == 0) return new Vector2(-0.45f, -0.25f);
        if (pos.x == 8 & pos.y == 0) return new Vector2(-0.45f, -0.35f);
        if (pos.x == 9 & pos.y == 0) return new Vector2(-0.45f, -0.45f);

        if (pos.x == 0 & pos.y == 1) return new Vector2(-0.35f, 0.45f);
        if (pos.x == 1 & pos.y == 1) return new Vector2(-0.35f, 0.35f);
        if (pos.x == 2 & pos.y == 1) return new Vector2(-0.35f, 0.25f);
        if (pos.x == 3 & pos.y == 1) return new Vector2(-0.35f, 0.15f);
        if (pos.x == 4 & pos.y == 1) return new Vector2(-0.35f, 0.05f);
        if (pos.x == 5 & pos.y == 1) return new Vector2(-0.35f, -0.05f);
        if (pos.x == 6 & pos.y == 1) return new Vector2(-0.35f, -0.15f);
        if (pos.x == 7 & pos.y == 1) return new Vector2(-0.35f, -0.25f);
        if (pos.x == 8 & pos.y == 1) return new Vector2(-0.35f, -0.35f);
        if (pos.x == 9 & pos.y == 1) return new Vector2(-0.35f, -0.45f);

        if (pos.x == 0 & pos.y == 2) return new Vector2(-0.25f, 0.45f);
        if (pos.x == 1 & pos.y == 2) return new Vector2(-0.25f, 0.35f);
        if (pos.x == 2 & pos.y == 2) return new Vector2(-0.25f, 0.25f);
        if (pos.x == 3 & pos.y == 2) return new Vector2(-0.25f, 0.15f);
        if (pos.x == 4 & pos.y == 2) return new Vector2(-0.25f, 0.05f);
        if (pos.x == 5 & pos.y == 2) return new Vector2(-0.25f, -0.05f);
        if (pos.x == 6 & pos.y == 2) return new Vector2(-0.25f, -0.15f);
        if (pos.x == 7 & pos.y == 2) return new Vector2(-0.25f, -0.25f);
        if (pos.x == 8 & pos.y == 2) return new Vector2(-0.25f, -0.35f);
        if (pos.x == 9 & pos.y == 2) return new Vector2(-0.25f, -0.45f);

        if (pos.x == 0 & pos.y == 3) return new Vector2(-0.15f, 0.45f);
        if (pos.x == 1 & pos.y == 3) return new Vector2(-0.15f, 0.35f);
        if (pos.x == 2 & pos.y == 3) return new Vector2(-0.15f, 0.25f);
        if (pos.x == 3 & pos.y == 3) return new Vector2(-0.15f, 0.15f);
        if (pos.x == 4 & pos.y == 3) return new Vector2(-0.15f, 0.05f);
        if (pos.x == 5 & pos.y == 3) return new Vector2(-0.15f, -0.05f);
        if (pos.x == 6 & pos.y == 3) return new Vector2(-0.15f, -0.15f);
        if (pos.x == 7 & pos.y == 3) return new Vector2(-0.15f, -0.25f);
        if (pos.x == 8 & pos.y == 3) return new Vector2(-0.15f, -0.35f);
        if (pos.x == 9 & pos.y == 3) return new Vector2(-0.15f, -0.45f);

        if (pos.x == 0 & pos.y == 4) return new Vector2(-0.05f, 0.45f);
        if (pos.x == 1 & pos.y == 4) return new Vector2(-0.05f, 0.35f);
        if (pos.x == 2 & pos.y == 4) return new Vector2(-0.05f, 0.25f);
        if (pos.x == 3 & pos.y == 4) return new Vector2(-0.05f, 0.15f);
        if (pos.x == 4 & pos.y == 4) return new Vector2(-0.05f, 0.05f);
        if (pos.x == 5 & pos.y == 4) return new Vector2(-0.05f, -0.05f);
        if (pos.x == 6 & pos.y == 4) return new Vector2(-0.05f, -0.15f);
        if (pos.x == 7 & pos.y == 4) return new Vector2(-0.05f, -0.25f);
        if (pos.x == 8 & pos.y == 4) return new Vector2(-0.05f, -0.35f);
        if (pos.x == 9 & pos.y == 4) return new Vector2(-0.05f, -0.45f);

        if (pos.x == 0 & pos.y == 5) return new Vector2(0.05f, 0.45f);
        if (pos.x == 1 & pos.y == 5) return new Vector2(0.05f, 0.35f);
        if (pos.x == 2 & pos.y == 5) return new Vector2(0.05f, 0.25f);
        if (pos.x == 3 & pos.y == 5) return new Vector2(0.05f, 0.15f);
        if (pos.x == 4 & pos.y == 5) return new Vector2(0.05f, 0.05f);
        if (pos.x == 5 & pos.y == 5) return new Vector2(0.05f, -0.05f);
        if (pos.x == 6 & pos.y == 5) return new Vector2(0.05f, -0.15f);
        if (pos.x == 7 & pos.y == 5) return new Vector2(0.05f, -0.25f);
        if (pos.x == 8 & pos.y == 5) return new Vector2(0.05f, -0.35f);
        if (pos.x == 9 & pos.y == 5) return new Vector2(0.05f, -0.45f);

        if (pos.x == 0 & pos.y == 6) return new Vector2(0.15f, 0.45f);
        if (pos.x == 1 & pos.y == 6) return new Vector2(0.15f, 0.35f);
        if (pos.x == 2 & pos.y == 6) return new Vector2(0.15f, 0.25f);
        if (pos.x == 3 & pos.y == 6) return new Vector2(0.15f, 0.15f);
        if (pos.x == 4 & pos.y == 6) return new Vector2(0.15f, 0.05f);
        if (pos.x == 5 & pos.y == 6) return new Vector2(0.15f, -0.05f);
        if (pos.x == 6 & pos.y == 6) return new Vector2(0.15f, -0.15f);
        if (pos.x == 7 & pos.y == 6) return new Vector2(0.15f, -0.25f);
        if (pos.x == 8 & pos.y == 6) return new Vector2(0.15f, -0.35f);
        if (pos.x == 9 & pos.y == 6) return new Vector2(0.15f, -0.45f);

        if (pos.x == 0 & pos.y == 7) return new Vector2(0.25f, 0.45f);
        if (pos.x == 1 & pos.y == 7) return new Vector2(0.25f, 0.35f);
        if (pos.x == 2 & pos.y == 7) return new Vector2(0.25f, 0.25f);
        if (pos.x == 3 & pos.y == 7) return new Vector2(0.25f, 0.15f);
        if (pos.x == 4 & pos.y == 7) return new Vector2(0.25f, 0.05f);
        if (pos.x == 5 & pos.y == 7) return new Vector2(0.25f, -0.05f);
        if (pos.x == 6 & pos.y == 7) return new Vector2(0.25f, -0.15f);
        if (pos.x == 7 & pos.y == 7) return new Vector2(0.25f, -0.25f);
        if (pos.x == 8 & pos.y == 7) return new Vector2(0.25f, -0.35f);
        if (pos.x == 9 & pos.y == 7) return new Vector2(0.25f, -0.45f);

        if (pos.x == 0 & pos.y == 8) return new Vector2(0.35f, 0.45f);
        if (pos.x == 1 & pos.y == 8) return new Vector2(0.35f, 0.35f);
        if (pos.x == 2 & pos.y == 8) return new Vector2(0.35f, 0.25f);
        if (pos.x == 3 & pos.y == 8) return new Vector2(0.35f, 0.15f);
        if (pos.x == 4 & pos.y == 8) return new Vector2(0.35f, 0.05f);
        if (pos.x == 5 & pos.y == 8) return new Vector2(0.35f, -0.05f);
        if (pos.x == 6 & pos.y == 8) return new Vector2(0.35f, -0.15f);
        if (pos.x == 7 & pos.y == 8) return new Vector2(0.35f, -0.25f);
        if (pos.x == 8 & pos.y == 8) return new Vector2(0.35f, -0.35f);
        if (pos.x == 9 & pos.y == 8) return new Vector2(0.35f, -0.45f);

        if (pos.x == 0 & pos.y == 9) return new Vector2(0.45f, 0.45f);
        if (pos.x == 1 & pos.y == 9) return new Vector2(0.45f, 0.35f);
        if (pos.x == 2 & pos.y == 9) return new Vector2(0.45f, 0.25f);
        if (pos.x == 3 & pos.y == 9) return new Vector2(0.45f, 0.15f);
        if (pos.x == 4 & pos.y == 9) return new Vector2(0.45f, 0.05f);
        if (pos.x == 5 & pos.y == 9) return new Vector2(0.45f, -0.05f);
        if (pos.x == 6 & pos.y == 9) return new Vector2(0.45f, -0.15f);
        if (pos.x == 7 & pos.y == 9) return new Vector2(0.45f, -0.25f);
        if (pos.x == 8 & pos.y == 9) return new Vector2(0.45f, -0.35f);
        if (pos.x == 9 & pos.y == 9) return new Vector2(0.45f, -0.45f);

        // If no match
        return new Vector2(0, 0);
    }

    public Vector2 WorldToArrayPOS(Vector2 pos)
    {
        if (pos.x == -0.45f && pos.y == 0.45f) return new Vector2(0f, 0f);
        if (pos.x == -0.45f && pos.y == 0.35f) return new Vector2(0f, 1f);
        if (pos.x == -0.45f && pos.y == 0.25f) return new Vector2(0f, 2f);
        if (pos.x == -0.45f && pos.y == 0.15f) return new Vector2(0f, 3f);
        if (pos.x == -0.45f && pos.y == 0.05f) return new Vector2(0f, 4f);
        if (pos.x == -0.45f && pos.y == -0.05f) return new Vector2(0f, 5f);
        if (pos.x == -0.45f && pos.y == -0.15f) return new Vector2(0f, 6f);
        if (pos.x == -0.45f && pos.y == -0.25f) return new Vector2(0f, 7f);
        if (pos.x == -0.45f && pos.y == -0.35f) return new Vector2(0f, 8f);
        if (pos.x == -0.45f && pos.y == -0.45f) return new Vector2(0f, 09f);

        if (pos.x == -0.35f && pos.y == 0.45f) return new Vector2(1f, 0f);
        if (pos.x == -0.35f && pos.y == 0.35f) return new Vector2(1f, 1f);
        if (pos.x == -0.35f && pos.y == 0.25f) return new Vector2(1f, 2f);
        if (pos.x == -0.35f && pos.y == 0.15f) return new Vector2(1f, 3f);
        if (pos.x == -0.35f && pos.y == 0.05f) return new Vector2(1f, 4f);
        if (pos.x == -0.35f && pos.y == -0.05f) return new Vector2(1f, 5f);
        if (pos.x == -0.35f && pos.y == -0.15f) return new Vector2(1f, 6f);
        if (pos.x == -0.35f && pos.y == -0.25f) return new Vector2(1f, 7f);
        if (pos.x == -0.35f && pos.y == -0.35f) return new Vector2(1f, 8f);
        if (pos.x == -0.35f && pos.y == -0.45f) return new Vector2(1f, 9f);

        if (pos.x == -0.25f && pos.y == 0.45f) return new Vector2(2f, 0f);
        if (pos.x == -0.25f && pos.y == 0.35f) return new Vector2(2f, 1f);
        if (pos.x == -0.25f && pos.y == 0.25f) return new Vector2(2f, 2f);
        if (pos.x == -0.25f && pos.y == 0.15f) return new Vector2(2f, 3f);
        if (pos.x == -0.25f && pos.y == 0.05f) return new Vector2(2f, 4f);
        if (pos.x == -0.25f && pos.y == -0.05f) return new Vector2(2f, 5f);
        if (pos.x == -0.25f && pos.y == -0.15f) return new Vector2(2f, 6f);
        if (pos.x == -0.25f && pos.y == -0.25f) return new Vector2(2f, 7f);
        if (pos.x == -0.25f && pos.y == -0.35f) return new Vector2(2f, 8f);
        if (pos.x == -0.25f && pos.y == -0.45f) return new Vector2(2f, 9f);

        if (pos.x == -0.15f && pos.y == 0.45f) return new Vector2(3f, 0f);
        if (pos.x == -0.15f && pos.y == 0.35f) return new Vector2(3f, 1f);
        if (pos.x == -0.15f && pos.y == 0.25f) return new Vector2(3f, 2f);
        if (pos.x == -0.15f && pos.y == 0.15f) return new Vector2(3f, 3f);
        if (pos.x == -0.15f && pos.y == 0.05f) return new Vector2(3f, 4f);
        if (pos.x == -0.15f && pos.y == -0.05f) return new Vector2(3f, 5f);
        if (pos.x == -0.15f && pos.y == -0.15f) return new Vector2(3f, 6f);
        if (pos.x == -0.15f && pos.y == -0.25f) return new Vector2(3f, 7f);
        if (pos.x == -0.15f && pos.y == -0.35f) return new Vector2(3f, 8f);
        if (pos.x == -0.15f && pos.y == -0.45f) return new Vector2(3f, 9f);

        if (pos.x == -0.05f && pos.y == 0.45f) return new Vector2(4f, 0f);
        if (pos.x == -0.05f && pos.y == 0.35f) return new Vector2(4f, 1f);
        if (pos.x == -0.05f && pos.y == 0.25f) return new Vector2(4f, 2f);
        if (pos.x == -0.05f && pos.y == 0.15f) return new Vector2(4f, 3f);
        if (pos.x == -0.05f && pos.y == 0.05f) return new Vector2(4f, 4f);
        if (pos.x == -0.05f && pos.y == -0.05f) return new Vector2(4f, 5f);
        if (pos.x == -0.05f && pos.y == -0.15f) return new Vector2(4f, 6f);
        if (pos.x == -0.05f && pos.y == -0.25f) return new Vector2(4f, 7f);
        if (pos.x == -0.05f && pos.y == -0.35f) return new Vector2(4f, 8f);
        if (pos.x == -0.05f && pos.y == -0.45f) return new Vector2(4f, 9f);

        if (pos.x == 0.05f && pos.y == 0.45f) return new Vector2(5f, 0f);
        if (pos.x == 0.05f && pos.y == 0.35f) return new Vector2(5f, 1f);
        if (pos.x == 0.05f && pos.y == 0.25f) return new Vector2(5f, 2f);
        if (pos.x == 0.05f && pos.y == 0.15f) return new Vector2(5f, 3f);
        if (pos.x == 0.05f && pos.y == 0.05f) return new Vector2(5f, 4f);
        if (pos.x == 0.05f && pos.y == -0.05f) return new Vector2(5f, 5f);
        if (pos.x == 0.05f && pos.y == -0.15f) return new Vector2(5f, 6f);
        if (pos.x == 0.05f && pos.y == -0.25f) return new Vector2(5f, 7f);
        if (pos.x == 0.05f && pos.y == -0.35f) return new Vector2(5f, 8f);
        if (pos.x == 0.05f && pos.y == -0.45f) return new Vector2(5f, 9f);

        if (pos.x == 0.15f && pos.y == 0.45f) return new Vector2(6f, 0f);
        if (pos.x == 0.15f && pos.y == 0.35f) return new Vector2(6f, 1f);
        if (pos.x == 0.15f && pos.y == 0.25f) return new Vector2(6f, 2f);
        if (pos.x == 0.15f && pos.y == 0.15f) return new Vector2(6f, 3f);
        if (pos.x == 0.15f && pos.y == 0.05f) return new Vector2(6f, 4f);
        if (pos.x == 0.15f && pos.y == -0.05f) return new Vector2(6f, 5f);
        if (pos.x == 0.15f && pos.y == -0.15f) return new Vector2(6f, 6f);
        if (pos.x == 0.15f && pos.y == -0.25f) return new Vector2(6f, 7f);
        if (pos.x == 0.15f && pos.y == -0.35f) return new Vector2(6f, 8f);
        if (pos.x == 0.15f && pos.y == -0.45f) return new Vector2(6f, 9f);

        if (pos.x == 0.25f && pos.y == 0.45f) return new Vector2(7f, 0f);
        if (pos.x == 0.25f && pos.y == 0.35f) return new Vector2(7f, 1f);
        if (pos.x == 0.25f && pos.y == 0.25f) return new Vector2(7f, 2f);
        if (pos.x == 0.25f && pos.y == 0.15f) return new Vector2(7f, 3f);
        if (pos.x == 0.25f && pos.y == 0.05f) return new Vector2(7f, 4f);
        if (pos.x == 0.25f && pos.y == -0.05f) return new Vector2(7f, 5f);
        if (pos.x == 0.25f && pos.y == -0.15f) return new Vector2(7f, 6f);
        if (pos.x == 0.25f && pos.y == -0.25f) return new Vector2(7f, 7f);
        if (pos.x == 0.25f && pos.y == -0.35f) return new Vector2(7f, 8f);
        if (pos.x == 0.25f && pos.y == -0.45f) return new Vector2(7f, 9f);

        if (pos.x == 0.35f && pos.y == 0.45f) return new Vector2(8f, 0f);
        if (pos.x == 0.35f && pos.y == 0.35f) return new Vector2(8f, 1f);
        if (pos.x == 0.35f && pos.y == 0.25f) return new Vector2(8f, 2f);
        if (pos.x == 0.35f && pos.y == 0.15f) return new Vector2(8f, 3f);
        if (pos.x == 0.35f && pos.y == 0.05f) return new Vector2(8f, 4f);
        if (pos.x == 0.35f && pos.y == -0.05f) return new Vector2(8f, 5f);
        if (pos.x == 0.35f && pos.y == -0.15f) return new Vector2(8f, 6f);
        if (pos.x == 0.35f && pos.y == -0.25f) return new Vector2(8f, 7f);
        if (pos.x == 0.35f && pos.y == -0.35f) return new Vector2(8f, 8f);
        if (pos.x == 0.35f && pos.y == -0.45f) return new Vector2(8f, 9f);

        if (pos.x == 0.45f && pos.y == 0.45f) return new Vector2(9f, 0f);
        if (pos.x == 0.45f && pos.y == 0.35f) return new Vector2(9f, 1f);
        if (pos.x == 0.45f && pos.y == 0.25f) return new Vector2(9f, 2f);
        if (pos.x == 0.45f && pos.y == 0.15f) return new Vector2(9f, 3f);
        if (pos.x == 0.45f && pos.y == 0.05f) return new Vector2(9f, 4f);
        if (pos.x == 0.45f && pos.y == -0.05f) return new Vector2(9f, 5f);
        if (pos.x == 0.45f && pos.y == -0.15f) return new Vector2(9f, 6f);
        if (pos.x == 0.45f && pos.y == -0.25f) return new Vector2(9f, 7f);
        if (pos.x == 0.45f && pos.y == -0.35f) return new Vector2(9f, 8f);
        if (pos.x == 0.45f && pos.y == -0.45f) return new Vector2(9f, 9f);

        return new Vector2(0, 0);
    }


    public void AddItemToRoomContents(Vector3 pos, char item)
    {
        if (pos.x == -0.45f && pos.y == 0.45f) RoomContents[0, 0] = item;
        if (pos.x == -0.45f && pos.y == 0.35f) RoomContents[0, 1] = item;
        if (pos.x == -0.45f && pos.y == 0.25f) RoomContents[0, 2] = item;
        if (pos.x == -0.45f && pos.y == 0.15f) RoomContents[0, 3] = item;
        if (pos.x == -0.45f && pos.y == 0.05f) RoomContents[0, 4] = item;
        if (pos.x == -0.45f && pos.y == -0.05f) RoomContents[0, 5] = item;
        if (pos.x == -0.45f && pos.y == -0.15f) RoomContents[0, 6] = item;
        if (pos.x == -0.45f && pos.y == -0.25f) RoomContents[0, 7] = item;
        if (pos.x == -0.45f && pos.y == -0.35f) RoomContents[0, 8] = item;
        if (pos.x == -0.45f && pos.y == -0.45f) RoomContents[0, 9] = item;

        if (pos.x == -0.35f && pos.y == 0.45f) RoomContents[1, 0] = item;
        if (pos.x == -0.35f && pos.y == 0.35f) RoomContents[1, 1] = item;
        if (pos.x == -0.35f && pos.y == 0.25f) RoomContents[1, 2] = item;
        if (pos.x == -0.35f && pos.y == 0.15f) RoomContents[1, 3] = item;
        if (pos.x == -0.35f && pos.y == 0.05f) RoomContents[1, 4] = item;
        if (pos.x == -0.35f && pos.y == -0.05f) RoomContents[1, 5] = item;
        if (pos.x == -0.35f && pos.y == -0.15f) RoomContents[1, 6] = item;
        if (pos.x == -0.35f && pos.y == -0.25f) RoomContents[1, 7] = item;
        if (pos.x == -0.35f && pos.y == -0.35f) RoomContents[1, 8] = item;
        if (pos.x == -0.35f && pos.y == -0.45f) RoomContents[1, 9] = item;

        if (pos.x == -0.25f && pos.y == 0.45f) RoomContents[2, 0] = item;
        if (pos.x == -0.25f && pos.y == 0.35f) RoomContents[2, 1] = item;
        if (pos.x == -0.25f && pos.y == 0.25f) RoomContents[2, 2] = item;
        if (pos.x == -0.25f && pos.y == 0.15f) RoomContents[2, 3] = item;
        if (pos.x == -0.25f && pos.y == 0.05f) RoomContents[2, 4] = item;
        if (pos.x == -0.25f && pos.y == -0.05f) RoomContents[2, 5] = item;
        if (pos.x == -0.25f && pos.y == -0.15f) RoomContents[2, 6] = item;
        if (pos.x == -0.25f && pos.y == -0.25f) RoomContents[2, 7] = item;
        if (pos.x == -0.25f && pos.y == -0.35f) RoomContents[2, 8] = item;
        if (pos.x == -0.25f && pos.y == -0.45f) RoomContents[2, 9] = item;

        if (pos.x == -0.15f && pos.y == 0.45f) RoomContents[3, 0] = item;
        if (pos.x == -0.15f && pos.y == 0.35f) RoomContents[3, 1] = item;
        if (pos.x == -0.15f && pos.y == 0.25f) RoomContents[3, 2] = item;
        if (pos.x == -0.15f && pos.y == 0.15f) RoomContents[3, 3] = item;
        if (pos.x == -0.15f && pos.y == 0.05f) RoomContents[3, 4] = item;
        if (pos.x == -0.15f && pos.y == -0.05f) RoomContents[3, 5] = item;
        if (pos.x == -0.15f && pos.y == -0.15f) RoomContents[3, 6] = item;
        if (pos.x == -0.15f && pos.y == -0.25f) RoomContents[3, 7] = item;
        if (pos.x == -0.15f && pos.y == -0.35f) RoomContents[3, 8] = item;
        if (pos.x == -0.15f && pos.y == -0.45f) RoomContents[3, 9] = item;

        if (pos.x == -0.05f && pos.y == 0.45f) RoomContents[4, 0] = item;
        if (pos.x == -0.05f && pos.y == 0.35f) RoomContents[4, 1] = item;
        if (pos.x == -0.05f && pos.y == 0.25f) RoomContents[4, 2] = item;
        if (pos.x == -0.05f && pos.y == 0.15f) RoomContents[4, 3] = item;
        if (pos.x == -0.05f && pos.y == 0.05f) RoomContents[4, 4] = item;
        if (pos.x == -0.05f && pos.y == -0.05f) RoomContents[4, 5] = item;
        if (pos.x == -0.05f && pos.y == -0.15f) RoomContents[4, 6] = item;
        if (pos.x == -0.05f && pos.y == -0.25f) RoomContents[4, 7] = item;
        if (pos.x == -0.05f && pos.y == -0.35f) RoomContents[4, 8] = item;
        if (pos.x == -0.05f && pos.y == -0.45f) RoomContents[4, 9] = item;

        if (pos.x == 0.05f && pos.y == 0.45f) RoomContents[5, 0] = item;
        if (pos.x == 0.05f && pos.y == 0.35f) RoomContents[5, 1] = item;
        if (pos.x == 0.05f && pos.y == 0.25f) RoomContents[5, 2] = item;
        if (pos.x == 0.05f && pos.y == 0.15f) RoomContents[5, 3] = item;
        if (pos.x == 0.05f && pos.y == 0.05f) RoomContents[5, 4] = item;
        if (pos.x == 0.05f && pos.y == -0.05f) RoomContents[5, 5] = item;
        if (pos.x == 0.05f && pos.y == -0.15f) RoomContents[5, 6] = item;
        if (pos.x == 0.05f && pos.y == -0.25f) RoomContents[5, 7] = item;
        if (pos.x == 0.05f && pos.y == -0.35f) RoomContents[5, 8] = item;
        if (pos.x == 0.05f && pos.y == -0.45f) RoomContents[5, 9] = item;

        if (pos.x == 0.15f && pos.y == 0.45f) RoomContents[6, 0] = item;
        if (pos.x == 0.15f && pos.y == 0.35f) RoomContents[6, 1] = item;
        if (pos.x == 0.15f && pos.y == 0.25f) RoomContents[6, 2] = item;
        if (pos.x == 0.15f && pos.y == 0.15f) RoomContents[6, 3] = item;
        if (pos.x == 0.15f && pos.y == 0.05f) RoomContents[6, 4] = item;
        if (pos.x == 0.15f && pos.y == -0.05f) RoomContents[6, 5] = item;
        if (pos.x == 0.15f && pos.y == -0.15f) RoomContents[6, 6] = item;
        if (pos.x == 0.15f && pos.y == -0.25f) RoomContents[6, 7] = item;
        if (pos.x == 0.15f && pos.y == -0.35f) RoomContents[6, 8] = item;
        if (pos.x == 0.15f && pos.y == -0.45f) RoomContents[6, 9] = item;

        if (pos.x == 0.25f && pos.y == 0.45f) RoomContents[7, 0] = item;
        if (pos.x == 0.25f && pos.y == 0.35f) RoomContents[7, 1] = item;
        if (pos.x == 0.25f && pos.y == 0.25f) RoomContents[7, 2] = item;
        if (pos.x == 0.25f && pos.y == 0.15f) RoomContents[7, 3] = item;
        if (pos.x == 0.25f && pos.y == 0.05f) RoomContents[7, 4] = item;
        if (pos.x == 0.25f && pos.y == -0.05f) RoomContents[7, 5] = item;
        if (pos.x == 0.25f && pos.y == -0.15f) RoomContents[7, 6] = item;
        if (pos.x == 0.25f && pos.y == -0.25f) RoomContents[7, 7] = item;
        if (pos.x == 0.25f && pos.y == -0.35f) RoomContents[7, 8] = item;
        if (pos.x == 0.25f && pos.y == -0.45f) RoomContents[7, 9] = item;

        if (pos.x == 0.35f && pos.y == 0.45f) RoomContents[8, 0] = item;
        if (pos.x == 0.35f && pos.y == 0.35f) RoomContents[8, 1] = item;
        if (pos.x == 0.35f && pos.y == 0.25f) RoomContents[8, 2] = item;
        if (pos.x == 0.35f && pos.y == 0.15f) RoomContents[8, 3] = item;
        if (pos.x == 0.35f && pos.y == 0.05f) RoomContents[8, 4] = item;
        if (pos.x == 0.35f && pos.y == -0.05f) RoomContents[8, 5] = item;
        if (pos.x == 0.35f && pos.y == -0.15f) RoomContents[8, 6] = item;
        if (pos.x == 0.35f && pos.y == -0.25f) RoomContents[8, 7] = item;
        if (pos.x == 0.35f && pos.y == -0.35f) RoomContents[8, 8] = item;
        if (pos.x == 0.35f && pos.y == -0.45f) RoomContents[8, 9] = item;

        if (pos.x == 0.45f && pos.y == 0.45f) RoomContents[9, 0] = item;
        if (pos.x == 0.45f && pos.y == 0.35f) RoomContents[9, 1] = item;
        if (pos.x == 0.45f && pos.y == 0.25f) RoomContents[9, 2] = item;
        if (pos.x == 0.45f && pos.y == 0.15f) RoomContents[9, 3] = item;
        if (pos.x == 0.45f && pos.y == 0.05f) RoomContents[9, 4] = item;
        if (pos.x == 0.45f && pos.y == -0.05f) RoomContents[9, 5] = item;
        if (pos.x == 0.45f && pos.y == -0.15f) RoomContents[9, 6] = item;
        if (pos.x == 0.45f && pos.y == -0.25f) RoomContents[9, 7] = item;
        if (pos.x == 0.45f && pos.y == -0.35f) RoomContents[9, 8] = item;
        if (pos.x == 0.45f && pos.y == -0.45f) RoomContents[9, 9] = item;

    }
}