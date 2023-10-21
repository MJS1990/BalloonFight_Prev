using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using UnityEngine;

public class Weapon_Enemy_Shooter : Weapon
{
    private Rigidbody2D rigid;
    private new BoxCollider2D collider;
    SpriteRenderer spriteRenderer;

    Effect muzzleEffect;

    //EWeaponType weaponType;

    public Projectile bullet;
    public Animator muzzleAnim;

    public int poolingCount = 30;
    public int fireCount = 1;
    public float RateOfFire = 1.0f;
    public float weight = 0.0f;

    float currentTime = 0.0f;

    [SerializeField]
    float bulletCollectTime = 5.0f;

    Vector2 muzzlePos;
    //Vector2 attachPos;

    [HideInInspector]
    public Queue<Projectile> bullets;
    List<Projectile> firedBullets;

    [SerializeField]
    GameObject target;
    //bool bSearchTarget;

    int damage;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        //weaponType = EWeaponType.Gun;

        bullets = new Queue<Projectile>();
        firedBullets = new List<Projectile>();

        //attachPos = new Vector2(0, 0);

        muzzlePos = new Vector2(transform.position.x, transform.position.y);
        //muzzleEffect = GetComponent<Effect>();
        //muzzleEffect.SetAnim(muzzleAnim);

        for (int i = 0; i < poolingCount; i++)
        {
            Projectile newBullet = Instantiate(bullet, muzzlePos, Quaternion.identity);
            newBullet.gameObject.SetActive(false);
            bullets.Enqueue(newBullet);
        }

        //bSearchTarget = false;
    }

    void FixedUpdate()
    {
        currentTime += Time.deltaTime;

        gameObject.transform.position = transform.parent.transform.position;
        //transform.localPosition = Vector3.zero;

        if (currentTime >= RateOfFire)
        {
            for (int i = 0; i <= fireCount; i++)
                Fire();

            currentTime = 0.0f;
        }

        //충돌한 투사체 보관
        for (int i = firedBullets.Count - 1; i >= 0; i--)
        {
            if (firedBullets[i].GetCollectTime() >= bulletCollectTime)// || firedBullets[i].impactEffect.bEnd) // || firedBullets[i].bImpact == true 
            {
                if (!firedBullets[i].impactEffect.bEnd) continue;

                Projectile p = firedBullets[i];
                p.gameObject.SetActive(false);
                bullets.Enqueue(p);
                firedBullets.RemoveAt(i);
            }
        }


        if (transform.parent.gameObject.GetComponent<SpriteRenderer>().flipX)
        {
             spriteRenderer.flipX = false;                
             //Vector3 offsetX = transform.parent.transform.position - transform.position;
             //transform.position += offsetX * 2;
        }
        else if (!transform.parent.gameObject.GetComponent<SpriteRenderer>().flipX)
        {
             spriteRenderer.flipX = true;
             //Vector3 offsetX = transform.parent.transform.position - transform.position;
             //transform.position += offsetX * 2;
        }
    }

    [System.Obsolete]
    void Fire()
    {
        if (bullets.Count == 0) return;
        Projectile currentBullet = bullets.Dequeue();

        Projectile_Enemy_Direct proj = currentBullet.GetComponent<Projectile_Enemy_Direct>();
        proj.transform.position = transform.position;

        proj.SetDir(target.transform.position - transform.position);

        if (spriteRenderer.flipX)
        {
            //proj.SetDir(-transform.right);
            proj.gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            //proj.SetDir(transform.right);
            proj.gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }

        currentBullet.gameObject.SetActive(true);

        proj.ResetProjectile();
        proj.Fire();

        firedBullets.Add(proj);
       
        //firedBullets.Add(currentBullet);
    }
}