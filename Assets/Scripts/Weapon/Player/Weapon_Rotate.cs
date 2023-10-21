using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Rotate : Weapon
{
    private Rigidbody2D rigid;
    private BoxCollider2D collider;

    //EWeaponType weaponType;

    public float distance = 1.0f;
    public float rotateSpeed = 1.0f;
    
    float angle;
    
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();

        //weaponType = EWeaponType.Rotate; 
        angle = 30.0f;
    }

    void FixedUpdate()
    {
        if (bAttach)
            CalcRadius();
    }

    private void CalcRadius()
    {
        angle += Time.deltaTime * rotateSpeed;
        if (angle <= 0 || angle >= 180)
            rotateSpeed *= -1;

        transform.position = new Vector2(attachParent.transform.position.x + (Mathf.Sin(angle) * distance), attachParent.transform.position.y + (Mathf.Cos(angle) * distance));
    }


    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            bAttach = true;
            transform.parent = attachParent.transform;

            if (rigid)
                rigid.gravityScale = 0.0f;
        }
    }

}