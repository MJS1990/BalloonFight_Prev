using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DelegateFunc;

public class InverseNode : Node
{
    public InverseNode(float tick = 0.0f)
    {
        children = new List<Node>();
        attribute = NodeAttribute.Inverse;
        isSuccesed = true;
        this.tick = tick;
    }

    override public void Execute()
    {
        isSuccesed = !children[0].isSuccesed;
    }
}