using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Rifle : Weapon
{
    private Rigidbody2D rigid;
    private BoxCollider2D collider;

    Effect muzzleEffect;

    EWeaponType weaponType;

    public Projectile bullet;
    public Animator muzzleAnim;

    public int poolingCount = 30;
    public float RateOfFire = 1.0f;
    public float weight = 0.0f;

    float currentTime = 0.0f;

    Vector2 muzzlePos;
    //Vector2 attachPos;

    [HideInInspector]
    public Queue<Projectile> bullets;
    List<Projectile> firedBullets;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();

        weaponType = EWeaponType.Gun;

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
    }

    void FixedUpdate()
    {
        currentTime += Time.deltaTime;

        if (bAttach)
        {
            //gameObject.transform.position = attachPos;
            //transform.localPosition = Vector3.zero;



            if (currentTime >= RateOfFire)
            {
                Fire();
                currentTime = 0.0f;
            }
        }

        //충돌한 투사체 보관
        for (int i = firedBullets.Count - 1; i >= 0; i--)
        {
            if (firedBullets[i].bEndImpact == true)
            {
                Projectile p = firedBullets[i];
                p.gameObject.SetActive(false);
                firedBullets.RemoveAt(i);
                bullets.Enqueue(p);

                //break;
            }
        }
    }

    void Fire()
    {
        if (bullets.Count == 0) return;

        Projectile currentBullet = bullets.Dequeue();
        currentBullet.transform.position = transform.position;
        currentBullet.Reset();
        currentBullet.gameObject.SetActive(true);

        firedBullets.Add(currentBullet);
    }
    
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Attachment")
        {
            bAttach = true;

            if (attachParent)
                transform.parent = attachParent.transform;

            //ContactPoint2D contact = collision.contacts[0];
            //attachPos = contact.point;
    
            //this.collider.isTrigger = true;

            //if (rigid)
            //{
            //    print("SetGravity");
            //    rigid.gravityScale = 0.0f;
            //    rigid.mass = 0.0f;
            //}
        }
    }
}