using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

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
    
    protected bool iscDead; 
    protected bool iscMove; 
    protected bool iscChase; 
    protected bool iscAttack; 
    protected bool iscDamaged;

    
    protected void Initialize()
    {
        originPos = new Vector2(transform.position.x, transform.position.y);
        
        rigid = GetComponent<Rigidbody2D>();

        attackStartRange = 0.02f;

        iscDead = false;
        iscMove = true;
        iscChase = false;
        iscAttack = false;
        iscDamaged = false;
    }
    
    public bool GetDeadCondition()
    {
        if (HP <= 0)
            iscDead = true;
        else
            iscDead = false;
        
        return iscDead;
    }
    
    public bool GetMoveCondition()
    {
        if (iscChase == true || iscAttack == true)
            iscMove = false;
        else
            iscMove = true;

        return iscMove;
    }
    
    public bool GetChaseCondition()
    {
        if (transform.position.x < maxLeftChaseRange || transform.position.x > maxRightChaseRange || iscAttack == true)
        {
            iscChase = false;
            return iscChase;
        }

        dir = rigid.velocity.x > 0 ? 1 : -1;

        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector2.right * dir, 0.5f, LayerMask.GetMask("Player"));

        if (rayHit.collider != null)
            iscChase = true;
       
        return iscChase;
    }
    
    public bool GetAttackCondition()
    {
        dir = rigid.velocity.x > 0 ? 1 : -1;
        
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector2.right * dir * attackStartRange, 0.5f, LayerMask.GetMask("Player"));

        if (rayHit.collider != null)
            iscAttack = true;
        else 
            iscAttack = false;
        
        return iscAttack;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerAttack")
            iscDamaged = true;
    }
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "PlayerAttack")
            iscDamaged = false;
    }

    public bool GetDamagedCondition()
    {
        return iscDamaged;
    }
    
    protected void Stop()
    {
        Vector3 prev = transform.position;
        transform.position = prev;
    }
    
    abstract public bool Dead();
    abstract public bool Move();
    abstract public bool Chase();
    abstract public bool Attack();
    abstract public bool Damaged();
}