using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Common : Monster
{
    void Awake()
    {
        moveSpeed = status.GetStatus().velocity;
    }

    void FixedUpdate()
    {
        
    }
}
