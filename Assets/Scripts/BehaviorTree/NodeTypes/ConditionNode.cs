using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DelegateFunc;

public class ConditionNode : Node
{
    public dAction func;

    public ConditionNode(dAction func = null, float tick = 0.0f)
    {
        attribute = NodeAttribute.Condition;
        isSuccesed = false;
        this.tick = tick;
        this.func = func;
        children = null;
    }
    
    override public void Execute()
    {
        isSuccesed = func();
    }
}
