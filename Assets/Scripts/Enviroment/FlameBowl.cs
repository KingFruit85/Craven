using UnityEngine;

public class FlameBowl : MonoBehaviour
{
    public Animator anim;
    public SpriteRenderer sr;
    public bool startLit;
    public Sprite unlit;
    void Awake()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        if (!startLit)
        {
            UnLight();
        }
    }

    public void Light()
    {
        anim.Play("lit");
    }

    public void UnLight()
    {
        anim.StopPlayback();
        startLit = false;
        sr.sprite = unlit;
    }

    void Update()
    {
        if (startLit)
        {
            Light();
        }
    }
}
