using UnityEngine;

public class TrapArrow : MonoBehaviour
{
    public Helper Helper;
    private Rigidbody2D RigidBody2D;

    private Vector3 LastVelocity;
    public float Speed = 0.0005f;
    private Vector3 Left = new Vector3(-1, 0, 0);
    private Vector3 Right = new Vector3(1, 0, 0);
    private Vector3 Up = new Vector3(0, 1, 0);
    private Vector3 Down = new Vector3(0, -1, 0);
    public string Direction;
    public int Damage = 10;
    public bool Deflected = false;
    public bool IgnoreWalls = false;


    void Awake()
    {
        Helper = GameObject.FindGameObjectWithTag("Helper").GetComponent<Helper>();
    }
    void Start()
    {
        RigidBody2D = GetComponent<Rigidbody2D>();

        // Check what object is firing me and fire in the appropriate direction
        if (gameObject.transform.parent.CompareTag("ArrowTrap"))
        {
            Direction = GetComponentInParent<ArrowTrap>().GetDirection();

            if (Direction == "up") transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
            if (Direction == "down") transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
            if (Direction == "left") transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
            if (Direction == "right") transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }

        if (gameObject.transform.parent.CompareTag("ArrowTurret"))
        {
            if (Direction == "up")
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                Up = new Vector3(0, 0, 0);
            }
        }
        Fire();
    }

    public void Fire()
    {
        if (Direction == "left")
        {
            RigidBody2D.AddForce(Left * Speed, ForceMode2D.Force);
        }
        if (Direction == "right")
        {
            RigidBody2D.AddForce(Right * Speed, ForceMode2D.Force);
        }
        if (Direction == "up")
        {
            RigidBody2D.AddForce(Up * Speed, ForceMode2D.Force);
        }
        if (Direction == "down")
        {
            RigidBody2D.AddForce(Down * Speed, ForceMode2D.Force);
        }
    }

    void Update()
    {
        // Tracked to calculate speed for deflections
        LastVelocity = RigidBody2D.velocity;

        // Removes any deflected arrows that are laying about doing nothing
        if (Deflected && RigidBody2D.velocity.x > -5f && RigidBody2D.velocity.y > -5f) Destroy(gameObject, .5f);

        if (Helper.Player.TryGetComponent(out Ghost ghost))
        {
            if (ghost.Phasing() == true)
            {
                Physics2D.IgnoreCollision(Helper.Player.GetComponent<CapsuleCollider2D>(), gameObject.GetComponent<BoxCollider2D>());
            }
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {

        if (coll.collider.gameObject.CompareTag("Player"))
        {
            if (!Deflected)
            {
                coll.gameObject.GetComponent<Health>().TakeDamage(Damage, transform.parent.gameObject, Helper.DamageTypes.TrapArrow, false);
                Destroy(gameObject);
            }
        }

        else if (coll.collider.gameObject.layer == LayerMask.NameToLayer("Items"))
        {
            Deflected = true;
            return;
        }

        else if (coll.collider.gameObject.layer == LayerMask.NameToLayer("enemies"))
        {
            Deflected = true;
            return;
        }

        else if (coll.collider.gameObject.CompareTag("PlayerSword"))
        {

            string[] deflects = new string[]{"SwordArrowDeflect1","SwordArrowDeflect2","SwordArrowDeflect3","SwordArrowDeflect4",
                                             "SwordArrowDeflect5","SwordArrowDeflect6","SwordArrowDeflect7","SwordArrowDeflect8"};

            int rand = Random.Range(0, deflects.Length);

            Helper.AudioManager.PlayAudioClip(deflects[rand]);

            // Deflect the arrow away from the sword
            float speed = LastVelocity.magnitude;
            Vector3 direction = Vector3.Reflect(LastVelocity.normalized, coll.contacts[0].normal);
            RigidBody2D.velocity = direction * speed / 2;

            // tag the arrow as having been deflected
            Deflected = true;
        }

        else if (coll.collider.gameObject.CompareTag("PlayerArrow"))
        {
            var speed = LastVelocity.magnitude;
            var direction = Vector3.Reflect(LastVelocity.normalized, coll.contacts[0].normal);
            RigidBody2D.velocity = direction * speed * 2;
        }

        else if (coll.gameObject.CompareTag("Wall") && !IgnoreWalls)
        {
            Destroy(gameObject);
        }

        else if (coll.gameObject.CompareTag("Wall") && IgnoreWalls)
        {
            return;
        }

    }
}