using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private SpriteRenderer sr;
    private GameManager gm;
    public Sprite open;
    public Sprite closed;
    public List<GameObject> treasure;
    private bool hasBeenOpened = false;
    private bool openButtonPressed = false;
    private Vector3 playerPOS;
    private float distanceBetween;
    public Door[] linkedDoors;
    public Barrier[] linkedbarriers;
    public FlameBowl[] linkedFlameBowls;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        open = Resources.Load<Sprite>("Level1Sprites/chest2");
        closed = Resources.Load<Sprite>("Level1Sprites/chest");
        sr.sprite = closed;
        var chestSpawnablesFolder = Directory.GetFiles(@".\Assets\Resources\ChestSpawnables");

        foreach (var item in chestSpawnablesFolder)
        {
            var i = Resources.Load<GameObject>(@$"ChestSpawnables\{item}");
            treasure.Add(i);
        }
    }

    private GameObject SpawnItem()
    {
        // If player doesn't have a bow yet, spawn a bow
        if (!gm.rangedWeaponEquipped)
        {
            return (GameObject)Resources.Load("BowPickup");
        }
        else
        {
            var R = Random.Range(0, treasure.Count);
            return treasure[R];
        }
    }

    void openChest()
    {
        sr.sprite = open;
        GameObject item = SpawnItem();
        var pos = transform.position;
        GameObject a = Instantiate
                            (
                                item,
                                new Vector3(pos.x, pos.y, pos.z),
                                transform.rotation
                            )
                            as GameObject;
        //sets flag to stop item spawning if chest is closes & reopened
        hasBeenOpened = true;
        //Spawn item on top of chest
        item.GetComponent<SpriteRenderer>().sortingOrder = 3;
    }

    void CloseLinkedDoors()
    {
        foreach (var door in linkedDoors)
        {
            door.CloseDoor();
        }
    }

    void OpenLinkedBarriers()
    {
        foreach (var barrier in linkedbarriers)
        {
            barrier.gameObject.SetActive(false);
        }
    }

    void LightFlameBowls()
    {
        foreach (var flameBowl in linkedFlameBowls)
        {
            flameBowl.Light();
        }
    }


    void Update()
    {
        playerPOS = GameObject.Find("Player").transform.position;
        distanceBetween = Vector2.Distance(transform.position, playerPOS);

        if (distanceBetween < 1f && Input.GetKeyDown(KeyCode.E))
        {
            openButtonPressed = true;
        }
        else
        {
            openButtonPressed = false;
        }

        if (openButtonPressed == true && hasBeenOpened == false)
        {
            openChest();

            if (linkedDoors.Length > 0 && linkedDoors != null)
            {
                CloseLinkedDoors();
            }

            if (linkedbarriers.Length > 0 && linkedbarriers != null)
            {
                OpenLinkedBarriers();
            }

            if (linkedFlameBowls.Length > 0 && linkedFlameBowls != null)
            {
                LightFlameBowls();
            }
        }
    }
}
