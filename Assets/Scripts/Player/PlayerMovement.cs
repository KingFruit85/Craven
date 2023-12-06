using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Vector2 movement;
    public Vector3 mousePOS;
    public Camera cam;
    public float moveSpeed;
    [SerializeField]
    private bool canMove = true;
    private Rigidbody2D rb;
    public SpriteRenderer sr;
    public bool isSlowed = false;
    public GameObject player;
    public Looking looking;
    public bool isMoving;

    public enum Looking
    {
        Up,
        Down,
        Left,
        Right
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            looking = Looking.Left;
            Debug.Log("Looking Left");
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            looking = Looking.Right;
            Debug.Log("Looking Right");
        }
        else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            looking = Looking.Up;
            Debug.Log("Looking Up");
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            looking = Looking.Down;
            Debug.Log("Looking Down");
        }

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (movement.x == 0 && movement.y == 0)
        {
            isMoving = false;
        }
        else
        {
            isMoving = true;
        }

        mousePOS = cam.ScreenToWorldPoint(Input.mousePosition);

        // Visual for slowed effect
        if (isSlowed)
        {
            sr.color = Color.green;
        }
        else if (!isSlowed)
        {
            sr.color = Color.white;
        }

    }

    void FixedUpdate()
    {
        Move();
        // lookDir = mousePOS - player.transform.position;
    }

    public void StopPlayerMovement()
    {
        canMove = false;
    }

    public void StartPlayerMovement()
    {
        canMove = true;
    }

    public Looking PlayerIsLooking()
    {
        return looking;
    }

    void Move()
    {
        if (canMove)
        {
            float moveX = movement.x * moveSpeed;
            float moveY = movement.y * moveSpeed;

            // If in mid dash dont change directon
            if (GetComponent<Human>() && GetComponent<Human>().isPlayerDashing())
            {
                return;
            }
            else
            {
                rb.velocity = new Vector2(moveX, moveY);
            }
        }
    }



    public void DazeForSeconds(int seconds)
    {
        isSlowed = true;
        StartCoroutine(SlowSpeed(seconds));
    }

    private IEnumerator SlowSpeed(int seconds)
    {
        // Half player speed for provided seconds
        moveSpeed = moveSpeed / 8;
        yield return new WaitForSeconds(seconds);

        //Restore to default speed
        moveSpeed = moveSpeed * 8;
        isSlowed = false;
    }

}

