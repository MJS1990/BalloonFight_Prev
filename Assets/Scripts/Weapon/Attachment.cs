using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attachment : MonoBehaviour
{
    Rigidbody2D rigidbody;
    CircleCollider2D collider;

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

    void FixedUpdate()
    {
        
    }
}
