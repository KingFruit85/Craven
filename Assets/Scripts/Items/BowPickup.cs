using UnityEngine;

public class BowPickup : MonoBehaviour
{
    public GameObject player;
    public PlayerCombat playerCombat;
    public Human playerHuman;
    public GameObject shortBow;
    public GameManager gameManager;

    void Awake()
    {
        shortBow = Resources.Load("BowAim") as GameObject;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        if (!player)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        if (!playerCombat || !playerHuman)
        {
            playerCombat = player.GetComponent<PlayerCombat>();
            playerHuman = player.GetComponent<Human>();
        }
    }

    public void AddBowToPlayer()
    {
        playerCombat.SetRangedWeaponEquipped(true);

        // add shortbow game object to player game object
        GameObject bow = Instantiate(
            shortBow,
            player.transform.position,
            Quaternion.identity,
            transform);

        bow.name = "BowAim";
        playerHuman.bowAim = bow;
        playerHuman.bow = bow.transform.Find("Bow").gameObject;
        playerHuman.bowAim.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && playerCombat.rangedWeaponEquipped == false && gameManager.currentHost == "Human")
        {
            gameManager.AddArrows(5);
            playerCombat.SetRangedWeaponEquipped(true);

            // add shortbow game object to player game object
            GameObject a = Instantiate(
                shortBow,
                player.transform.position,
                player.transform.rotation,
                player.transform);

            a.name = "BowAim";
            //Updates the gameobject variable in <Human>
            playerHuman.bowAim = a;
            playerHuman.bow = a.transform.Find("Bow").gameObject;
            //Resets the scale, for some reason it spawns tiny on the player without this. Probably just need to change the scale on teh sprite but I'm being lazy
            playerHuman.bowAim.transform.localScale = new Vector3(1.2f, 1.2f, 0);
            playerHuman.SetRangedAsActiveWeapon();
            //Removes the pickup sprite from the level
            Destroy(this.gameObject);


        }
        else if (other.tag == "Player" && playerCombat.rangedWeaponEquipped == true && gameManager.currentHost == "Human")
        {
            gameManager.AddArrows(5);
            Destroy(this.gameObject);
        }

    }
}