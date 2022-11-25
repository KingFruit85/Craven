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
        anim.SetBool("isLit", true);
    }

    public void UnLight()
    {
        startLit = false;
        anim.SetBool("isLit", false);
    }

    void Update()
    {
        if (startLit)
        {
            Light();
        }
    }
}
