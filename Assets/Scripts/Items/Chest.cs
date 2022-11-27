using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private Helper helper;
    private SpriteRenderer spriteRenderer;
    private GameManager gameManager;
    public Sprite open, closed;
    public List<GameObject> treasure;
    private bool chestOpen = false;
    private bool openButtonPressed = false;
    private bool playerInRange = false;

    void Awake()
    {
        helper = GameObject.FindGameObjectWithTag("Helper").GetComponent<Helper>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameManager = helper.GameManager;
        spriteRenderer.sprite = closed;
        var chestSpawnablesFolder = Directory.GetFiles(@".\Assets\Resources\ChestSpawnables");

        foreach (var item in chestSpawnablesFolder)
        {
            var i = Resources.Load<GameObject>(@$"ChestSpawnables\{item}");
            treasure.Add(i);
        }
    }

    private GameObject GetItem()
    {
        // If player doesn't have a bow yet, spawn a bow
        if (!gameManager.rangedWeaponEquipped)
        {
            return Resources.Load<GameObject>("BowPickup");
        }
        else
        {
            return treasure[Random.Range(0, treasure.Count)];
        }
    }

    void openChest()
    {
        spriteRenderer.sprite = open;
        GameObject a = Instantiate
                            (
                                GetItem(),
                                transform.position,
                                transform.rotation
                            )
                            as GameObject;
        a.GetComponent<SpriteRenderer>().sortingOrder = 3;
        chestOpen = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            playerInRange = false;
        }
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            openButtonPressed = true;
        }

        if (openButtonPressed && !chestOpen)
        {
            openChest();
        }
    }
}
