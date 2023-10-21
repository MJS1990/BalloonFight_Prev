using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class Monster_Common : Monster
{
    int id { get; }

    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer sprite;

    void Start()
    {
        //Components
        sprite = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        
        //State
        state = GetComponent<MonsterState>();
        
        //Status
        status = GetComponent<MonsterStatus>();
        maxhp = status.GetHp();
        currenthp = maxhp;
        //status.GetStatus(id);
        
        //Pathfinding
        chasePath = new List<Vector2Int>();
        targetPos = new Vector3();
        pathPointPos = new Vector3();
        //Patrol
        patrolPath = GetComponentInChildren<LineRenderer>();
        //patrolPath = GetComponent<LineRenderer>();
        
        knockBackDir = new Vector3();
        kTime = 0.0f;
        knockBackOrigin = knockBackPower;
    }

    private void Update()
    {
        //if (bDamaged)
        //{
        //    bDamaged = true;
        //
        //    kTime += Time.deltaTime;
        //    if (kTime >= knockbackTime)
        //    {
        //        bDamaged = false;
        //        kTime = 0.0f;
        //    }
        //
        //    transform.position += new Vector3(knockBackDir.x * knockBackPower * Time.deltaTime, knockBackDir.y * knockBackPower * Time.deltaTime, transform.position.z);
        //}
        if (moveSpeed <= 0)
            moveSpeed = UnityEngine.Random.Range(minSpeed, maxSpeed);
    }
    
    void FixedUpdate()
    {
        if (state.IsIdle())
        {
            Patrol();
        }
        else if (state.IsMove())
        {
            MoveTo();
        }
        else if (state.IsHitted())
        {
            Hitted();
        }
        else if (state.IsGroggy() && deadTime == 0)
        {
            Groggy();
        }
        else if (state.IsDead())
        {
            Dead();
        }

        if (state.IsDead()) deadTime += Time.deltaTime;

        //Player탐지////////////////////////////////////////////////////////////////
        Vector3 vec = transform.position - player.gameObject.transform.position;
        float dis = Mathf.Sqrt(Mathf.Abs(vec.x) + Mathf.Abs(vec.y));

        if(dis < 4.5f && moveOffsetTime >= 2.0f && !state.IsGroggy())
        {
            targetPos = player.transform.position;
            moveOffsetTime = 0.0f;
            state.SetMove();
        }
        //else
        //{
        //    state.SetIdle();
        //}
        moveOffsetTime += Time.deltaTime;
        ////////////////////////////////////////////////////////////////////////////

        if (bDamaged)
        {
            bDamaged = true;
            NoneDamaged(knockbackTime);
            gameObject.layer = 19; 
            kTime += Time.deltaTime;
            if (kTime >= knockbackTime)
            {
                bDamaged = false;
                gameObject.layer = 7;
                kTime = 0.0f;
                knockBackPower = knockBackOrigin;
            }

            //transform.position += new Vector3(knockBackDir.x * knockBackPower * Time.deltaTime, knockBackDir.y * knockBackPower * Time.deltaTime, transform.position.z);
            rigid.AddForce(new Vector2(knockBackDir.x * knockBackPower, knockBackDir.y * knockBackPower), ForceMode2D.Force);
            if(knockBackPower >= 0)
                knockBackPower *= 0.7f;
        }

        if (GetStatus().GetHp() <= 0) state.SetGroggy();
    }

    public Vector3 GetTargetPos() { return player.transform.position; }
    
    public override void GetDamage(int damage, Vector3 dir)
    {
        knockBackDir = dir;
        bDamaged = true;
        state.SetHitted();
        status.SetDamage(damage);
    }

    public override bool Hitted()
    {
        if (status.GetHp() <= 0 && !state.IsGroggy())
        {
            state.SetGroggy();
            return false;
        }
        else
            End_Hitted();

        return true;
    }

    public override void End_Hitted()
    {
        state.SetIdle();
    }

    public override bool NoneDamaged(float time)
    {
        if(!state.IsGroggy())
        {
            gameObject.layer = 6;
        }


        noneDamageTime += Time.deltaTime;

        if (sprite.color.a == 1.0f)
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.3f);
        else
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1.0f);

        if (time != 0 && (noneDamageTime >= time || !state.IsGroggy()))
        {

            if (sprite.color.a != 1.0f)
                sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1.0f);
            gameObject.layer = 7;
            noneDamageTime = 0.0f;

            return true;
        }

        return false;
    }

    public override bool Groggy()
    {
        gameObject.layer = 18;
        rigid.mass = 1.0f;
        rigid.gravityScale = 1.0f;
        NoneDamaged(0);

        return true;
    }

    public override bool Dead()
    {
        gameObject.layer = 19;

        Attachment at = player.gameObject.GetComponentInChildren<Attachment>();
        if (at.transform.position.x > transform.position.x)
            knockBackDir = new Vector2(-1, 1);
        else
            knockBackDir = new Vector2(1, 1);

        //애니메이션 재생
        End_Dead();

        return true;
    }

    public override void End_Dead()
    {
        NoneDamaged(2.5f);

        rigid.AddForce(new Vector2(knockBackDir.x * knockBackPower * 2, knockBackDir.y * knockBackPower * 2), ForceMode2D.Force);

        Destroy(this.gameObject, 1.5f); //2번째 파라미터 : 함수 콜 지연시간

        PlayerStatus playerStatus = player.gameObject.GetComponent<PlayerStatus>();
        playerStatus.SetExp(status.GetExp());
        status.exp = 0;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Attachment" || col.gameObject.tag == "PlayerAttack") //Attachment
        {
            bDamaged = true;
            //state.SetHitted();
            CombatManager.Get().bPlayerAttackHit = true;

            //Vector3 dir = transform.position - col.gameObject.transform.position;
            //dir.Normalize();
            ////Vector2 vel = collision.gameObject.GetComponent<Rigidbody2D>().velocity;
            //rigid.AddForce(new Vector2(dir.x, dir.y) * attachmentImpact, ForceMode2D.Impulse);
        }

        if (col.gameObject.layer == 13) //Bullet
        {
            //state.SetHitted();

            //bDamaged = true;
            //CombatManager.Get().bPlayerAttackHit = true;
            //
            //Vector3 dir = transform.position - col.gameObject.transform.position;
            //dir.Normalize();
            ////Vector2 vel = collision.gameObject.GetComponent<Rigidbody2D>().velocity;
            //rigid.AddForce(new Vector2(dir.x, dir.y) * 0.5f, ForceMode2D.Force);
        }
    }



    //private void OnTriggerEnter2D(Collider2D col)
    //{
    //    if (col.gameObject.tag == "Player")
    //    {
    //        targetPos = col.transform.position;
    //        state.SetMove();
    //    }
    //}

    //private void OnTriggerStay2D(Collider2D col)
    //{
    //    if (!state.IsMove() && col.gameObject.tag == "Player" && moveOffsetTime >= 2.0f && !state.IsGroggy())
    //    {
    //        moveOffsetTime = 0.0f;
    //        targetPos = col.transform.position;
    //        //state.SetMove();
    //    }
    //}
    //
    //private void OnTriggerExit2D(Collider2D col)
    //{
    //    if (state.IsMove() && col.gameObject.tag == "Player")
    //    {
    //        moveOffsetTime = 0.0f;
    //        state.SetIdle();
    //    }
    //}
}
