using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody2D rigid;
    BoxCollider2D collider;
    SpriteRenderer spriteRenderer;
    TrailRenderer trail;

    Animator anim;
    
    Effect impactEffect;

    public float speed = 5.0f;
    public float spreadAngle = 0.0f;
    [HideInInspector]
    public bool bImpact;
    [HideInInspector]
    public bool bEndImpact;

    public Vector3 offset;
    
    protected enum eProjectileType
    {
        Nomal = 0,
        Explosion,
        Homing,
    }
    protected eProjectileType type { get; set; }
    
    private void Awake()
    {
        bEndImpact = false;
        bImpact = false;
        rigid = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if(anim)
            anim = GetComponent<Animator>();

        trail = GetComponent<TrailRenderer>();
        impactEffect = GetComponentInChildren<Effect>();

        //offset = new Vector3(1.0f, 0.0f, 0.0f);

        //gameObject.transform.position += offset;
        //impactEffect.gameObject.transform.position += offset;
    }
    
    public void Reset()
    {
        spriteRenderer.enabled = true;
        bEndImpact = false;
        bImpact = false;
    }

    private void Update()
    {
        if (bImpact && impactEffect.bEnd)
        {
            //impactEffect.bEnd = false;
            impactEffect.Reset();
            bEndImpact = true;
            //this.gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
       rigid.AddForce(transform.right * speed, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (impactEffect)
            impactEffect.Play();

        spriteRenderer.enabled = false;
        bImpact = true;
    }
}