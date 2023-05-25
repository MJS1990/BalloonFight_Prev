using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    enum EStateType //TODO : getter, setter 만들고 애니메이션 상태값과 매치되도록
    {
        Idle = 0,
        Move,
        Hitted,
        Dead,
        Action,
    };
    EStateType currentState;
    
    protected Rigidbody2D rigid;
    protected Animator anim;
    protected SpriteRenderer sprite;

    protected MonsterStatus status;
    
    public PlayerStatus player;
    public float attachmentImpact = 2.0f;    
    public float moveSpeed = 10.0f;
    public float moveOffset = 0.05f;
    public float jumpPower = 5.0f;
    
    //Patrol
    LineRenderer patrolPath;
    int patrolIndex = 0;

    //PathFind
    Monster_Pathfinding pathfinder;
    List<Vector2Int> chasePath;
    Vector3 pathPointPos;
    public Vector2 pathMoveOffset;
    int pathIndex = 0;
    bool bArrived = false;
    bool bChase = false;

    bool bDamaged = false;
    
    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();

        status = GetComponent<MonsterStatus>();
        //status.GetStatus(id);

        patrolPath = GetComponentInChildren<LineRenderer>();

        chasePath = new List<Vector2Int>();
        pathfinder = GetComponent<Monster_Pathfinding>();
        pathPointPos = new Vector3();
    }
    
    private void FixedUpdate()
    {
        Patrol();

        if (bChase && chasePath.Count > 0)
            Chase();
    }

    private void Patrol()
    {
        if (patrolPath != null && MoveTo(patrolPath.GetPosition(patrolIndex)) && !bChase)
        {
            if (patrolIndex == (patrolPath.positionCount - 1))
            {
                patrolIndex = 0;
                return;
            }
            else
                patrolIndex++;
    
            MoveTo(patrolPath.GetPosition(patrolIndex));
        }
    }

    private void Chase()
    {
        float x = (float)chasePath[pathIndex].x + pathMoveOffset.x;
        float y = (float)chasePath[pathIndex].y + pathMoveOffset.y;
        pathPointPos = new Vector3(x, y, 0.0f);
        print("PathPos[" + pathIndex + "] : " + pathPointPos);

        bArrived = MoveTo(pathPointPos);
        if (bArrived)
        {
            if (pathIndex == chasePath.Count - 1)
            {
                chasePath.Clear();
                pathIndex = 0;
                bChase = false;
            }

            pathIndex++;
        }
    }

    private bool MoveTo(Vector3 target)
    {
        Vector3 dir = target - transform.position;
        dir.Normalize();

        transform.position += dir * moveSpeed * Time.deltaTime;

        if (target.y > transform.position.y)
            rigid.AddForce(dir * jumpPower, ForceMode2D.Force); //Impulse
        
        Vector3 min = new Vector3(target.x - moveOffset, target.y - moveOffset, target.z);
        Vector3 max = new Vector3(target.x + moveOffset, target.y + moveOffset, target.z);

        if ((transform.position.x >= min.x && transform.position.y >= min.y) && (transform.position.x <= max.x && transform.position.x <= max.x))
            return true;

        return false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 12 || collision.gameObject.tag == "PlayerAttack") //Attachment
        {
            bDamaged = true;
            CombatManager.Get().bPlayerAttackHit = true;

            Vector3 dir = transform.position - collision.gameObject.transform.position;
            dir.Normalize();
            //Vector2 vel = collision.gameObject.GetComponent<Rigidbody2D>().velocity;
            rigid.AddForce(new Vector2(dir.x, dir.y) * attachmentImpact, ForceMode2D.Impulse);
        }

        //if (collision.gameObject.layer == 13) //Bullet
        //{
        //    isDamaged = true;
        //    CombatManager.Get().bPlayerAttackHit = true;
        //
        //    Vector3 dir = transform.position - collision.gameObject.transform.position;
        //    dir.Normalize();
        //    //Vector2 vel = collision.gameObject.GetComponent<Rigidbody2D>().velocity;
        //    Rigidbody.AddForce(new Vector2(dir.x, dir.y) * 0.5f, ForceMode2D.Impulse);
        //}

        //if (collision.gameObject.layer == 9) //MonsterAttack
        //    OnDamaged(collision.gameObject.transform.position, 1);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.layer == 8)// && path.Count == 0)
        {
            chasePath = pathfinder.FindPath(col.transform.position);
            if(chasePath.Count > 0)
                bChase = true;
        }
    }
}