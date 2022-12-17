using System.Linq;
using UnityEngine;

public class StartRoom : MonoBehaviour
{
    private Helper Helper;
    private SimpleRoom Room;
    private DoorController DoorController;
    private GameManager GameManager;
    public GameObject Camera;
    private bool PlayerMovedToRoom;

    private void Awake()
    {
        Helper = GameObject.FindGameObjectWithTag("Helper").GetComponent<Helper>();
    }

    void Start()
    {
        Room = GetComponent<SimpleRoom>();
        DoorController = GetComponentInChildren<DoorController>();
        GameManager = Helper.GameManager;
        Camera = Helper.Camera;

        gameObject.transform.name += " START ROOM";
        DoorController.roomComplete = true;

        // Debug: Spawn kill square 
        Instantiate(Resources.Load("KillSquare"), Room.ExitTile.transform.position, Quaternion.identity);

        // Place level specific terminal with new lore
        if (GameManager.currentGameLevel == 1)
        {
            GameObject terminal = Instantiate(Resources.Load("InteractableRune1"), Room.ReturnTerminalSpawnLocation(), Quaternion.identity) as GameObject;
            terminal.transform.parent = gameObject.transform.Find("Tiles");
            terminal.transform.position = gameObject.transform.Find("CameraAnchor").transform.position;
        }
    }

    void Update()
    {
        // Move player to room
        if (Helper.Player && !PlayerMovedToRoom)
        {
            Helper.Player.transform.position = GetComponent<SimpleRoom>().SpawnableFloorTiles[1].transform.position;
            PlayerMovedToRoom = true;
        }

    }
}
