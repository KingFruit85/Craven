using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    private Helper Helper;
    private PlayAnimations PlayAnimation;
    private SpriteRenderer SpriteRenderer;
    private bool IsPhasing = false;
    public float MoveSpeed = 5;
    private bool IsPlayer = false;

    public string idleLeft = "Ghost_Idle_Left";
    public string idleRight = "Ghost_Idle_Right";
    public string walkLeft = "Ghost_Walk_Left";
    public string walkRight = "Ghost_Walk_Right";
    public string walkUp = "Ghost_Walk_Up";
    public string walkDown = "Ghost_Walk_Down";
    public string idleUp = "Ghost_Walk_Up"; // Replace this when posable
    public string idleDown = "Ghost_Idle_Front";
    public string death = "Ghost_Death";

    void Awake()
    {
        Helper = GameObject.FindGameObjectWithTag("Helper").GetComponent<Helper>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        PlayAnimation = GetComponent<PlayAnimations>();

        if (gameObject.CompareTag(Helper.PlayerTag))
        {
            IsPlayer = true;
            GetComponent<PlayerMovement>().moveSpeed = MoveSpeed;
            //Due to the sprite scaling when you change from a human to a ghost the capsule collider is too large to move horizontally though 1 unit tall corridors
            GetComponent<CapsuleCollider2D>().size = new Vector2(0.1f, 0.2f);
            Helper.GameManager.currentHost = HostType.Ghost;
        }

        transform.localScale = new Vector3(3.5f, 3.5f, 0);

        //Set the player animations/sprites to the current host creature
        PlayAnimation.idleLeft = idleLeft;
        PlayAnimation.idleRight = idleRight;
        PlayAnimation.walkLeft = walkLeft;
        PlayAnimation.walkRight = walkRight;
        PlayAnimation.walkUp = walkUp;
        PlayAnimation.walkDown = walkDown;
        PlayAnimation.death = death;

    }

    public bool Phasing()
    {
        return IsPhasing;
    }

    public void FireGhostBolt()
    {
        if (!IsPhasing)
        {
            _ = Instantiate
                (
                    Resources.Load<GameObject>("Ghost_Bolt"),
                    transform.position,
                    transform.rotation,
                    transform
                );
        }
    }

    void Update()
    {
        if (IsPlayer)
        {
            if (Input.GetKeyDown(KeyCode.Space) && IsPhasing == false)
            {
                SpriteRenderer.material.color = new Color(1f, 1f, 1f, 0.5f);
                IsPhasing = true;
                //Can pass through enemies
                Physics2D.IgnoreLayerCollision(12, 8, true);
                //Does not pick up items
                Physics2D.IgnoreLayerCollision(12, 10, true);
            }
            else if (Input.GetKeyDown(KeyCode.Space) && IsPhasing == true)
            {
                SpriteRenderer.material.color = new Color(1f, 1f, 1f, 1f);
                IsPhasing = false;
                Physics2D.IgnoreLayerCollision(12, 8, false);
                Physics2D.IgnoreLayerCollision(12, 10, false);
            }
        }
    }
}
