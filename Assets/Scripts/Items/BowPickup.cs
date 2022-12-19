using UnityEngine;

public class BowPickup : MonoBehaviour
{
    public Helper Helper;
    public GameObject Player;
    public PlayerCombat PlayerCombat;
    public Human Human;
    public GameObject ShortBow;
    public GameManager GameManager;

    void Awake()
    {
        Helper = GameObject.FindGameObjectWithTag("Helper").GetComponent<Helper>();
        Player = Helper.Player;
        ShortBow = Resources.Load("BowAim") as GameObject;
        GameManager = Helper.GameManager;
    }

    void Update()
    {
        if (!Player)
        {
            Player = Helper.Player;
        }
        if (!PlayerCombat || !Human)
        {
            PlayerCombat = Player.GetComponent<PlayerCombat>();
            Human = Player.GetComponent<Human>();
        }
    }

    public void AddBowToPlayer()
    {
        PlayerCombat.SetRangedWeaponEquipped(true);

        // add shortbow game object to player game object
        GameObject bow = Instantiate(
            ShortBow,
            Player.transform.position,
            Quaternion.identity,
            transform);

        bow.name = "BowAim";
        Human.BowController = bow;
        Human.Bow = bow.transform.Find("Bow").gameObject;
        Human.BowController.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!Human.HasPickedUpRangedWeapon && GameManager.currentHost == HostType.Human)
        {
            GameManager.AddArrows(5);
            Human.HasPickedUpRangedWeapon = true;

            // add shortbow game object to player game object
            GameObject a = Instantiate(
                ShortBow,
                Player.transform.position,
                Player.transform.rotation,
                Player.transform);

            a.name = "BowAim";
            Human.BowController = a;
            Human.Bow = a.transform.Find("Bow").gameObject;
            Human.SetRangedAsActiveWeapon();
            Destroy(this.gameObject);
        }
        else if (other.tag == "Player" && Human.HasPickedUpRangedWeapon == true && GameManager.currentHost == HostType.Human)
        {
            GameManager.AddArrows(5);
            Destroy(this.gameObject);
        }

    }
}