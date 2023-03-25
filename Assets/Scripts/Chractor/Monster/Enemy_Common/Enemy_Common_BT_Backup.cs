using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Enemy_Common_BT_Backup : MonoBehaviour
{
//    Enemy_Common enemy;
//
//    public BehaviorTree bt;
//
//    RootNode root;
//    SequenceNode seqDead;
//    TaskNode aDead; 
//    SelectorNode selBranch;
//    SequenceNode seqDamaged; 
//    TaskNode aDamaged;
//    SequenceNode seqAttack;
//    TaskNode aAttack; 
//    SequenceNode seqChase; 
//    TaskNode aChase; 
//    SequenceNode seqMove;
//    TaskNode aMove; 
//
//    void Start()
//    {
//        //goblin = GetComponent<Goblin>();
//        //InitGoblinTree();
//        //LinkTreeNode();
//    }
//    
//    void Update()
//    {
//        //bt.ExecuteBT(root);
//    }
//    
//    void InitGoblinTree()
//    {
//        root = new RootNode();
//
//        seqDead = new SequenceNode();
//        aDead = new TaskNode(enemy.Dead);
//
//        selBranch = new SelectorNode();
//
//        seqDamaged = new SequenceNode();
//        aDamaged = new TaskNode(enemy.Damaged);
//
//        seqAttack = new SequenceNode();
//        aAttack = new TaskNode(enemy.Attack);
//
//        seqChase = new SequenceNode();
//        aChase = new TaskNode(enemy.Chase);
//
//        seqMove = new SequenceNode();
//        aMove = new TaskNode(enemy.Move);
//    }
//    
//    void LinkTreeNode()
//    {
//        root.SetChild(seqDead);
//                seqDead.SetChild(aDead);
//        root.SetChild(selBranch);
//            selBranch.SetChild(seqMove);
//                seqMove.SetChild(aMove);
//            selBranch.SetChild(seqChase);
//                seqChase.SetChild(aChase);
//            selBranch.SetChild(seqAttack);
//                seqAttack.SetChild(aAttack);
//            selBranch.SetChild(seqDamaged);
//                seqDamaged.SetChild(aDamaged);
//    }
}
