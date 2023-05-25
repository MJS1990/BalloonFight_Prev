using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Monster_BT : MonoBehaviour
{
    Monster monster;

    [HideInInspector]
    public BehaviorTree bt;

    RootNode root;
    SequenceNode seqTest;
    TaskNode tTest; 

    void Awake()
    {
        //bt = gameObject.AddComponent<BehaviorTree>();
        bt = GetComponent<BehaviorTree>();

        monster = GetComponent<Monster>();
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
