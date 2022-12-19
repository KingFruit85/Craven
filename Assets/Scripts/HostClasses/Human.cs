using System.Collections;
using UnityEngine;
using static PlayerMovement;

public class Human : MonoBehaviour
{
    public string idleLeft = "Human_Idle_Left";
    public string idleRight = "Human_Idle_Right";
    public string walkLeft = "Human_Walk_Left";
    public string walkRight = "Human_Walk_Right";
    public string walkUp = "Human_Move_Up";
    public string walkDown = "Human_Move_Down";
    public string walkLeftNonBloodied = "Human_Walk_Left";
    public string walkRightNonBloodied = "Human_Walk_Right";
    public string walkUpNonBloodied = "Human_Move_Up";
    public string walkDownNonBloodied = "Human_Move_Down";

    public string walkDownBloodied = "Human_Move_Down_Bloodied";
    public string walkUpBloodied = "Human_Move_Up_Bloodied";
    public string walkLeftBloodied = "Human_Move_Left_Bloodied";
    public string walkRightBloodied = "Human_Move_Right_Bloodied";

    public string idleUp = "Human_Idle_Up";
    public string idleDown = "Human_Idle_Down";
    public string death = "Human_Death";
    public string attackLeft = "Sword_Stab_Left";
    public string attackRight = "Sword_Stab_Right";
    public string attackUp = "Human_Attack_Up";
    public string attackDown = "Human_Attack_Down";


    public GameObject Player;
    private Rigidbody2D Rigidbody2D;
    private Animator PlayerAnimator;
    private PlayAnimations PlayAnimations;
    private Shaker Shaker;
    public GameObject SwordController;
    public GameObject BowController;
    public GameObject Sword;
    public GameObject Bow;
    private AudioManager AudioManager;
    public Helper Helper;
    private GameManager GameManager;
    public PlayerMovement.Looking playerIsLooking;

    public bool SwordEquipped = true;
    public bool BowEquipped = false;
    public float MoveSpeed = 5;
    public int SwordDamage = 30;
    public float SwordRange = 0.5f;
    public int ArrowDamage = 10;
    public int ArrowSpeed = 10;
    public int CritModifier = 2;
    public float DashSpeed = 20f;
    private float DashCoolDown = -9999;
    public float DashDelay = .5f;
    private bool CanDash = true;
    private bool IsDashing = false;
    public bool HasPickedUpRangedWeapon { get; set; } = false;

    void Awake()
    {
        Sword = FindObjectOfType<Sword>().gameObject;
        SwordController = Sword.transform.parent.gameObject;
    }
    void Start()
    {
        Helper = GameObject.FindGameObjectWithTag("Helper").GetComponent<Helper>();
        Rigidbody2D = GetComponent<Rigidbody2D>();
        GameManager = Helper.GameManager;
        GameManager.currentHost = HostType.Human;
        PlayerAnimator = GetComponent<Animator>();
        Shaker = Helper.Camera.GetComponent<Shaker>();
        Player = Helper.Player;
        AudioManager = Helper.AudioManager;
        transform.localScale = new Vector3(2.5f, 2.5f, 0);

        GetComponent<PlayerMovement>().moveSpeed = MoveSpeed;

        //Set the player animations/sprites to the current host creature
        PlayAnimations = GetComponent<PlayAnimations>();
        PlayAnimations.idleLeft = idleLeft;
        PlayAnimations.idleRight = idleRight;
        PlayAnimations.walkLeft = walkLeft;
        PlayAnimations.walkRight = walkRight;
        PlayAnimations.walkUp = walkUp;
        PlayAnimations.walkDown = walkDown;
        PlayAnimations.death = death;
        PlayAnimations.attackLeft = attackLeft;
        PlayAnimations.attackRight = attackRight;
        PlayAnimations.attackUp = attackUp;
        PlayAnimations.attackDown = attackDown;
    }

