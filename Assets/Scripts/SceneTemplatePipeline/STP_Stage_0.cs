using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.SceneTemplate;
using UnityEngine.Rendering;

public class STP_Stage_0 : ISceneTemplatePipeline
{
    public virtual bool IsValidTemplateForInstantiation(SceneTemplateAsset sceneTemplateAsset)
    {
        Debug.Log("IsValidTemplateForInstantiation");



        return true;
    }

    public virtual void BeforeTemplateInstantiation(SceneTemplateAsset sceneTemplateAsset, bool isAdditive, string sceneName)
    {
        Debug.Log("BeforeTemplateInstantiation");
    }

    public virtual void AfterTemplateInstantiation(SceneTemplateAsset sceneTemplateAsset, Scene scene, bool isAdditive, string sceneName)
    {
        Debug.Log("AfterTemplateInstantiation");
    }
}
