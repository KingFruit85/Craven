using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSprites : MonoBehaviour
{
    [SerializeField]
    public Sprite[][] level1 = new Sprite[7][];
    public Sprite[] bottomWalls;
    public Sprite[] critBottomWalls;
    public Sprite[] topWalls;
    public Sprite[] critTopWalls;
    public Sprite[] leftWalls;
    public Sprite[] critLeftWalls;
    public Sprite[] rightWalls;
    public Sprite[] critRightWalls;
    public Sprite[] corners;
    public Sprite[] critTopLeftCorners;
    public Sprite[] critTopRightCorners;
    public Sprite[] critBottomLeftCorners;
    public Sprite[] critBottomRightCorners;
    public Sprite[] floor;
    public Sprite[] critFloor;
    public Sprite[] miscWalls;

    public Sprite[] floorDressing;

    void Start()
    {
        transform.parent = GameObject.Find("Rooms").transform;
    }

    public void LoadLevel1Sprites()
    {
        // 0 - Bottom Walls
        level1[0] = new Sprite[3];
        level1[0][0] = Resources.Load<Sprite>("Level1Sprites/BottomMudWall1");
        level1[0][1] = Resources.Load<Sprite>("Level1Sprites/BottomMudWall2");
        level1[0][2] = Resources.Load<Sprite>("Level1Sprites/BottomMudWall3");

        // 1 - Top Walls
        level1[1] = new Sprite[3];
        level1[1][0] = Resources.Load<Sprite>("Level1Sprites/TopMudWall1");
        level1[1][1] = Resources.Load<Sprite>("Level1Sprites/TopMudWall2");
        level1[1][2] = Resources.Load<Sprite>("Level1Sprites/TopMudWall3");

        // 2 - LeftWalls
        level1[2] = new Sprite[4];
        level1[2][0] = Resources.Load<Sprite>("Level1Sprites/LeftMudWall1");
        level1[2][1] = Resources.Load<Sprite>("Level1Sprites/LeftMudWall2");
        level1[2][2] = Resources.Load<Sprite>("Level1Sprites/LeftMudWall3");
        level1[2][3] = Resources.Load<Sprite>("Level1Sprites/LeftMudWall4");

        // 3 - RightWalls
        level1[3] = new Sprite[2];
        level1[3][0] = Resources.Load<Sprite>("Level1Sprites/RightMudWall1");
        level1[3][1] = Resources.Load<Sprite>("Level1Sprites/RightMudWall2");

        // 4 - Corners
        level1[4] = new Sprite[4];
        level1[4][0] = Resources.Load<Sprite>("Level1Sprites/MudCorner1");
        level1[4][1] = Resources.Load<Sprite>("Level1Sprites/MudCorner2");
        level1[4][2] = Resources.Load<Sprite>("Level1Sprites/MudCorner3");
        level1[4][3] = Resources.Load<Sprite>("Level1Sprites/MudCorner4");

        // 5 - Floor Tiles
        level1[5] = new Sprite[16];
        level1[5][0]  = Resources.Load<Sprite>("Level1Sprites/mudtiles1");
        level1[5][1]  = Resources.Load<Sprite>("Level1Sprites/mudtiles2");
        level1[5][2]  = Resources.Load<Sprite>("Level1Sprites/mudtiles3");
        level1[5][3]  = Resources.Load<Sprite>("Level1Sprites/mudtiles4");
        level1[5][4]  = Resources.Load<Sprite>("Level1Sprites/mudtiles5");
        level1[5][5]  = Resources.Load<Sprite>("Level1Sprites/mudtiles6");
        // level1[5][6]  = Resources.Load<Sprite>("Level1Sprites/mudtiles7");
        // level1[5][7]  = Resources.Load<Sprite>("Level1Sprites/mudtiles8");
        // level1[5][8]  = Resources.Load<Sprite>("Level1Sprites/mudtiles9");
        // level1[5][9]  = Resources.Load<Sprite>("Level1Sprites/mudtiles10");
        // level1[5][10] = Resources.Load<Sprite>("Level1Sprites/mudtiles11");
        // level1[5][11] = Resources.Load<Sprite>("Level1Sprites/mudtiles12");
        // level1[5][12] = Resources.Load<Sprite>("Level1Sprites/mudtiles13");
        // level1[5][13] = Resources.Load<Sprite>("Level1Sprites/mudtiles14");
        // level1[5][14] = Resources.Load<Sprite>("Level1Sprites/mudtiles15");
        // level1[5][15] = Resources.Load<Sprite>("Level1Sprites/mudtiles16"); 

        // 6 - Misc Tiles
        level1[6] = new Sprite[14];
        level1[6][0]  = Resources.Load<Sprite>("Level1Sprites/BottomLeftMudWall");
        level1[6][1]  = Resources.Load<Sprite>("Level1Sprites/BottomRightMudWall");
        level1[6][2]  = Resources.Load<Sprite>("Level1Sprites/TopLeftMudWall");
        level1[6][3]  = Resources.Load<Sprite>("Level1Sprites/TopRightMudWall");
        level1[6][4]  = Resources.Load<Sprite>("Level1Sprites/TopBottomMudWall1");
        level1[6][5]  = Resources.Load<Sprite>("Level1Sprites/TopBottomMudWall2");
        level1[6][6]  = Resources.Load<Sprite>("Level1Sprites/TopBottomMudWall3");
        level1[6][7]  = Resources.Load<Sprite>("Level1Sprites/TopBottomLeftMudWall");
        level1[6][8]  = Resources.Load<Sprite>("Level1Sprites/TopBottomRightMudWall");
        level1[6][9]  = Resources.Load<Sprite>("Level1Sprites/RightBottomMudWall");
        level1[6][10]  = Resources.Load<Sprite>("Level1Sprites/TopLeftRightMudWall");
        level1[6][11]  = Resources.Load<Sprite>("Level1Sprites/LeftRightMudWall");
        level1[6][12]  = Resources.Load<Sprite>("Level1Sprites/BottomLeftRightMudWall");
        level1[6][13]  = Resources.Load<Sprite>("Level1Sprites/TopBottomLeftRightMudWall");



        



        


        


    }
}
