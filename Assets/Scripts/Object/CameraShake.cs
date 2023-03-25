using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Vector3 dir = new Vector3(1.0f, 0.0f, 0.0f);
    public float shakeFactor = 1.0f;

    void Start()
    {
        shakeFactor *= 45.0f;
    }

    void FixedUpdate()
    {
        print(Mathf.Cos(shakeFactor * Time.deltaTime));

        //gameObject.transform.position += dir * Mathf.Sin(shakeFactor * Time.deltaTime);
    }
}
