using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attachment : MonoBehaviour
{
    Rigidbody2D rigidbody;
    CircleCollider2D collider;

    public Vector2 weight = new Vector2(0.0f, -2.5f);

    Animator anim;

    public Effect effect;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<CircleCollider2D>();
        anim = GetComponent<Animator>();

        effect.gameObject.transform.localScale = new Vector3(3.0f, 3.0f, 1.0f);
    }

    public Vector2 GetWeight()
    {
        return weight;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 7) //Monster
        {
            ////collision.gameObject.GetCompoenent<Enemy_Common>.isDamaged = true;
            //float imapactValue = collision.gameObject.GetComponent<Enemy_Common>.attachmentImpact;
            //
            //Vector3 dir = collision.gameObject.transform.position - transform.position;
            //dir.Normalize();
            //collision.gameObject.GetComponent<Rigidbody2D>.AddForce(new Vector2(dir.x, dir.y) * rigidbody.velocity * attachmentImpact, ForceMode2D.Impulse);

            //Animation
            effect.gameObject.transform.position = collision.GetContact(0).point;
            effect.Play();
        }

        //if (collision.gameObject.layer == 12) //Ather Attachment
        //{
        //    ContactPoint2D contact = collision.contacts[0];
        //    Vector2 attachPos = contact.point;
        //    collision.gameObject.transform.position = attachPos;
        //}
    }
}