    public void Dash()
    {
        var direction = GetComponent<PlayerMovement>().looking;
        if (CanDash)
        {
            // Set users in dashing state/invincibility state for a frame or two
            CanDash = false;
            StartCoroutine(ToggleIsDashingBool());

            switch (direction)
            {
                default: throw new System.Exception("invalid dash direction provided");
                case Looking.Up:
                    Rigidbody2D.velocity = new Vector2(Rigidbody2D.velocity.x, Rigidbody2D.velocity.y + DashSpeed);
                    PlayerAnimator.Play("Human_Dash_Up");
                    Shaker.CombatShaker("Up");
                    DashCoolDown = Time.time;
                    break;

                case Looking.Down:
                    Rigidbody2D.velocity = new Vector2(Rigidbody2D.velocity.x, Rigidbody2D.velocity.y - DashSpeed);
                    PlayerAnimator.Play("Human_Dash_Down");
                    Shaker.CombatShaker("Down");
                    DashCoolDown = Time.time;
                    break;

                case Looking.Left:
                    Rigidbody2D.velocity = new Vector2(Rigidbody2D.velocity.x - DashSpeed, Rigidbody2D.velocity.y);
                    PlayerAnimator.Play("Human_Dash_Left");
                    Shaker.CombatShaker("Left");
                    DashCoolDown = Time.time;
                    break;

                case Looking.Right:
                    Rigidbody2D.velocity = new Vector2(Rigidbody2D.velocity.x + DashSpeed, Rigidbody2D.velocity.y);
                    PlayerAnimator.Play("Human_Dash_Right");
                    Shaker.CombatShaker("Right");
                    DashCoolDown = Time.time;
                    break;
            }
        }
    }

    private IEnumerator ToggleIsDashingBool()
    {
        IsDashing = true;
        yield return new WaitForSeconds(0.1f);
        IsDashing = false;
    }

    public bool isPlayerDashing()
    {
        return IsDashing;
    }

    public void SwordAttack()
    {
        switch (playerIsLooking)
        {
            default: throw new System.Exception("PlayerMovement.Looking state not valid");
            case PlayerMovement.Looking.Left:
                break;

            case PlayerMovement.Looking.Right:
                break;

            case PlayerMovement.Looking.Up:
                break;

            case PlayerMovement.Looking.Down:
                break;
        }

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(Sword.transform.position, SwordRange, LayerMask.GetMask("enemies"));

        foreach (Collider2D enemy in hitEnemies)
        {
            bool isCrit = false;

            if (enemy.GetComponent<Health>())
            {
                int dammageToApply = SwordDamage + GameManager.meleeAttackBonus;
                var random = Random.Range(0, 11);

                // Check if critical hit
                if (random == 10)
                {
                    dammageToApply *= CritModifier;
                    isCrit = true;
                }

                enemy.GetComponent<Health>().TakeDamage(dammageToApply, transform.gameObject, Helper.DamageTypes.PlayerSword, isCrit);

                string[] swordHits = new string[] { "SwordHit", "SwordHit1", "SwordHit2", "SwordHit3", "SwordHit4", "SwordHit5", "SwordHit6" };
                int rand = Random.Range(0, swordHits.Length);

                AudioManager.PlayAudioClip(swordHits[rand]);

            }
            else
            {
                AudioManager.PlayAudioClip("SwordMiss");
            }
        }
    }

    public void BowAttack(Vector3 mouseClickPosition)
    {
        Bow.GetComponent<ShortBow>().ShootBow(mouseClickPosition);
    }

    public void SetBloodiedSprites(bool x)
    {
        if (x)
        {
            walkLeft = walkLeftBloodied;
            walkRight = walkRightBloodied;
            walkUp = walkUpBloodied;
            walkDown = walkDownBloodied;
        }
        else
        {
            walkLeft = walkLeftNonBloodied;
            walkRight = walkRightNonBloodied;
            walkUp = walkUpNonBloodied;
            walkDown = walkDownNonBloodied;
        }
    }

