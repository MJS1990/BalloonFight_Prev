using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTree : MonoBehaviour
{
    private bool isTreeEnable;
    
    public double callCount;

    void Start()
    {
        isTreeEnable = true;
        callCount = 0.0d;
    }

    private IEnumerator BTUpdate(Node node, float tick = 0.0f)
    {
        if (node != null) 
        {
            switch (node.attribute)
            {
                case Node.NodeAttribute.Root:
                {
                    for (int i = 0; i < node.children.Count; i++)
                        StartCoroutine(BTUpdate(node.children[i], node.children[i].tick));
                    
                    node.Execute();
                    yield return new WaitForSeconds(tick);

                    if (node.isSuccesed == false)
                        isTreeEnable = false;

                    break;
                }
                case Node.NodeAttribute.Sequence:
                {
                    for (int i = 0; i < node.children.Count; i++)
                    {
                        StartCoroutine(BTUpdate(node.children[i], node.children[i].tick));
                        if (node.children[i].isSuccesed == false)
                        {
                            node.isSuccesed = false;
                            break;
                        }
                    }

                    yield return new WaitForSeconds(tick);

                    break;
                }
                case Node.NodeAttribute.Selector:
                {
                    for (int i = 0; i < node.children.Count; i++)
                    {
                        StartCoroutine(BTUpdate(node.children[i], node.children[i].tick));
                    }

                    node.Execute();
                    yield return new WaitForSeconds(tick);

                    break;
                }
                case Node.NodeAttribute.Task:
                {
                    node.Execute();
                    yield return new WaitForSeconds(tick);

                    break;
                }
            }
        }
    }

    public void ExecuteBT(RootNode root)
    {
        if (isTreeEnable == true)
            StartCoroutine(BTUpdate(root, root.tick));
    }
}