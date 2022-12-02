using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper : MonoBehaviour
{
    public GameObject Camera = null;
    public GameObject CameraBox = null;
    public GameManager GameManager = null;
    public AudioManager AudioManager = null;
    public Vector3 PlayerPosition;
    public GameObject chest = null;
    public GameObject wall = null;
    public GameObject flameBowl = null;
    public GameObject arrowTrap = null;
    public GameObject Player;
    public const string PlayerTag = "Player";
    public GameObject SimpleRoomPrefab;

    public enum DamageTypes
    {
        Melee,
        Ranged,
        WormPoison,
        Debug,
        PlayerArrow,
        FlamingPlayerArrow,
        GoldArrow,
        PlayerSword,
        TrapArrow,
        WallOfDeath
    }

    public enum EnemyTypes
    {
        Worm,
        Ghost,
        MiniBoss,
        Boss,
        Human,
        Trap
    }

    void Start()
    {
    }

    void Update()
    {
        if (!Player) Player = GameObject.FindGameObjectWithTag("Player");
        if (Player) PlayerPosition = Player.transform.position;
    }
}
