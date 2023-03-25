using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskNode : Node
{
    public delegate bool dTask();

    public dTask func;
    
    public TaskNode(dTask func = null, float tick = 0.0f)
    {
        attribute = NodeAttribute.Task;
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
