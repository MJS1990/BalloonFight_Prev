using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private static CameraShake instance = null;

    public float YAxis_shakeLength = 1.0f;
    public float XAxis_shakeLength = 1.0f;

    public float shakeSpeed = 1.0f;
    float amplitude = 0.0f;    
    public float shakeTime = 1.0f;

    bool bShake = false;

    void Start()
    {
        instance = this;
    }

    public static CameraShake Get()
    {
        if (!instance)
            return null;

        return instance;
    }

    void FixedUpdate()
    {
        if (bShake)
            Shake();
    }

    public bool GetbShake() { return bShake; }
    public void SetbShake(bool val) { bShake = val; }

    void Shake()
    {
        amplitude += Time.deltaTime * shakeSpeed;
        
        gameObject.transform.position = new Vector3(gameObject.transform.position.x + (XAxis_shakeLength * Mathf.Sin((amplitude))), gameObject.transform.position.y + (YAxis_shakeLength * Mathf.Sin((amplitude))), gameObject.transform.position.z);

        if (amplitude >= shakeTime)
        {
            bShake = false;
            amplitude = 0.0f;
            gameObject.transform.position = new Vector3(transform.parent.gameObject.transform.position.x, transform.parent.gameObject.transform.position.y, gameObject.transform.position.z);
        }
    }
}
