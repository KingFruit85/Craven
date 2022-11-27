using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isOpen, isLocked;
    public Sprite closedDoor, openDoor;
    private DoorController doorController;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;

    void Awake()
    {
        doorController = transform.parent.parent.GetComponent<DoorController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        OpenDoor();
    }

    public void OpenDoor()
    {
        spriteRenderer.sprite = openDoor;
        boxCollider.enabled = false;
        isOpen = true;
    }

    public void CloseDoor()
    {
        spriteRenderer.sprite = closedDoor;
        boxCollider.enabled = true;
        isOpen = false;
    }
}