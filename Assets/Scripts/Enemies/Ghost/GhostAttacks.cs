using System.Collections;
using UnityEngine;

public class GhostAttacks : MonoBehaviour
{

    public Helper Helper;
    private AudioManager AudioManager;
    private SpriteRenderer SpriteRenderer;
    public GameObject GhostBolt;
    private GameObject Player;

    [SerializeField]
    private float AttackDelay = 2.0f;
    private float LastAttacked = -9999;
    private bool CanAttackIsThisNeeded = true;

    private float attackCooldown = 2.5f; // The time between attacks
    private float lastAttackTime = 0f;

    public void Start()
    {
        Helper = GameObject.FindGameObjectWithTag("Helper").GetComponent<Helper>();
        Player = Helper.Player;
        AudioManager = Helper.AudioManager;
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private bool CanAttack()
    {
        return Time.time - lastAttackTime >= attackCooldown;
    }

    public void FireGhostBolt()
    {
        if (CanAttack())
        {
            lastAttackTime = Time.time;

            string[] ghostBolts = new string[]{"GhostBolt1","GhostBolt2","GhostBolt3","GhostBolt4",
                                                "GhostBolt5","GhostBolt6","GhostBolt7"};

            AudioManager.PlayAudioClip(ghostBolts[Random.Range(0, ghostBolts.Length)]);

            // Visual warning for the player that attack is incoming

            StartCoroutine(FlashColor(Color.red));
            _ = Instantiate
                                    (
                                        GhostBolt,
                                        transform.position,
                                        transform.rotation,
                                        transform
                                    );
        }
    }

    private IEnumerator FlashColor(Color color)
    {
        SpriteRenderer.color = color;
        yield return new WaitForSeconds(0.4f);
        SpriteRenderer.color = Color.white;
    }

    public void ResetAttackDelay()
    {
        LastAttacked = Time.time;
    }

    void Update()
    {
        // Check for player in agro range
        if (Vector3.Distance(transform.position, Player.transform.position) <= 2.5f)
        {
            if (Time.time > LastAttacked + AttackDelay && CanAttackIsThisNeeded)
            {
                FireGhostBolt();
                LastAttacked = Time.time;
            }
        }
    }

    public void SetCanAttack(bool newCanAttack)
    {
        CanAttackIsThisNeeded = newCanAttack;
    }
}
