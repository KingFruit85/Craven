using UnityEngine;

public class BowPickup : MonoBehaviour
{
    public Helper Helper;
    public GameObject Player;
    public PlayerCombat PlayerCombat;
    public Human Human;
    public GameObject ShortBow;
    public GameManager GameManager;

    private void OnTriggerEnter2D(Collider2D other)
    {

        Helper = GameObject.FindGameObjectWithTag("Helper").GetComponent<Helper>();

        if (!Player)
        {
            Player = Helper.Player;
            PlayerCombat = Player.GetComponent<PlayerCombat>();
            Human = Player.GetComponent<Human>();

        }

        ShortBow = Resources.Load("BowAim") as GameObject;

        if (!GameManager)
        {
            GameManager = Helper.GameManager;
        }

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

        DamagePopup.CreatePickupMessage(transform.position, "Picked up bow!");

    }
}