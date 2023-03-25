using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Enemy_Common : Monster
{
    SpriteRenderer spriteRenderer;
    Rigidbody2D Rigidbody;

    Status status;

    public PlayerStatus player;

    //Test
    LineRenderer patrolPath;
    int pathIndex = 0;
    public float attachmentImpact = 2.0f;
    
    Animator anim;
    SpriteRenderer sprite;

    public float MoveSpeed;

    void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();

        status = GetComponent<Status>();

        patrolPath = this.GetComponentInChildren<LineRenderer>();

        //Rigidbody.gravityScale = status.DT_Status.Gravity;
        /*
        public int Id;
        public EMonsterType MonsterType;
        public float Gravity;
        public float Velocity;
        public float Acceleration;
        public float AccelerationMax;
        public int HpCount;
        public EWeaponType WeaponType;
         */
         
        sprite = GetComponent<SpriteRenderer>();
    }


    void Update()
    {
    }

    void FixedUpdate()
    {
        
        if (MoveTo(patrolPath.GetPosition(pathIndex)))
        {
            if (pathIndex == patrolPath.positionCount - 1)
                pathIndex = 0;
            else
                pathIndex++;

            MoveTo(patrolPath.GetPosition(pathIndex));
        }
    }
    
    //////////////////////////////////////////////////////////////////////
    
    bool MoveTo(Vector3 target)
    {
        if (isDamaged) return false;

        float moveSpeed = 0.2f; //0.2
        float jumpPower = 2.5f; //0.08

        Vector3 dir = target - transform.position;
        dir.Normalize();

        transform.position += dir * moveSpeed * Time.deltaTime;

        if (target.y > transform.position.y)
            Rigidbody.AddForce(dir * jumpPower, ForceMode2D.Force); //Impulse

        float offset = 0.05f;
        Vector3 min = new Vector3(target.x - offset, target.y - offset, target.z);
        Vector3 max = new Vector3(target.x + offset, target.y + offset, target.z);

        if ((transform.position.x >= min.x && transform.position.y >= min.y) && (transform.position.x <= max.x && transform.position.x <= max.x))
            return true;

        return false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.gameObject.tag == "Terrain")
        //{
        //    //Anim.SetBool("IsJumping", false);
        //    Anim.SetBool("IsFlying", false);
        //    gameObject.layer = 8;
        //}

        if (collision.gameObject.layer == 12 || collision.gameObject.tag == "PlayerAttack") //Attachment
        {
            isDamaged = true;
            Vector3 dir = transform.position - collision.gameObject.transform.position;
            dir.Normalize();
            //Vector2 vel = collision.gameObject.GetComponent<Rigidbody2D>().velocity;
            Rigidbody.AddForce(new Vector2(dir.x, dir.y) * attachmentImpact, ForceMode2D.Impulse);
        }

        //if (collision.gameObject.layer == 9) //MonsterAttack
        //    OnDamaged(collision.gameObject.transform.position, 1);
    }
}
