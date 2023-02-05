using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

namespace Player
{
    class PlayerAction : MonoBehaviour
    {
        PlayerStatus player;
        
        float h;
        public Sprite[] sprites = new Sprite[2];
        SpriteRenderer spriteRenderer;

        void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();

        }

        private void Awake()
        {
            player = GetComponent<PlayerStatus>();
            //spriteRenderer = GetComponent<SpriteRenderer>();
        }

        void Update()
        {
            InputMove();
            InputJump();

            if(player.gameObject.layer == 10)
            {
                spriteRenderer.sprite = sprites[1];
            }

            //if (Input.GetButtonDown("Fire1"))// && player.GetAnim().GetBool("IsDash") == true)
            //{
            //    player.GetAnim().SetBool("IsJumping", true);
            //}
            //else
            //{
            //    player.GetAnim().SetBool("IsJumping", false);
            //}
        }

        void FixedUpdate()
        {   
            //if (player.HP <= 0) StartCoroutine(Dead());
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Terrain")
            {
                //player.GetAnim().SetBool("IsJumping", false);
                player.GetAnim().SetBool("IsFlying", false);
                player.gameObject.layer = 8;
            }

            //if(collision.gameObject.tag == "Monster")
            //{
            //    if (collision.gameObject.layer == 7)
            //        OnDamaged(collision.gameObject.transform.position, 1);
            //}
        }
        
        //private void OnTriggerEnter2D(Collider2D collision)
        //{
        //    if (collision.gameObject.layer == 7)
        //    {
        //        player.GetAnim().speed = 0.0f;
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

        //void OnDamaged(Vector2 enemyPos, int damage)
        //{
        //    for (int i = 1; i <= damage; i++)
        //    {
        //        if (i > player.HP) break;
        //
        //        int index = player.HP - i;
        //    }
        //
        //    player.HP -= damage;
        //    player.GetSprite().color = new Color(1, 1, 1, 0.4f);
        //
        //    int dir = transform.position.x - enemyPos.x > 0 ? 1 : -1;
        //    player.GetRigid().AddForce(new Vector2(dir, 0.6f) * 0.5f, ForceMode2D.Impulse);
        //
        //    gameObject.layer = 6;
        //    player.GetAnim().SetBool("IsHit", true);
        //
        //    Invoke("OffDamaged", 0.8f);
        //}

        //void OffDamaged()
        //{
        //    player.GetSprite().color = new Color(1, 1, 1, 1);
        //    player.GetAnim().SetBool("IsHit", false);
        //    gameObject.layer = 0;
        //}

        //void ReturnAnimSpeed()
        //{
        //    player.GetAnim().speed = 1.0f;
        //}

        void InputMove()
        {
            if (player.gameObject.layer == 6) return;

            h = Input.GetAxisRaw("Horizontal") * 0.003f;
            player.GetRigid().AddForce(Vector2.right * h, ForceMode2D.Impulse);

            if (Input.GetButton("Horizontal"))
            {
                player.GetAnim().SetBool("IsRunning", true);
                player.GetSprite().flipX = Input.GetAxisRaw("Horizontal") == 1;
            }
            else if (Input.GetButtonUp("Horizontal"))
            {
                player.GetAnim().SetBool("IsRunning", false);

                if (player.GetAnim().GetBool("IsFlying") == false)
                    player.GetRigid().velocity = new Vector2((player.GetRigid().velocity.normalized.x * 0.02f), player.GetRigid().velocity.y);
            }
        }

        void InputJump()
        {
            if (player.gameObject.layer == 6) return;

            if (Input.GetButtonDown("Fire2"))
            {
                //player.GetAnim().SetBool("IsDash", false);
                player.GetAnim().SetBool("IsJumping", true);
                player.GetAnim().SetBool("IsFlying", false);
                player.GetAnim().SetBool("IsRunning", false);

                if (player.gameObject.layer == 10)
                    player.GetRigid().AddForce(Vector2.up * player.JumpPower / 2, ForceMode2D.Impulse);
                else
                    player.GetRigid().AddForce(Vector2.up * player.JumpPower, ForceMode2D.Impulse);

                player.gameObject.layer = 10;
            }
            else if (Input.GetButtonUp("Fire2"))
            {
                player.GetAnim().SetBool("IsJumping", false);
                player.GetAnim().SetBool("IsFlying", true);
            }
            //player.GetAnim().SetFloat("IsFalling", player.GetRigid().velocity.y);
        }
        public void VelocityZero()
        {
            player.GetRigid().velocity = Vector2.zero;
        }

        //public IEnumerator Dead()
        //{
        //    player.GetAnim().SetTrigger("IsDead");
        //    
        //    yield return new WaitForSeconds(0.82f);
        //    player.GetAnim().speed = 0.0f;
        //    player.isDead = true; 
        //    
        //    yield return true;
        //}
    }
}
