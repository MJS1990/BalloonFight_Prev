using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Enemy_Common_BT : MonoBehaviour
{
    Enemy_Common enemy;

    [HideInInspector]
    public BehaviorTree bt;

    RootNode root;
    SequenceNode seqTest;
    TaskNode tTest; 

    void Awake()
    {
        //bt = gameObject.AddComponent<BehaviorTree>();
        bt = GetComponent<BehaviorTree>();

        enemy = GetComponent<Enemy_Common>();
        root = new RootNode();

        seqTest = new SequenceNode();
        //tTest = new TaskNode(enemy.Move);
     
        LinkTreeNode();
    }
    
    void FixedUpdate()
    {
        //bt.ExecuteBT(root);
    }
        
    void LinkTreeNode()
    {
        root.SetChild(seqTest);
           //seqTest.SetChild(tTest);
    }
}
