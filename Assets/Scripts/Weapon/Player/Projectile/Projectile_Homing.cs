using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
//using static UnityEngine.GraphicsBuffer;
//using static UnityEngine.RuleTile.TilingRuleOutput;

public class Projectile_Homing : Projectile
{
    GameObject targetObj;

    bool bGetTarget = false;
    [SerializeField]
    float homingSpeed = 2.0f;
    [SerializeField]
    float homingRotSpeed = 2.0f;
    [SerializeField]
    float searchInterval = 2.0f;

    float searchTime = 0.0f;

    Quaternion rotTarget;
    

    void Awake()
    {
        fireType = EProjectileType.Homing;

        rigid = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (anim)
            anim = GetComponent<Animator>();

        trail = GetComponent<TrailRenderer>();
        impactEffect = GetComponentInChildren<Effect>();

        targetObj = null;
        dir = transform.right;
        bImpact = false;
    }

    private void Update()
    {
        if (bGetTarget)
            Fire();
        else
            rigid.AddForce(dir * speed, ForceMode2D.Force);
    }

    void FixedUpdate()
    {
        if (collectTime >= 5.0f || (transform.position.x > 200.0f || transform.position.y > 200.0f))
            Destroy(gameObject);

        collectTime += Time.deltaTime;

        if (bGetTarget)
            searchTime += Time.deltaTime;
    }

    public new void ResetProjectile()
    {
        targetObj = null;
        spriteRenderer.enabled = true;
        trail.enabled = true;
        transform.rotation = Quaternion.identity;
        bImpact = false;
        bGetTarget = false;
        collectTime = 0.0f;
    }

    public override void Fire()
    {
        if (bImpact || targetObj == null) return;

        if (transform.parent != null)
        {
            if (transform.parent.gameObject.GetComponent<SpriteRenderer>().flipX)
                spriteRenderer.flipX = true;
            else
                spriteRenderer.flipX = false;
        }

        dir = (targetObj.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        rotTarget = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotTarget, Time.deltaTime * homingRotSpeed);
        rigid.velocity = new Vector2(dir.x * homingSpeed, dir.y * homingSpeed);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (impactEffect != null)
        {
            impactEffect.Play(transform.position);
            Vector3 dir = (transform.position - col.transform.position).normalized;
            dir.x -= 0.5f;
            dir.x *= -1;
            impactEffect.PlayParticle(dir);
        }

        bImpact = true;
        spriteRenderer.enabled = false;
        trail.enabled = false;
        if (col.gameObject.layer == 7)
        {
            //적hp감소, 죽음판정, 플레이어 경험치 증가
            Monster monster = col.gameObject.GetComponent<Monster>();
            monster.GetDamage(damage, new Vector3(0.0f, 0.0f, 0.0f));
        }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 7 && targetObj == null)
        {
            bGetTarget = true;
            targetObj = col.gameObject;
        }
    }
    //private void OnTriggerStay2D(Collider2D col)
    //{
    //    if (col.gameObject.layer == 7 && searchTime > searchInterval)
    //    {
    //        searchTime = 0.0f;
    //        targetObj = col.gameObject;
    //    }
    //}
}
