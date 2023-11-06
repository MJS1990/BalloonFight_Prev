using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
//using System.Runtime.InteropServices;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Monster : MonoBehaviour
{
    [SerializeField]
    protected PlayerStatus player;

    protected MonsterState state;
    protected MonsterStatus status;

    protected int maxhp;
    protected int currenthp;
    public float attachmentImpact = 2.0f;
    public float moveSpeed = 5.0f;
    public float minSpeed = 5.0f;
    public float maxSpeed = 8.0f;
    public float moveOffset = 0.05f;
    public float moveForce = 0.0f;
    public float jumpPower = 5.0f;
    public float moveOffsetTime = 0.0f;

    //Patrol
    protected LineRenderer patrolPath;
    protected int patrolIndex = 0;

    //PathFind
    //protected Monster_Pathfinding pathfinder;
    protected List<Vector2Int> chasePath;
    protected GameObject target;
    protected Vector3 targetPos;
    protected Vector3 pathPointPos;
    protected Vector2 pathMoveOffset;
    protected int pathIndex = 0;
    protected bool bArrived = false;
    protected float chaseTime = 0.0f;

    protected bool bDamaged = false;
    //int takeDamage = 0;
    [SerializeField]
    protected float knockBackPower;
    protected float knockBackOrigin;
    protected Vector3 knockBackDir;
    [SerializeField]
    protected float knockbackTime;
    protected float kTime;

    //NoneDamage
    [SerializeField]
    protected float noneDamageTime = 0.0f;

    //Groggy

    //Dead
    protected float deadTime = 0.0f;

    //Test
    protected int PatrolCount = 0;
    protected int ChaseCount = 0;
    protected int MoveCount = 0;
    protected int HittedCount = 0;


    public Monster Get() { return this; }
    public MonsterStatus GetStatus() { return status; }
    public MonsterState GetState() { return state; }
    public void Awake()
    {
        target = GameObject.Find("Player");
    }

    public bool Patrol()
    {
        if (patrolPath == null || patrolPath.positionCount <= 0)
            return false;

        targetPos = patrolPath.GetPosition(patrolIndex);

        if (MoveTo())
        {
            if (patrolIndex == (patrolPath.positionCount - 1))
                patrolIndex = 0;
            else
                patrolIndex++;
        }

        PatrolCount++;

        return true;
    }
    public bool MoveTo()
    {
        Vector3 dir = targetPos - transform.position;
        dir.Normalize();
        transform.position += dir * moveSpeed * Time.deltaTime;
        //rigid.AddForce(dir * moveSpeed, ForceMode2D.Force); //Force
        moveSpeed -= 0.3f;

        ////Jump
        //if (target.y > transform.position.y)
        //    rigid.AddForce(dir * jumpPower, ForceMode2D.Impulse);
        //else if (target.y < transform.position.y)
        //    rigid.AddForce(-dir * jumpPower, ForceMode2D.Impulse);

        //이동 도착위치 설정
        Vector3 min = new Vector3(targetPos.x - moveOffset, targetPos.y - moveOffset, targetPos.z);
        Vector3 max = new Vector3(targetPos.x + moveOffset, targetPos.y + moveOffset, targetPos.z);
        //도착했다면 true
        if ((transform.position.x > min.x && transform.position.y > min.y) && (transform.position.x < max.x && transform.position.y < max.y))
        {
            state.SetIdle();
            return true;
        }

        MoveCount++;

        return false;
    }

    public virtual bool Chase() { return false; }
    public virtual void GetDamage(int damage, Vector3 dir) {}
    public virtual bool Hitted() { return false; }
    public virtual void End_Hitted() {}
    public virtual bool NoneDamaged(float time) { return false; }
    public virtual bool Groggy() { return false; }
    public virtual bool Dead(){ return false; }
    public virtual void End_Dead() {}
}