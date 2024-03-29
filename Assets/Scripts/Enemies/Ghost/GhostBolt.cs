﻿using UnityEngine;

public class GhostBolt : MonoBehaviour
{
    private Rigidbody2D Rigidbody2d;

    public Helper Helper;

    private GameObject Player;
    private Vector3 Aim;
    private Vector3 PlayerMouseClick;

    private float Born;
    private float LifeTime = 1.5f;

    public string Shooter;
    private Vector3 LastVelocity;
    public bool Deflected;
    public float speed = 0.0001f;
    public int damage = 10;

    void Awake()
    {
        Helper = GameObject.FindGameObjectWithTag("Helper").GetComponent<Helper>();
        Player = Helper.Player;
        Rigidbody2d = GetComponent<Rigidbody2D>();
        PlayerMouseClick = Player.GetComponent<PlayerCombat>().mouseClickPosition;
        Born = Time.time;
    }

    void Start()
    {
        Shooter = transform.parent.tag;

        // Stops the bolt colliding with whoever is firing it
        if (Shooter == "Player")
        {
            Physics2D.IgnoreCollision(gameObject.GetComponent<BoxCollider2D>(), Player.GetComponent<CapsuleCollider2D>());
        }
        else
        {
            Physics2D.IgnoreCollision(gameObject.GetComponent<BoxCollider2D>(), transform.parent.GetComponent<CapsuleCollider2D>());
        }
    }

    void Update()
    {
        if (Shooter == "Player") ShootAtEnemy();
        if (Shooter != "Player") ShootAtPlayer();

        if (Time.time >= Born + LifeTime)
        {
            Destroy(this.gameObject);
        }

        // Tracked to calculate speed for deflections
        LastVelocity = Rigidbody2d.velocity;

        if (Deflected)
        {
            damage = 0;
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    private void ShootAtEnemy()
    {
        damage = Helper.GameManager.rangedAttackBonus + damage;
        Aim = (PlayerMouseClick - transform.position).normalized;
        Rigidbody2d.AddForce(Aim * speed, ForceMode2D.Impulse);
    }

    private void ShootAtPlayer()
    {
        Aim = (Player.transform.position - transform.position).normalized;
        Rigidbody2d.AddForce(Aim * speed, ForceMode2D.Impulse);
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        var other = coll.collider.gameObject;

        if (other.name == "Sword")
        {
            // Deflect the bolt away from the sword

            string[] deflects = new string[]{"SwordGhostBoltDeflect1","SwordGhostBoltDeflect2","SwordGhostBoltDeflect3","SwordGhostBoltDeflect4",
                                             "SwordGhostBoltDeflect5","SwordGhostBoltDeflect6","SwordGhostBoltDeflect7"};

            int rand = Random.Range(0, deflects.Length - 1);

            Helper.AudioManager.PlayAudioClip(deflects[rand]);

            float speed = LastVelocity.magnitude;
            Vector3 direction = Vector3.Reflect(LastVelocity.normalized, coll.contacts[0].normal);
            Rigidbody2d.velocity = direction * speed * 4;
            Deflected = true;
        }

        if (other.tag == "Wall")
        {
            //Add animation
            Destroy(this.gameObject);
        }

        // Player logic

        if (Shooter == "Player" && other.layer == 8)
        {
            other.GetComponent<Health>().TakeDamage(damage, transform.parent.gameObject, Helper.DamageTypes.Ranged, false);
        }

        // Ghost logic, should only do damage to player and if shot is deflected do no damage
        if (Shooter == "Ghost" && other.tag == "Player" && !Deflected)
        {
            // If player is human and dashing don't apply damage
            if (Player.GetComponent<Human>() && Player.GetComponent<Human>().isPlayerDashing())
            {
                return;
            }

            // if player not dashing apply damage
            else
            {
                bool isCrit = false;
                if (Random.Range(0, 11) == 10)
                {
                    isCrit = true;
                    damage += (damage * 2);
                }
                Player.GetComponent<Health>().TakeDamage(damage, transform.parent.gameObject, Helper.DamageTypes.Ranged, isCrit);
                //add animation
                Destroy(this.gameObject);
            }

        }
    }

}
