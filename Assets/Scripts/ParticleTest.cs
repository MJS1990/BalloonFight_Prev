using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTest : MonoBehaviour
{
    ParticleSystem paritcle;
    ParticleSystem.VelocityOverLifetimeModule vm;

    void Start()
    {
        paritcle = GetComponent<ParticleSystem>();

        vm = new ParticleSystem.VelocityOverLifetimeModule();
        vm = paritcle.velocityOverLifetime;
    }

    void Update()
    {
        if(Input.GetButtonDown("TestButton1"))
        {
            vm.x = 50.0f;
            print("vm.x : " + vm.x.constant);

            paritcle.Play();
        }
        if (Input.GetButtonDown("TestButton2"))
        {
            vm.x = -50.0f;
            print("vm.x : " + vm.x.constant);

            paritcle.Play();
        }
    }
}
