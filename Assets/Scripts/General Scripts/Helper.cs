using UnityEngine;

public class Helper : MonoBehaviour
{
    public static Helper instance;
    public GameObject Camera;
    public GameManager GameManager;
    public AudioManager AudioManager;
    public Vector3 PlayerPosition;
    public GameObject Chest;
    public GameObject Wall;
    public GameObject Flamebowl;
    public GameObject ArrowTrap;
    public GameObject Player;
    public GameObject[] Prefabs;
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

        // Prefabs = Resources.LoadAll<GameObject>("");
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
        if (!GameManager) GameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        if (!AudioManager) AudioManager = GameObject.FindObjectOfType<AudioManager>();
        if (!Chest) Chest = Resources.Load<GameObject>("chest");
        if (!Wall) Wall = Resources.Load<GameObject>("wall");
        if (!Flamebowl) Flamebowl = Resources.Load<GameObject>("FlameBowl");
        if (!ArrowTrap) ArrowTrap = Resources.Load<GameObject>("arrowTrap");
    }

    void Update()
    {
        if (!Player)
        {
            try
            {
                Player = GameObject.FindGameObjectWithTag("Player");
                if (!Player)
                {
                    Player = Resources.Load<GameObject>("Player Variant 1");
                }
            }
            catch (System.Exception)
            {
                throw;
            }

        }
        if (Player) PlayerPosition = Player.transform.position;
        if (!Camera) Camera = GameObject.FindGameObjectWithTag("MainCamera");
    }
}
