using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor.SceneTemplate;
using UnityEditor.SceneManagement;
using System.IO;
//using UnityEditor.SearchService;

[System.Serializable]
public class DicIntScene : SerializableDictionary<int, string> {}

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;

    [SerializeField]
    StoreManager storeManager;
    [SerializeField]
    StageManager stageManager;

    [SerializeField]
    private GameObject player;

    PlayerAction playerAction;
    PlayerStatus playerStatus;

    [HideInInspector]
    public int stageClearIndex = 0;
    [HideInInspector]
    public int keyStageClearIndex = 0;

    public Image fadePanel;
    MonsterSpawner spawner;

    [SerializeField]
    DicIntScene sceneList;

    private void Start()
    {
        /////////////////////////////////////////////////////////////////////////////

        //print(st.templatePipeline.name);
        ////st.description.TrimStart();
        ////SceneTemplateService.Instantiate(st, true, "Assets / Scenes / Test");
        //SceneManager.LoadScene(SceneTemplateService.Instantiate(st, false).scene.buildIndex);
        ////SceneManager.LoadSceneAsync(SceneTemplateService.Instantiate(st, true).scene.buildIndex);

        /////////////////////////////////////////////////////////////////////////////

        instance = this;

        if(SceneManager.sceneCount != 0)
            StartCoroutine(FadeOut());

        //데이터로드
        DataManager.Get().LoadPlayerData();
        DataManager.Get().LoadStageData();

        spawner = GetComponent<MonsterSpawner>();
        //Test Spawn
        spawner.ReadSpawnDatas(1);
    }

    public static GameManager Get()
    {
        if (!instance)
            return null;

        return instance;
    }

    public GameObject GetPlayer()
    {
        if (player == null)
            return null;

        return player;
    }

    private void FixedUpdate()
    {
        spawner.UpdateSpawn();
        if(spawner.spawnId.Count > 0)
        {
            print("==========================");
            while(spawner.spawnId.Count > 0)
            {
                if (spawner.spawnId.Count == 0) break;
        
                print("spawnId : " + spawner.spawnId.Dequeue());
            }
            print("==========================");
        }
    }

    public void MoveNextScene()
    {
        StartCoroutine(CMoveNextScene());
    }
    public void MoveNextScene(int index)
    {
        StartCoroutine(CMoveNextScene(index));
    }
    public void MovePrevScene()
    {
        StartCoroutine(CMovePrevScene());
    }

    public void MoveGameOverScene()
    {
        StartCoroutine(CMoveGameOverScene());
    }

    IEnumerator CMoveNextScene()
    {
        int index = SceneManager.GetActiveScene().buildIndex + 1;

        if(index > 3) //스테이지 인덱스라면
        {
            spawner.ReadSpawnDatas(index);
            spawner.ResetTime();

            if (index > stageClearIndex)
            {
                if((index - 3) % 5 == 0) //키 스테이지라면
                    keyStageClearIndex = index;

                stageClearIndex = index;
            }

            //TODO : 스테이지가 넘어갈때마다 데이터 저장
            DataManager.Get().SaveStageData();
            DataManager.Get().SavePlayerData();
        }

        StartCoroutine(FadeIn());
        yield return new WaitForSeconds(1.25f);
    
        SceneManager.LoadScene(index);
    }

    IEnumerator CMoveNextScene(int index) //스테이지 셀렉트 화면에서 이미 클리어한 스테이지만 접근
    {
        int count = Directory.GetFiles("Assets/Scenes/Stages/" + index.ToString()).Length / 2;
        if(count > 0)
        {
            spawner.ReadSpawnDatas(index);
            spawner.ResetTime();

            StartCoroutine(FadeIn());
            yield return new WaitForSeconds(1.25f);

            //씬 로드
            int r = UnityEngine.Random.Range(0, count);
            SceneManager.LoadScene("Stage_" + r.ToString());
        }
    }

    IEnumerator CMovePrevScene()
    {
        int index = SceneManager.GetActiveScene().buildIndex - 1;

        spawner.ReadSpawnDatas(index);
        spawner.ResetTime();

        StartCoroutine(FadeIn());
        yield return new WaitForSeconds(1.25f);

        SceneManager.LoadScene(index);
    }


    IEnumerator CMoveGameOverScene()
    {
        yield return new WaitForSeconds(1.25f);

        StartCoroutine(FadeIn());
        yield return new WaitForSeconds(1.25f);

        SceneManager.LoadScene("GameOver");
    }

    IEnumerator FadeIn()
    {
        while(fadePanel.color.a < 1.0f)
        {
            fadePanel.color += new Color(0.0f, 0.0f, 0.0f, 0.01f);
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator FadeOut()
    {
        while (fadePanel.color.a > 0.0f)
        {
           fadePanel.color -= new Color(0.0f, 0.0f, 0.0f, 0.01f);
            yield return new WaitForSeconds(0.01f);
        }
    }
}