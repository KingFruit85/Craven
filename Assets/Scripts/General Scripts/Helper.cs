using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper : MonoBehaviour
{
    public GameObject Camera;
    public GameObject CameraBox;
    public GameManager GameManager;
    public AudioManager AudioManager;
    public Vector3 playerPosition;
    public GameObject chest;
    public GameObject wall;
    public GameObject flameBowl;
    public GameObject arrowTrap;

    void Start()
    {
        Camera = GameObject.FindGameObjectWithTag("MainCamera");
        CameraBox = GameObject.FindGameObjectWithTag("CameraBox");
        GameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        AudioManager = GameObject.FindObjectOfType<AudioManager>();
        chest = Resources.Load<GameObject>("chest");
        wall = Resources.Load<GameObject>("wall");
        flameBowl = Resources.Load<GameObject>("FlameBowl");
        arrowTrap = Resources.Load<GameObject>("arrowTrap");

    }

    void Update()
    {
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
    }
}
