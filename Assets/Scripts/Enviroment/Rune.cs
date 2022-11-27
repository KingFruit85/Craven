using UnityEngine;

public class Rune : MonoBehaviour
{
    public Sprite deactivatedSprite, activatedSprite;
    public FlameBowl flameBowl;
    private SpriteRenderer SR;
    public string MyCode;
    public GameObject myTrap;
    public PuzzleRooms myRoom;

    void Awake()
    {
        SR = GetComponent<SpriteRenderer>();
        SR.sprite = deactivatedSprite;
    }

    void Update()
    {
        if (!myRoom)
        {
            myRoom = transform.parent.parent.GetComponent<PuzzleRooms>();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            SR.sprite = activatedSprite;
            myRoom.SubmitCode(MyCode);
            if (flameBowl) flameBowl.Light();
        }
    }

    public void ResetRune()
    {
        SR.sprite = deactivatedSprite;
    }
}
