using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Elite : Monster
{
    int id { get; }

    protected Rigidbody2D rigid;
    protected Animator anim;
    protected SpriteRenderer sprite;

    //MonsterState state;
    //MonsterStatus status;
    //public PlayerStatus player;
    //
    //protected int maxhp;
    //protected int currenthp;
    //public float attachmentImpact;// = 2.0f;
    //public float moveSpeed = 5.0f;
    //public float minSpeed = 5.0f;
    //public float maxSpeed = 8.0f;
    //public float moveOffset = 0.05f;
    //public float moveForce = 0.0f;
    //public float jumpPower = 5.0f;
    //protected float moveOffsetTime = 0.0f;
    //
    ////Patrol
    //protected LineRenderer patrolPath;
    //protected int patrolIndex = 0;
    //
    ////PathFind
    Monster_Pathfinding pathfinder;
    //protected List<Vector2Int> chasePath;
    //protected Vector3 targetPos;
    //protected Vector3 pathPointPos;
    //protected Vector2 pathMoveOffset;
    //protected int pathIndex = 0;
    //protected bool bArrived = false;
    //protected float chaseTime = 0.0f;
    //
    //protected bool bDamaged = false;
    ////int takeDamage = 0;
    //[SerializeField]
    //protected float knockBackPower;
    //protected Vector3 knockBackDir;
    //[SerializeField]
    //protected float knockbackTime;
    //protected float kTime;
    //
    bool bResetPath = false;
    //
    ////Test
    //protected int PatrolCount = 0;
    //protected int ChaseCount = 0;
    //protected int MoveCount = 0;
    //protected int HittedCount = 0;
    //bool bChase = true;
    //
    //public Monster Get() { return this; }
    //public MonsterStatus GetStatus() { return status; }
    //public MonsterState GetState() { return state; }

    void Start()
    {
        print("Call Monster");

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

        //Chase
        pathfinder = GetComponent<Monster_Pathfinding>();
        chasePath = new List<Vector2Int>();

        targetPos = new Vector3();
        pathPointPos = new Vector3();
        pathMoveOffset = new Vector3(); 

        knockBackDir = new Vector3();
        kTime = 0.0f;

        state.SetIdle();
    }
    public Vector3 GetTargetPos() { return player.transform.position; }
    private void Update()
    {
    }

    void FixedUpdate()
    {
        moveOffsetTime += Time.deltaTime;
        chaseTime += Time.deltaTime;

        if (moveSpeed <= 0)
            moveSpeed = UnityEngine.Random.Range(minSpeed, maxSpeed);
        
        if (bDamaged)
        {
            bDamaged = true;

            kTime += Time.deltaTime;
            if (kTime >= knockbackTime)
            {
                bDamaged = false;
                kTime = 0.0f;
            }

            transform.position += new Vector3(knockBackDir.x * knockBackPower * Time.deltaTime, knockBackDir.y * knockBackPower * Time.deltaTime, transform.position.z);
        }
    }

    public override bool Chase()
    {
        if (pathIndex >= chasePath.Count || chasePath.Count <= 0)
        {
            pathIndex = 0;
            chasePath.Clear();
            state.SetIdle();
            return false;
        }

        float x = (float)chasePath[pathIndex].x;// + pathMoveOffset.x;
        float y = (float)chasePath[pathIndex].y;// + pathMoveOffset.y;
        pathPointPos = new Vector3(x, y, 0.0f);

        targetPos = pathPointPos;
        bArrived = MoveTo();

        if (bArrived)
        {
            if (pathIndex == chasePath.Count - 1)
            {
                pathIndex = 0;
                chasePath.Clear();
                state.SetIdle();

                return true;
            }

            pathIndex++;
            pathMoveOffset.x = UnityEngine.Random.Range(-0.5f, 0.5f);
            pathMoveOffset.y = UnityEngine.Random.Range(-0.5f, 0.5f);

            return false;
        }

        return false;
    }

    public override void GetDamage(int damage, Vector3 dir)
    {
        knockBackDir = dir;
        bDamaged = true;
        status.SetDamage(damage);
    }

    public override bool Hitted()
    {
        if (status.GetHp() <= 0)
            state.SetDead();
        else
            End_Hitted();

        return true;
    }

    public override void End_Hitted()
    {
        //state.SetIdle();
    }

    public override bool Dead()
    {
        //애니메이션 재생
        PlayerStatus playerStatus = player.gameObject.GetComponent<PlayerStatus>();
        playerStatus.SetExp(status.GetExp());

        End_Dead();

        return true;
    }

    public override void End_Dead()
    {
        Destroy(this.gameObject); //2번째 파라미터 : 함수 콜 지연시간
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
        //
        //if (collision.gameObject.layer == 9) //MonsterAttack
        //    OnDamaged(collision.gameObject.transform.position, 1);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            chasePath = pathfinder.FindPath(col.transform.position);
            print("EnterCount : " + chasePath.Count);
            for (int i = 0; i < chasePath.Count; i++)
            {
                //chasePath[i] -= new Vector2Int(9, 0);
                //chasePath[i] += new Vector2Int(0, 1);
                print(i + "번째 위치 : " + chasePath[i]);
            }

            state.SetPathfind();
        }
    }

    //private void OnTriggerStay2D(Collider2D col)
    //{
    //    if (col.gameObject.tag == "Player" && chaseTime >= 3.0f)
    //    {
    //        pathfinder.ReFindPath(col.transform.position);
    //        pathIndex = 0;
    //        bResetPath = true;
    //        chaseTime = 0.0f;
    //    }
    //}

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {  
            print("Exit_Path");
            bResetPath = false;
            state.SetIdle();
            chaseTime = 0.0f;
            chasePath.Clear();        
        }
    }
}
