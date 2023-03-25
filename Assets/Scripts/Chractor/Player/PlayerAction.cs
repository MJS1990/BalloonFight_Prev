using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

class PlayerAction : MonoBehaviour
{
    PlayerStatus status;
    
    float h;
    Rigidbody2D Rigidbody;
    SpriteRenderer SpriteRenderer;
    Animator Anim;

    //Test
    Vector3 pos;

    void Start()
    {
    }

    private void Awake()
    {
        status = GetComponent<PlayerStatus>();
        
        Rigidbody = GetComponent<Rigidbody2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        Anim = GetComponent<Animator>();
    }

    void Update()
    {
        InputMove();
        InputJump();
    }

    void FixedUpdate()
    {
        //if (player.HP <= 0) StartCoroutine(Dead());
    }

    public Vector3 GetPosition()
    {
        return gameObject.transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Terrain")
        {
            //Anim.SetBool("IsJumping", false);
            Anim.SetBool("IsFlying", false);
            gameObject.layer = 8;
        }

        if (collision.gameObject.layer == 7) //Monster
        {
            int dir = transform.position.x - collision.gameObject.transform.position.x > 0 ? 1 : -1;
            Rigidbody.AddForce(new Vector2(dir, 0.6f) * 0.7f, ForceMode2D.Impulse);
        }

        if (collision.gameObject.layer == 9) //MonsterAttack
            OnDamaged(collision.gameObject.transform.position, 1);
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.layer == 7)
    //    {
    //        Anim.speed = 0.0f;
    //
    //        Invoke("ReturnAnimSpeed", 0.15f);
    //    }
    //
    //    if (collision.gameObject.layer == 9)
    //    {
    //        int damage = collision.GetComponent<Monster>().GetAttackDamage();
    //        if (isAttack == false)
    //            OnDamaged(collision.gameObject.transform.position, damage);
    //    }
    //}

    void OnDamaged(Vector2 enemyPos, int damage)
    {
        for (int i = 1; i <= damage; i++)
        {
            if (i > status.HP) break;
    
            int index = status.HP - i;
        }
    
        status.HP -= damage;
        SpriteRenderer.color = new Color(1, 1, 1, 0.4f);
    
        int dir = transform.position.x - enemyPos.x > 0 ? 1 : -1;
        Rigidbody.AddForce(new Vector2(dir, 0.6f) * 0.5f, ForceMode2D.Impulse);
    
        gameObject.layer = 6;
        Anim.SetBool("IsHit", true);
    
        Invoke("OffDamaged", 0.8f);
    }

    void OffDamaged()
    {
        SpriteRenderer.color = new Color(1, 1, 1, 1);
        Anim.SetBool("IsHit", false);
        gameObject.layer = 0;
    }

    //void ReturnAnimSpeed()
    //{
    //    Anim.speed = 1.0f;
    //}

    void InputMove()
    {
        if (gameObject.layer == 6) return;

        h = Input.GetAxisRaw("Horizontal") * 0.003f;
        Rigidbody.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        if (Input.GetButton("Horizontal"))
        {
            Anim.SetBool("IsRunning", true);
            SpriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == 1;
        }
        else if (Input.GetButtonUp("Horizontal"))
        {
            Anim.SetBool("IsRunning", false);

            if (Anim.GetBool("IsFlying") == false)
                Rigidbody.velocity = new Vector2((Rigidbody.velocity.normalized.x * 0.02f), Rigidbody.velocity.y);
        }
    }

    void InputJump()
    {
        if (gameObject.layer == 6) return;

        if (Input.GetButtonDown("Fire2"))
        {
            //Anim.SetBool("IsDash", false);
            Anim.SetBool("IsJumping", true);
            Anim.SetBool("IsFlying", false);
            Anim.SetBool("IsRunning", false);

            if (gameObject.layer == 10)
                Rigidbody.AddForce(Vector2.up * status.JumpPower / 2, ForceMode2D.Impulse);
            else
                Rigidbody.AddForce(Vector2.up * status.JumpPower, ForceMode2D.Impulse);

            gameObject.layer = 10;
        }
        else if (Input.GetButtonUp("Fire2"))
        {
            Anim.SetBool("IsJumping", false);
            Anim.SetBool("IsFlying", true);
        }
        //Anim.SetFloat("IsFalling", Rigidbody.velocity.y);
    }
    public void VelocityZero()
    {
        Rigidbody.velocity = Vector2.zero;
    }

    //public IEnumerator Dead()
    //{
    //    Anim.SetTrigger("IsDead");
    //    
    //    yield return new WaitForSeconds(0.82f);
    //    Anim.speed = 0.0f;
    //    player.isDead = true; 
    //    
    //    yield return true;
    //}
}
