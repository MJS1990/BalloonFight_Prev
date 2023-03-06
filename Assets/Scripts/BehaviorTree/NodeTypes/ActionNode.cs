using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DelegateFunc;

public class ActionNode : Node
{
    public dAction func;

    public ActionNode(dAction func = null, float tick = 0.0f)
    {
        attribute = NodeAttribute.Action;
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
