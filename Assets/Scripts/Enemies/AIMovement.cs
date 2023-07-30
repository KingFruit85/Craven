using System.Collections;
using UnityEngine;


public class AIMovement : MonoBehaviour
{
    public enum State
    {
        Roaming,
        ChaseTarget,
        GoingBackToStart,
        Attacking,
    }

    private State CurrentState;
    private Vector2 StartingPosition;
    private Vector2 RoamingPosition;
    private float DistanceApart;
    [SerializeField]
    private float Speed = 1f;
    [SerializeField]
    private float AggroRange = 5f;
    private Vector3 PlayerPOS;
    private Rigidbody2D RB;
    [SerializeField]
    private bool IsSlowed;
    private GameObject CurrentRoom;

    void Awake()
    {
        CurrentState = State.Roaming;
        StartingPosition = transform.position;
        RoamingPosition = GetRoamingPosition();
        RB = GetComponent<Rigidbody2D>();

        switch (transform.tag)
        {
            default:
            case "Ghost": gameObject.AddComponent<Ghost>(); break;
            case "Worm": gameObject.AddComponent<Worm>(); break;
        }
    }

    private Vector3 GetRoamingPosition()
    {
        // Get random direction
        var RD = new Vector2(UnityEngine.Random.Range(-1f, 1f),
                             UnityEngine.Random.Range(-1f, 1f)
                            ).normalized;

        return StartingPosition + RD * Random.Range(1f, 5f);

    }

    void Update()
    {
        var doorController = transform.parent.transform.Find("DoorController").GetComponent<DoorController>();

        if (doorController.playerInRoom)
        {
            Speed = 1.0f;
        }
        else
        {
            Speed = 0.0f;
        }

        PlayerPOS = GameObject.FindGameObjectWithTag("Player").transform.position;
        DistanceApart = Vector2.Distance(transform.position, PlayerPOS);

        switch (CurrentState)
        {
            default: throw new System.Exception("Invalid AI movement state");

            case State.Roaming:
                Patrol();
                FindTarget();
                break;

            case State.ChaseTarget:
                MoveToPlayer();
                break;

            case State.GoingBackToStart:
                ReturnToStartPoint();
                break;

            case State.Attacking:
                Attack();
                break;
        }

        // Visual for slowed effect
        if (IsSlowed)
        {
            GetComponent<SpriteRenderer>().color = Color.green;
        }
        else if (!IsSlowed)
        {
            GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    private void Attack() { }

    private void FindTarget()
    {
        if (DistanceApart < AggroRange)
        {
            CurrentState = State.ChaseTarget;
        }
        else if (DistanceApart > AggroRange)
        {
            CurrentState = State.Roaming;
        }
    }

    private void Patrol()
    {
        transform.position = Vector3.MoveTowards(transform.position, RoamingPosition, Speed * Time.deltaTime);
        float distanceToTarget = Vector3.Distance(transform.position, RoamingPosition);

        if (distanceToTarget <= .1f)
        {
            RoamingPosition = GetRoamingPosition();
        }
    }

    public void ReturnToStartPoint()
    {
        transform.position = Vector3.MoveTowards(transform.position, StartingPosition, Speed * Time.deltaTime);
        float distanceToTarget = Vector3.Distance(transform.position, StartingPosition);

        if (distanceToTarget <= .5f)
        {
            CurrentState = State.Roaming;
        }
    }

    private void MoveToPlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, PlayerPOS, Speed * Time.deltaTime);

        if (DistanceApart > 15f)
        {
            CurrentState = State.GoingBackToStart;
        }
    }

    public void KnockBack(PlayerMovement.Looking direction)
    {
        var knockDirection = "";

        if (direction == PlayerMovement.Looking.Left) knockDirection = "Left";
        if (direction == PlayerMovement.Looking.Right) knockDirection = "Right";
        if (direction == PlayerMovement.Looking.Up) knockDirection = "Up";
        if (direction == PlayerMovement.Looking.Down) knockDirection = "Down";

        StartCoroutine(Knock(knockDirection));
    }

    public IEnumerator Knock(string direction)
    {
        RB.bodyType = RigidbodyType2D.Dynamic;
        RB.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        var thrust = 1.5f;


        // Hit came from left so knock left
        if (direction == "Left")
        {
            RB.AddForce(-transform.right * thrust, ForceMode2D.Impulse);
        }

        // Hit came from right so knock right
        else if (direction == "Right")
        {
            RB.AddForce(transform.right * thrust, ForceMode2D.Impulse);
        }

        // Hit came from up so knock up
        else if (direction == "Up")
        {
            RB.AddForce(transform.up * thrust, ForceMode2D.Impulse);
        }

        // Hit came from down so knock down
        else if (direction == "Down")
        {
            RB.AddForce(-transform.up * thrust, ForceMode2D.Impulse);
        }

        yield return new WaitForSeconds(thrust);
        StartCoroutine(EndKnockBack());
    }

    IEnumerator EndKnockBack()
    {
        RB.velocity = new Vector2(0, 0);
        RB.collisionDetectionMode = CollisionDetectionMode2D.Discrete;
        transform.position = this.transform.position;
        yield return new WaitForSeconds(0.5f);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Wall")
        {
            CurrentState = State.GoingBackToStart;
        }
    }

    public void DazeForSeconds(int seconds)
    {
        IsSlowed = true;
        StartCoroutine(SlowSpeed(seconds));
    }

    public IEnumerator SlowSpeed(int seconds)
    {
        // Half speed for provided seconds
        Speed = Speed / 2;
        yield return new WaitForSeconds(seconds);

        //Restore to default speed
        Speed = Speed * 2;
        IsSlowed = false;
    }

    public void SetSpeed(int newSpeed)
    {
        Speed = newSpeed;
    }
}
