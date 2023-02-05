using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

namespace Player
{
    public class PlayerStatus : MonoBehaviour
    {
        Rigidbody2D rigid;
        SpriteRenderer spriteRenderer;
        Animator anim;

        public int HP;
        int MaxHP;
        public bool isDead { get; set; }

        public int AttackDamage;

        public float MaxSpeed;
        public float JumpPower;
        public float DashSpeed;

        public Rigidbody2D GetRigid() { return rigid; }
        public SpriteRenderer GetSprite() { return spriteRenderer; }
        public Animator GetAnim() { return anim; }

        private void Awake()
        {
            rigid = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            anim = GetComponent<Animator>();

            MaxHP = HP;
            DashSpeed = 0.0f;
            isDead = false;
        }
    }
}