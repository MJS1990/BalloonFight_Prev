using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attachment : MonoBehaviour
{
    [HideInInspector]
    public Rigidbody2D rigidbody;
    [HideInInspector]
    public CircleCollider2D collider;

    public Vector2 weight = new Vector2(0.0f, -2.5f);

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<CircleCollider2D>();
    }

    public Vector2 GetWeight()
    {
        return weight;
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.layer == 12) //Monster
    //    {
    //        //collision.gameObject.GetCompoenent<Enemy_Common>.isDamaged = true;
    //        float imapactValue = collision.gameObject.GetComponent<Enemy_Common>.attachmentImpact;
    //
    //        Vector3 dir = collision.gameObject.transform.position - transform.position;
    //        dir.Normalize();
    //        collision.gameObject.GetComponent<Rigidbody2D>.AddForce(new Vector2(dir.x, dir.y) * rigidbody.velocity * attachmentImpact, ForceMode2D.Impulse);
    //    }
    //}
}
