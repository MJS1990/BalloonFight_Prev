using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DelegateFunc;

public class FixedNode : Node
{
    bool fixValue;

    public FixedNode(bool fixValue, float tick = 0.0f)
    {
        children = new List<Node>();

        attribute = NodeAttribute.Fixed;
        isSuccesed = false;
        this.fixValue = fixValue;
        this.tick = tick;
    }

    override public void Execute()
    {
        isSuccesed = fixValue;
    }
}
