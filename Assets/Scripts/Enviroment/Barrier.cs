using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    public Sprite OpenSprite;
    public DoorController doorController;
    public SpriteRenderer sr;

    void Start()
    {
        doorController = transform.parent.parent.transform.Find("DoorController").GetComponent<DoorController>();
    }

    void Update()
    {

        if (doorController.roomComplete)
        {
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<SpriteRenderer>().sprite = OpenSprite;
        }
    }
}
