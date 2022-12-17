using UnityEngine;

public class BowPickup : MonoBehaviour
{
    public Helper Helper;
    public GameObject Player;
    public PlayerCombat PlayerCombat;
    public Human PlayerIsHuman;
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
        if (!PlayerCombat || !PlayerIsHuman)
        {
            PlayerCombat = Player.GetComponent<PlayerCombat>();
            PlayerIsHuman = Player.GetComponent<Human>();
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
        PlayerIsHuman.BowController = bow;
        PlayerIsHuman.Bow = bow.transform.Find("Bow").gameObject;
        PlayerIsHuman.BowController.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && PlayerCombat.RangedWeaponEquipped == false && GameManager.currentHost == HostType.Human)
        {
            GameManager.AddArrows(5);
            PlayerCombat.SetRangedWeaponEquipped(true);

            // add shortbow game object to player game object
            GameObject a = Instantiate(
                ShortBow,
                Player.transform.position,
                Player.transform.rotation,
                Player.transform);

            a.name = "BowAim";
            PlayerIsHuman.BowController = a;
            PlayerIsHuman.Bow = a.transform.Find("Bow").gameObject;
            PlayerIsHuman.SetRangedAsActiveWeapon();
            Destroy(this.gameObject);


        }
        else if (other.tag == "Player" && PlayerCombat.RangedWeaponEquipped == true && GameManager.currentHost == HostType.Human)
        {
            GameManager.AddArrows(5);
            Destroy(this.gameObject);
        }

    }
}