using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Effect : MonoBehaviour
{
    Animator effectAnim;
    RuntimeAnimatorController ac;
    AnimatorStateInfo info;

    [HideInInspector]
    public float length;
    public float currentTime;

    public bool loop;
    public bool bEnd;
    bool bStart;

    private void Start()
    {
        effectAnim.enabled = false;
        length = 0.0f;
        loop = false;
        bStart = false;
        bEnd = false;
    }

    private void Awake()
    {
        effectAnim = GetComponent<Animator>();
        ac = effectAnim.runtimeAnimatorController;
        info = effectAnim.GetCurrentAnimatorStateInfo(0);
    }

    public void Reset()
    {
        effectAnim.enabled = false;
        bStart = false;
        bEnd = false;
        length = 0.0f;
        currentTime = 0.0f;
    }

    private void Update()
    {
        if(bStart)
        {
            currentTime += Time.deltaTime;
            length = (currentTime / info.length);
            
            if (length >= 1.0f)
            {
                bStart = false;
                bEnd = true;
                //Reset();
            }
        }
    }

    public void SetAnim(Animator newAnim)
    {
        effectAnim = newAnim;
    }

    public void Play()
    {
        if(effectAnim)
        {
            bStart = true;
            effectAnim.enabled = true;
        }
    }
}