    public void SetMeleeAsActiveWeapon()
    {
        //Deactivate bow if it currently is held by the player
        if (BowEquipped)
        {
            BowController.SetActive(false);
            Bow.SetActive(false);
            BowEquipped = false;
            GameManager.rangedWeaponEquipped = false;
        }

        SwordEquipped = true;
        SwordController.SetActive(true);
        Sword.SetActive(true);
        Player.GetComponent<PlayerCombat>().SetEquippedWeaponName("Short Sword");
    }

    public void SetRangedAsActiveWeapon()
    {
        if (SwordEquipped)
        {
            SwordEquipped = false;
            SwordController.SetActive(false);
            Sword.SetActive(false);
        }

        BowEquipped = true;
        GameManager.rangedWeaponEquipped = true;
        BowController.SetActive(true);
        Bow.SetActive(true);
        Player.GetComponent<PlayerCombat>().SetEquippedWeaponName("Short Bow");

    }

    // This update function if really chonky, probably needs a look over
    void Update()
    {
        if (!Player) Player = Helper.Player;

        playerIsLooking = GetComponent<PlayerMovement>().PlayerIsLooking();
        if (BowEquipped)
        {
            BowController.transform.localScale = new Vector2(-transform.localScale.x + 1.5f, transform.localScale.y - 1.5f);
        }

        switch (playerIsLooking)
        {
            default: return;
            case PlayerMovement.Looking.Left:
                if (SwordEquipped != null && SwordEquipped == true) SwordController.transform.localPosition = new Vector3(0.054f, 0.057f, 180f);
                if (SwordEquipped != null && SwordEquipped == true) Sword.GetComponent<SpriteRenderer>().sortingOrder = 3;
                if (BowEquipped == true)
                {
                    Bow.GetComponent<SpriteRenderer>().sortingOrder = 3;
                }
                break;

            case PlayerMovement.Looking.Right:
                if (SwordEquipped != null && SwordEquipped == true) SwordController.transform.localPosition = new Vector3(-0.05f, 0.043f, 0);
                if (SwordEquipped != null && SwordEquipped == true) Sword.GetComponent<SpriteRenderer>().sortingOrder = 3;

                if (BowEquipped == true)
                {
                    Bow.GetComponent<SpriteRenderer>().sortingOrder = 3;
                }

                break;

            case PlayerMovement.Looking.Up:
                if (SwordEquipped != null && SwordEquipped == true) SwordController.transform.localPosition = new Vector3(0.052f, 0.055f, 0);
                if (SwordEquipped != null && SwordEquipped == true) Sword.GetComponent<SpriteRenderer>().sortingOrder = 1;
                if (BowEquipped == true)
                {
                    Bow.GetComponent<SpriteRenderer>().sortingOrder = 1;
                }
                break;

            case PlayerMovement.Looking.Down:
                if (SwordEquipped != null && SwordEquipped == true) SwordController.transform.localPosition = new Vector3(-0.0287f, 0.0485f, 0);
                if (SwordEquipped != null && SwordEquipped == true) Sword.GetComponent<SpriteRenderer>().sortingOrder = 3;

                if (BowEquipped == true)
                {
                    Bow.GetComponent<SpriteRenderer>().sortingOrder = 3;
                }
                break;
        }

        if (Time.time > DashCoolDown + DashDelay)
        {
            CanDash = true;
            DashCoolDown = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Dash();
        }

        ///WEAPON SELECTION SHOULD PROBABLY BE MOVED TO PlayerCombat.CS

        // If the "1" button is pressed equip the sword

        if (Input.GetKeyDown(KeyCode.Alpha1) && SwordEquipped == false)
        {
            SetMeleeAsActiveWeapon();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && BowEquipped == false && HasPickedUpRangedWeapon)
        {
            SetRangedAsActiveWeapon();
        }

        Vector3 mousePOS = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePOS.z = 0f;

        Vector3 aimDirection = (mousePOS - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

        if (SwordEquipped)
        {
            SwordController.transform.eulerAngles = new Vector3(0, 0, angle);
        }

        if (BowEquipped)
        {
            BowController.transform.eulerAngles = new Vector3(0, 0, angle);
        }

    }
}