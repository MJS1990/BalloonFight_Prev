using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DelegateFunc;

public abstract class Node
{
    public enum NodeAttribute
    {
        Root = 0,

        Sequence,
        Inverse,
        Selector, 

        //Decorator
        If,
        Loop,
        Fixed,
        Random,

        Condition, 
        Action,
    }

    public NodeAttribute attribute;
    public bool isSuccesed;
    public float tick;
    public List<Node> children;

    public abstract void Execute();

    public void SetChild(Node node)
    {
        children.Add(node);
    }

    public bool GetChildResult(int index)
    {
        return children[index].isSuccesed;
    }
}
