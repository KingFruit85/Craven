using System.Collections;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Helper Helper;
    private SpriteRenderer SpriteRenderer;
    [SerializeField]
    private string CurrentSprite;
    public Animator an;
    public string EquippedWeaponName;
    public bool RangedWeaponEquipped = false;
    private float RangedCooldown = -9999;
    private float ArrowSpeed = 1;
    private int ArrowDamage = 10;
    private float RangedAttackDelay = 0.5f;
    private PlayerMovement.Looking PlayerIsLooking;
    private PlayAnimations pa;
    public GameManager GameManager;
    public Vector3 mouseClickPosition;



    void Start()
    {
        Helper = GameObject.FindGameObjectWithTag("Helper").GetComponent<Helper>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        an = GetComponent<Animator>();
        pa = GetComponent<PlayAnimations>();
        GameManager = Helper.GameManager;
        EquippedWeaponName = "Short Sword"; // this sucks, switch to enum
    }

    public void SetEquippedWeaponName(string name)
    {
        EquippedWeaponName = name;
    }

    public void SetRangedWeaponEquipped(bool x)
    {
        RangedWeaponEquipped = x;
        GameObject.Find("GameManager").GetComponent<GameManager>().rangedWeaponEquipped = true;
    }

    private void SetCurrentSprite()
    {
        // Gets the current player sprite with the junk text trimmed off
        CurrentSprite = SpriteRenderer.sprite.ToString();
        CurrentSprite = CurrentSprite.Substring(0, CurrentSprite.LastIndexOf(" ") - 1).Trim();
    }

    public string GetCurrentSprite()
    {
        return CurrentSprite;
    }

    void Attack()
    {
        if (GameManager.currentHost == HostType.Human)
        {
            if (GameManager.rangedWeaponEquipped) gameObject.transform.GetComponent<Human>().BowAttack(mouseClickPosition);
            if (!GameManager.rangedWeaponEquipped) GetComponent<Human>().SwordAttack();
        }

        if (GameManager.currentHost == HostType.Ghost) GetComponent<Ghost>().FireGhostBolt();
        if (GameManager.currentHost == HostType.Worm) GetComponent<Worm>().PoisonBite();
    }

    public void SetRangedAttack(float speed, int damage, float attackDelay)
    {
        ArrowSpeed = speed;
        ArrowDamage = damage;
        RangedAttackDelay = attackDelay;
    }

    public void Update()
    {
        PlayerIsLooking = Helper.Player.GetComponent<PlayerMovement>().PlayerIsLooking();

        if (RangedWeaponEquipped == true && GameManager.GetArrowCount() > 0)
        {
            if (Time.time > RangedCooldown + RangedAttackDelay)
            {
                RangedCooldown = Time.time;
            }
        }
        SetCurrentSprite();
        GetCurrentSprite();

        mouseClickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0)) Attack();
    }


}
