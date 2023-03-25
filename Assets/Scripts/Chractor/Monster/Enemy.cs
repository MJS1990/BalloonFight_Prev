using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract class Enemy : MonoBehaviour
{
    protected int HP { get; set; }
    protected int attackDamage { get; set; }
    public int GetAttackDamage() { return attackDamage; }
    
    protected bool isDamaged = false;

    protected Rigidbody2D rigid { get; set; }
    
    protected Vector3 originPos;
    protected int dir = 1;
    
    public float attackStartRange;
    
    protected void Initialize()
    {
        isDamaged = false;
        originPos = new Vector2(transform.position.x, transform.position.y);
    
        rigid = GetComponent<Rigidbody2D>();
    
        attackStartRange = 0.02f;
    }
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerAttack")
            isDamaged = true;
    }
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "PlayerAttack")
            isDamaged = false;
    }
    
    protected void Stop()
    {
        Vector3 prev = transform.position;
        transform.position = prev;
    }

    //bool MoveTo(Vector3 target)
    //{
    //    if (transform.position == target)
    //        return true;
    //
    //    float moveSpeed = 4.5f;
    //    float jumpPower = 0.8f;
    //
    //    Vector3 dir = target - transform.position;
    //    dir.Normalize();
    //
    //    if(transform.position.y < target.y)
    //        rigid.AddForce(dir * jumpPower / 2, ForceMode2D.Impulse);
    //    else
    //        transform.position += dir * moveSpeed * Time.deltaTime;
    //    //transform.position += new Vector3((moveSpeed * Time.deltaTime), 0.0f, 0.0f);
    //
    //    return false;
    //}

    abstract public bool Dead();
    abstract public bool Move();
    abstract public bool Chase();
    abstract public bool Attack();
    abstract public bool Damaged();
}