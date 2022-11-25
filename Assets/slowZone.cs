using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class slowZone : MonoBehaviour
{
    public SpriteRenderer sr;
    public CircleCollider2D myCollider;
    public Animator anim;
    public Collider2D[] objectsInSlowZone;

    void Awake()
    {
        sr = gameObject.transform.parent.GetComponent<SpriteRenderer>();
        anim = transform.parent.GetComponent<Animator>();
    }
    void Start()
    {
        myCollider = GetComponent<CircleCollider2D>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        var projectile = other.GetComponents<IProjectile>();
        if (projectile.Length > 0)
        {
            sr.material.SetColor("_GlowColour", new Color(1f, 0f, 0f, 1f));
            projectile[0].speed = projectile[0].speed / 6;
        };
    }
    void OnTriggerExit2D(Collider2D other)
    {
        var projectile = other.GetComponents<IProjectile>();
        if (projectile.Length > 0)
        {
            projectile[0].speed = projectile[0].speed / 6;
        };
    }

    void Flash()
    {
        anim.Play("Pentagram");
    }

    void StopFlash()
    {
        anim.Play("PentagramNOFLASH");
    }

    void Update()
    {
        objectsInSlowZone = Physics2D.OverlapCircleAll(transform.position, myCollider.radius).Where(o => o.gameObject.layer == 9).ToArray();

        if (objectsInSlowZone.Length > 0)
        {
            Flash();
        }

        if (objectsInSlowZone.Length == 0)
        {
            StopFlash();
        }
    }
}
