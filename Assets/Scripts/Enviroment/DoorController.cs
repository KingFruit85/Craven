using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private Helper helper;
    public List<GameObject> doors = new List<GameObject>();
    public Collider2D[] enemiesInRoom;
    public EnemySpawner enemySpawner;
    public Collider2D playerInRoom;
    public LayerMask enemyLayer, playerLayer;
    public GameObject topLeft, bottomRight, camAnchor;
    public enum OpenCondition
    {
        MobDeath,
        PuzzleComplete
    }
    public OpenCondition openCondition;
    public bool roomComplete = false;

    void Awake()
    {
        helper = GameObject.FindGameObjectWithTag("Helper").GetComponent<Helper>();
        helper.CameraBox.transform.position = camAnchor.transform.position;
    }

    // Do not delete, called below by Invoke
    public void closeThisRoomsDoors()
    {
        foreach (var door in doors)
        {
            if (door.activeSelf)
            {
                door.GetComponent<Door>().CloseDoor();
            }
        }
    }

    // Do not delete, called below by Invoke
    public void openThisRoomsDoors()
    {
        foreach (var door in doors)
        {
            if (door.activeSelf)
            {
                door.GetComponent<Door>().OpenDoor();
            }
        }
    }

    void Update()
    {
        if (!enemySpawner) enemySpawner = transform.parent.GetComponent<EnemySpawner>();

        // Handles player entering a new room
        playerInRoom = Physics2D.OverlapArea(topLeft.transform.position, bottomRight.transform.position, playerLayer);
        enemiesInRoom = Physics2D.OverlapAreaAll(topLeft.transform.position, bottomRight.transform.position, enemyLayer);

        if (playerInRoom)
        {
            helper.CameraBox.transform.position = camAnchor.transform.position;
            helper.Camera.transform.position = camAnchor.transform.position;
            helper.Player.transform.parent = transform.parent;

            if (!roomComplete)
            {
                // Closes the door shortly after the player enters
                foreach (var door in doors)
                {
                    if (door.activeSelf)
                    {
                        Invoke("closeThisRoomsDoors", 0.2f);
                    }
                }
            }
        }

        if (!playerInRoom && !roomComplete)
        {
            Invoke("openThisRoomsDoors", 1f);
        }

        if (openCondition == OpenCondition.MobDeath && enemiesInRoom.Length <= 0 && playerInRoom)
        {
            roomComplete = true;
            Invoke("openThisRoomsDoors", 1f);
        }
    }
}