using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject attachParent;

    protected bool bAttach; //°ø¿¡ ¹«Âø

    protected enum EWeaponType
    {
        None = -1,
        IronBall = 0,
        Gun,
        Rotate,
        Bomb,
        Trap,
    }
    
    private void Awake()
    {
        bAttach = false;
    }

    //private void OnTriggerEnter2D(Collider2D collider)
    //{
    //    if (collider.gameObject.tag == "Attachment")
    //    {
    //        bAttach = true;
    //
    //        if (attachParent)
    //            transform.parent = attachParent.transform;
    //
    //        //collider.isTrigger = true;
    //
    //        if(rigid)
    //        {
    //            print("SetGravity");
    //            rigid.gravityScale = 0.0f;
    //            rigid.mass = 0.0f;
    //        }
    //    }
    //    //else if (collision.gameObject.tag == "Player") 
    //    //{
    //    //    bGet = true;
    //    //    transform.parent = attachParent.transform;
    //    //}
    //}
    
    //private void OnTriggerStay2D(Collider2D collider)
    //{
    //    if (collider.gameObject.tag == "Terrain")
    //    {
    //        float y = collider.gameObject.transform.position.y;
    //        transform.position = new Vector3(transform.position.x, y, transform.position.z);
    //    }
    //}
}