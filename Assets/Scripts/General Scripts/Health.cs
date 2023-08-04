using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    private Helper Helper;
    private SpriteRenderer SpriteRenderer;
    private Shaker CameraShaker;
    private GameManager GameManager;
    private Color NonDefaultColor;
    public GameObject LastHitBy;
    public GameObject Player;

    public float MaxHealth;
    public float CurrentHealth;
    public HostType CurrentHost;
    public bool IsBoss = false;
    public bool IsImmuneToMeleeDamage = false;
    public bool IsImmuneToProjectileDamage = false;
    public bool IsImmuneToAllDamage = false;
    private int CurrentGameLevel;
    private bool IsUsingNonDefaultColor = false;
    private bool IsDead = false;

    void Start()
    {
        Helper = GameObject.FindGameObjectWithTag("Helper").GetComponent<Helper>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        GameManager = Helper.GameManager;
        CurrentGameLevel = GameManager.currentGameLevel;
        Player = Helper.Player;
        CameraShaker = Helper.Camera.GetComponent<Shaker>();

        if (!gameObject.CompareTag(Helper.PlayerTag)) SetScalingEnemyHealth();

        if (gameObject.CompareTag(Helper.PlayerTag))
        {
            ChangeMaxHealth(GameManager.healthBonus);
            CurrentHost = Helper.GameManager.currentHost;
            GameManager.LoadHostScript();
            CurrentHealth = MaxHealth;
        }
    }

    private void SetScalingEnemyHealth()
    {
        if (CurrentGameLevel == 1) CurrentHealth = MaxHealth;
        if (CurrentGameLevel > 1)
        {
            MaxHealth *= (CurrentGameLevel / 1.5f);
            CurrentHealth = MaxHealth;
        }
    }

    public void ChangeMaxHealth(int increaseAmount)
    {
        MaxHealth += increaseAmount;
        AddHealth(increaseAmount);
    }

    public void AddHealth(int amount)
    {
        if (CurrentHealth + amount < MaxHealth) CurrentHealth += amount;
        if (CurrentHealth + amount >= MaxHealth) CurrentHealth = MaxHealth;
    }

    public void RemoveHealth(int amount)
    {
        CurrentHealth -= amount;
    }

    public void SwapHost(GameObject newHost) // This probably needs to be moved out of Health into it's own script
    {
        //Remove the old host script
        switch (CurrentHost)
        {
            default: throw new System.Exception("failed to remove current host script, unknown host");
            case HostType.Human:
                Destroy(gameObject.GetComponent<Human>());
                Destroy(GameObject.Find("SwordAim"));
                Destroy(GameObject.Find("BowAim"));
                break;
            case HostType.Ghost: Destroy(gameObject.GetComponent<Ghost>()); break;
            case HostType.Worm: Destroy(gameObject.GetComponent<Worm>()); break;
        }

        //Add the new host script
        switch (newHost.tag)
        {
            default: throw new System.Exception("Swap to new host failed, attacker host type unknown");
            case "Human":
                GameManager.currentHost = CurrentHost = HostType.Human;
                gameObject.AddComponent<Human>();
                gameObject.GetComponent<Animator>().Play(gameObject.GetComponent<Human>().idleDown);
                gameObject.GetComponent<PlayAnimations>().human = GetComponent<Human>();
                break;

            case "Ghost":
                GameManager.currentHost = CurrentHost = HostType.Ghost;
                gameObject.AddComponent<Ghost>();
                gameObject.GetComponent<Animator>().Play(gameObject.GetComponent<Ghost>().idleDown);
                gameObject.GetComponent<PlayAnimations>().ghost = GetComponent<Ghost>();
                break;

            case "Worm":
                GameManager.currentHost = CurrentHost = HostType.Worm;
                gameObject.AddComponent<Worm>();
                gameObject.GetComponent<Animator>().Play(gameObject.GetComponent<Worm>().idleDown);
                gameObject.GetComponent<PlayAnimations>().worm = GetComponent<Worm>();
                break;
        }

        // Transfer health stats
        MaxHealth = newHost.GetComponent<Health>().MaxHealth;
        CurrentHealth = newHost.GetComponent<Health>().CurrentHealth;

        //Remove attacker from the game and move to their location
        transform.position = newHost.transform.position;
        Destroy(newHost);
    }

    public void SetProjectileImmunity(bool isImmune)
    {
        if (isImmune) IsImmuneToProjectileDamage = true;
        if (!isImmune) StartCoroutine(DisableProjectileImmunity(2f));
    }

    public void SetMeleeImmunity(bool isImmune)
    {
        if (isImmune) IsImmuneToMeleeDamage = true;
        if (!isImmune) StartCoroutine(DisableMeleeImmunity(2f));
    }

    private IEnumerator DisableProjectileImmunity(float duration)
    {
        SpriteRenderer.color = Color.red;
        IsImmuneToProjectileDamage = false;
        yield return new WaitForSeconds(duration);
        IsImmuneToProjectileDamage = true;
        SpriteRenderer.color = Color.white;
    }

    private IEnumerator DisableMeleeImmunity(float duration)
    {
        SpriteRenderer.color = Color.red;
        IsImmuneToMeleeDamage = false;
        yield return new WaitForSeconds(duration);
        IsImmuneToMeleeDamage = true;
        SpriteRenderer.color = Color.white;
    }

    // Takes in a damage value to apply and the game object that caused the damage
    public void TakeDamage(int damage, GameObject attacker, Helper.DamageTypes damageType, bool isCrit)
    {
        LastHitBy = attacker;
        if (TryGetComponent(out Ghost ghost)) IsImmuneToProjectileDamage = ghost.Phasing();
        if (TryGetComponent(out Human human)) IsImmuneToAllDamage = human.isPlayerRolling();

        if (IsImmuneToMeleeDamage && damageType == Helper.DamageTypes.Melee) return;
        if (IsImmuneToAllDamage) return;

        if (gameObject.tag == Helper.PlayerTag && isCrit)
        {
            CameraShaker.Shake(.3f, 3.0f);
            GameManager.SetPlayerHit(isCrit);
        }

        DamagePopup.Create(transform.position, damage, isCrit);
        StartCoroutine(FlashColor(Color.gray, 0.3f));
        RemoveHealth(damage);

        if (gameObject.layer == 8)
        {
            TriggerKnockBack();
            TriggerAttackDelayReset();
        }
    }

    private void TriggerKnockBack()
    {
        if (!IsBoss)
        {
            var playerIsLooking = Player.GetComponent<PlayerMovement>().PlayerIsLooking();
            TryGetComponent(out AIMovement move);
            move.KnockBack(playerIsLooking);
        }
    }

    private void TriggerAttackDelayReset()
    {
        switch (gameObject.tag)
        {
            default: throw new System.Exception("unknown recipient of damage");
            case "Ghost": gameObject.GetComponent<GhostAttacks>().ResetAttackDelay(); break;
            case "MiniBoss": gameObject.GetComponent<GhostAttacks>().ResetAttackDelay(); break;
            case "Worm": gameObject.GetComponent<WormAttacks>().ResetAttackDelay(); break;
        }
    }

    private IEnumerator FlashColor(Color color, float duration)
    {
        SpriteRenderer.color = color;
        yield return new WaitForSeconds(duration);
        SpriteRenderer.color = Color.white;
    }

    public void Die()
    {
        IsDead = true;
        // Player death
        if (gameObject.CompareTag(Helper.PlayerTag))
        {
            // Stops player from being able to move on death and plays death animation 
            GetComponent<PlayerMovement>().moveSpeed = 0;
            StartCoroutine(GetComponent<PlayAnimations>().Death());

            if (TryGetComponent(out Human human)) human.enabled = false;
            // Removes any weapons the host may have
            foreach (Transform child in gameObject.transform)
            {
                if (child.name == "SwordAim")
                {
                    child.GetComponentInChildren<SpriteRenderer>().enabled = false;
                }
            }
            GameManager.EndGame();
        }

        if (IsBoss && gameObject.CompareTag(Helper.EnemyTypes.MiniBoss.ToString()))
        {
            transform.parent.GetComponent<SimpleRoom>().UnlockExitTile();
            GameManager.miniBossKilled = true;
        }

        if (!Player.CompareTag(gameObject.tag))
        {
            if (TryGetComponent(out AIMovement aim))
            {
                aim.SetSpeed(0);
            }

            if (TryGetComponent(out GhostAttacks ga))
            {
                ga.SetCanAttack(false);
            }

            StartCoroutine(GetComponent<PlayAnimations>().Death());

            if (TryGetComponent(out DropLoot dropLoot))
            {
                dropLoot.SpawnLoot();
            }

            if (TryGetComponent(out Collider2D collider))
            {
                collider.enabled = false;
            }
        }
    }

    public void SetSpriteColor(Color color)
    {
        IsUsingNonDefaultColor = true;
        NonDefaultColor = color;
    }

    void Update()
    {
        if (!Player)
        {
            Player = Helper.Player;
        }
        if (CurrentHealth <= 0 && !IsDead)
        {
            if (Player.CompareTag(gameObject.tag))
            {
                int chance = Random.Range(0, 11);
                switch (LastHitBy.tag)
                {
                    default: Die(); break;

                    case "Ghost":
                        if (chance == 10)
                        {
                            SwapHost(LastHitBy);
                            break;
                        }
                        else
                        {
                            Die(); break;
                        }
                    case "Worm":
                        if (chance == 10)
                        {
                            SwapHost(LastHitBy);
                            break;
                        }
                        else
                        {
                            Die(); break;
                        }
                }
            }
            else
            {
                Die();
            }
        }
        if (IsUsingNonDefaultColor)
        {
            GetComponent<SpriteRenderer>().color = NonDefaultColor;
        }
    }
}
