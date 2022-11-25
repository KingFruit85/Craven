using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmToken : MonoBehaviour
{
    public Transform myRoom;
    public FlameBowl[] flameBowls;

    void Start()
    {
        myRoom = transform.parent.transform;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            // GameObject.Find("GameManager").GetComponent<GameManager>().AddArrows(arrowCount);
            flameBowls = myRoom.GetComponentsInChildren<FlameBowl>();
            
            foreach (var fb in flameBowls)
            {
                fb.UnLight();
            }

            Destroy(gameObject);
        }
    }
}

