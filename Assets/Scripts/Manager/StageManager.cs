using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEditor.SceneTemplate;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    [SerializeField]
    List<SceneTemplateAsset> StageSet_0;
    [SerializeField]
    List<SceneTemplateAsset> StageSet_1;
    [SerializeField]
    List<SceneTemplateAsset> StageSet_2;
    [SerializeField]
    List<SceneTemplateAsset> StageSet_3;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public SceneTemplateAsset LoadStage(int index)
    {
        SceneTemplateAsset scene = new SceneTemplateAsset();

        switch(index)
        {
            case 0:
                {
                    int r = UnityEngine.Random.Range(0, StageSet_0.Count);
                    scene = StageSet_0[r];
                    break;
                }
            case 1:
                {
                    int r = UnityEngine.Random.Range(0, StageSet_1.Count);
                    scene = StageSet_1[r];
                    break;
                }
            case 2:
                {
                    int r = UnityEngine.Random.Range(0, StageSet_2.Count);
                    scene = StageSet_2[r];
                    break;
                }
            case 3:
                {
                    int r = UnityEngine.Random.Range(0, StageSet_3.Count);
                    scene = StageSet_3[r];
                    break;
                }
            default:
                break;
        }

        return scene;
    }
}
