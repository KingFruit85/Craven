using UnityEngine;

public class FlameBowl : MonoBehaviour
{
    public Animator anim;
    public bool startLit;
    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        if (!startLit)
        {
            Extinguish();
        }

        if (startLit)
        {
            Light();
        }
    }

    public void Light()
    {
        anim.SetBool("isLit", true);
    }

    public void Extinguish()
    {
        startLit = false;
        anim.SetBool("isLit", false);
    }

}
