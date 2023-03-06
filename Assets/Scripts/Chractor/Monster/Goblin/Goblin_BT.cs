using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Goblin_BT : MonoBehaviour
{
    Goblin goblin;

    public BehaviorTree bt;

    RootNode root;
    InverseNode invDead;
    SequenceNode seqDead;
    ConditionNode cDead; 
    ActionNode aDead; 
    SelectorNode selBranch;
    SequenceNode seqDamaged; 
    ConditionNode cDamaged; 
    ActionNode aDamaged;
    SequenceNode seqAttack;
    ConditionNode cAttack;
    ActionNode aAttack; 
    SequenceNode seqChase; 
    ConditionNode cChase; 
    ActionNode aChase; 
    SequenceNode seqMove;
    ConditionNode cMove;
    ActionNode aMove; 

    void Start()
    {
        //goblin = GetComponent<Goblin>();
        //InitGoblinTree();
        //LinkTreeNode();
    }
    
    void Update()
    {
        //bt.ExecuteBT(root);
    }
    
    void InitGoblinTree()
    {
        root = new RootNode();

        invDead = new InverseNode();
        seqDead = new SequenceNode();
        cDead = new ConditionNode(goblin.GetDeadCondition);
        aDead = new ActionNode(goblin.Dead);

        selBranch = new SelectorNode();

        seqDamaged = new SequenceNode();
        cDamaged = new ConditionNode(goblin.GetDamagedCondition);
        aDamaged = new ActionNode(goblin.Damaged);

        seqAttack = new SequenceNode();
        cAttack = new ConditionNode(goblin.GetAttackCondition);
        aAttack = new ActionNode(goblin.Attack);

        seqChase = new SequenceNode();
        cChase = new ConditionNode(goblin.GetChaseCondition);
        aChase = new ActionNode(goblin.Chase);

        seqMove = new SequenceNode();
        cMove = new ConditionNode(goblin.GetMoveCondition);
        aMove = new ActionNode(goblin.Move);
    }
    
    void LinkTreeNode()
    {
        root.SetChild(invDead);
            invDead.SetChild(seqDead);
                seqDead.SetChild(cDead);
                seqDead.SetChild(aDead);
        root.SetChild(selBranch);
            selBranch.SetChild(seqMove);
                seqMove.SetChild(cMove);
                seqMove.SetChild(aMove);
            selBranch.SetChild(seqChase);
                seqChase.SetChild(cChase);
                seqChase.SetChild(aChase);
            selBranch.SetChild(seqAttack);
                seqAttack.SetChild(cAttack);
                seqAttack.SetChild(aAttack);
            selBranch.SetChild(seqDamaged);
                seqDamaged.SetChild(cDamaged);
                seqDamaged.SetChild(aDamaged);
    }
}
