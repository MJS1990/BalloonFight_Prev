using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Search;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Projectile_Enemy_Direct : Projectile
{
    private void Awake()
    {
        fireType = EProjectileType.Direct;

        rigid = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (anim)
            anim = GetComponent<Animator>();

        trail = GetComponent<TrailRenderer>();
        impactEffect = GetComponentInChildren<Effect>();

        dir = transform.right;
        bImpact = false;
    }
    
    private void FixedUpdate()
    {
        collectTime += Time.deltaTime;
    }

    [System.Obsolete]
    public new void ResetProjectile()
    {
        collectTime = 0.0f;
        transform.rotation = Quaternion.identity;

        bFire = false;
        bImpact = false;

        if(!spriteRenderer.enabled)
            spriteRenderer.enabled = true;
        if(!gameObject.active)
            gameObject.SetActive(true);
    }

    [System.Obsolete]
    public override void Fire()
    {
        if (bImpact) return;
        bFire = true;

        float angle = Mathf.Atan2(-dir.y, -dir.x) * Mathf.Rad2Deg;        
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        //transform.rotation = Quaternion.Euler(0, 0, angle);

        rigid.AddForce(dir * speed, ForceMode2D.Force);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (impactEffect != null)
            impactEffect.Play(transform.position);
        
        bImpact = true;
        spriteRenderer.enabled = false;
        trail.enabled = false;
        //gameObject.SetActive(false);

        if (col.gameObject.tag == "Player")
        {
            //적hp감소, 죽음판정, 플레이어 경험치 증가
            PlayerAction player = col.gameObject.GetComponent<PlayerAction>();

            player.OnDamaged(transform.position, damage);
        }
    }
}
