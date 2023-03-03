using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    PlayerAction player;
    public Image fadePanel;
    SpawnManager spawnManager;

    private void Start()
    {
        //spawnManager = new SpawnManager();
        spawnManager = GetComponent<SpawnManager>();

        StartCoroutine(FadeOut());
    }

    private void Update()
    {
        spawnManager.UpdateSpawn();
    }

    public void MoveNextScene()
    {
        StartCoroutine(CMoveNextScene());
    }

    public void MoveGameOverScene()
    {
        StartCoroutine(CMoveGameOverScene());
    }


    IEnumerator CMoveNextScene()
    {
        int index = SceneManager.GetActiveScene().buildIndex + 1;

        spawnManager.ReadSpawnDatas(index);
        spawnManager.ResetTime();

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