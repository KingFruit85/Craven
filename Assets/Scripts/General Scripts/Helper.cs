using UnityEngine;

public class Helper : MonoBehaviour
{
    public static Helper instance;
    public GameObject Camera;
    public GameObject CameraBox;
    public GameManager GameManager;
    public AudioManager AudioManager;
    public Vector3 PlayerPosition;
    public GameObject Chest;
    public GameObject Wall;
    public GameObject Flamebowl;
    public GameObject ArrowTrap;
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


    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (!Camera) Camera = GameObject.FindGameObjectWithTag("MainCamera");
        if (!CameraBox) CameraBox = GameObject.FindGameObjectWithTag("CameraBox");
        if (!GameManager) GameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        if (!AudioManager) AudioManager = GameObject.FindObjectOfType<AudioManager>();
        if (!Chest) Chest = Resources.Load<GameObject>("chest");
        if (!Wall) Wall = Resources.Load<GameObject>("wall");
        if (!Flamebowl) Flamebowl = Resources.Load<GameObject>("FlameBowl");
        if (!ArrowTrap) ArrowTrap = Resources.Load<GameObject>("arrowTrap");
    }

    void Update()
    {
        if (!Player) Player = GameObject.FindGameObjectWithTag("Player");
        if (Player) PlayerPosition = Player.transform.position;
    }
}
