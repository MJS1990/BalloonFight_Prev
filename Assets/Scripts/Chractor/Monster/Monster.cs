using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract class Monster : MonoBehaviour
{
    protected int HP { get; set; }
    protected int attackDamage { get; set; }
    public int GetAttackDamage() { return attackDamage; }

    protected Rigidbody2D rigid { get; set; }

    protected Vector3 originPos;
    protected int dir = 1;
    
    protected float maxRightChaseRange;
    protected float maxLeftChaseRange;
    
    public float attackStartRange;
    
    protected bool isAlive; 
    protected bool isMove; 
    protected bool isChase; 
    protected bool isAttack; 
    protected bool isDamaged;

    
    protected void Initialize()
    {
        originPos = new Vector2(transform.position.x, transform.position.y);
        
        rigid = GetComponent<Rigidbody2D>();

        attackStartRange = 0.02f;

        isAlive = true;
        isMove = true;
        isChase = false;
        isAttack = false;
        isDamaged = false;
    }
    
    public bool GetDeadCondition()
    {
        if (HP <= 0)
            isAlive = false;
        
        return isAlive;
    }
    
    public bool GetMoveCondition()
    {
        if (isChase == true || isAttack == true)
            isMove = false;
        else
            isMove = true;

        return isMove;
    }
    
    public bool GetChaseCondition()
    {
        if (transform.position.x < maxLeftChaseRange || transform.position.x > maxRightChaseRange || isAttack == true)
        {
            isChase = false;
            return isChase;
        }

        dir = rigid.velocity.x > 0 ? 1 : -1;

        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector2.right * dir, 0.5f, LayerMask.GetMask("Player"));

        if (rayHit.collider != null)
            isChase = true;
       
        return isChase;
    }
    
    public bool GetAttackCondition()
    {
        dir = rigid.velocity.x > 0 ? 1 : -1;
        
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector2.right * dir * attackStartRange, 0.5f, LayerMask.GetMask("Player"));

        if (rayHit.collider != null)
            isAttack = true;
        else 
            isAttack = false;
        
        return isAttack;
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

    public bool GetDamagedCondition()
    {
        return isDamaged;
    }
    
    protected void Stop()
    {
        Vector3 prev = transform.position;
        transform.position = prev;
    }
    
    //abstract public bool Dead();
    //abstract public bool Move();
    //abstract public bool Chase();
    //abstract public bool Attack();
    //abstract public bool Damaged();
}