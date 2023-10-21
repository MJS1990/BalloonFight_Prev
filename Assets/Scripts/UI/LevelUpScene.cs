using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpScene : MonoBehaviour
{
    GameObject growthCanvas;
    [SerializeField]
    GameObject player;


    [SerializeField]
    RectTransform listPos;
    float onListPos = -280.0f;
    float offListPos = 240.0f;

    [SerializeField]
    RectTransform buttonPos;
    float onButtonPos = -630.0f;
    float offButtonPos = -860.0f;

    float tempTime = 0.0f;

    [HideInInspector]
    public float inExp = 0.0f; //올라야 할 경험치
    [HideInInspector]
    public float ratio = 0.0f; //경험치 1당 경험치바 길이값
    [HideInInspector]
    public float currentExp;
    float totalExp;
    [HideInInspector]
    public float remain;


    [HideInInspector]
    public bool bAciveUI = false;
    [HideInInspector]
    public bool bGaugeUp = false;
    [HideInInspector]
    public bool bLevelUp = false;


    [SerializeField]
    Image[] rewardPanelImages;
    [SerializeField]
    List<Image> rewardImages;

    List<TextMesh> rewardText;
    
    List<int> rewardId;

    //UI
    [SerializeField]
    Image expBar;

    public void SetRewardData(List<int> id) { rewardId = id; }

    private void Start()
    {
        totalExp = 0.0f;

        growthCanvas = transform.GetChild(0).gameObject;

        rewardImages = new List<Image>();
        rewardPanelImages = new Image[3];

        rewardText = new List<TextMesh>();

        rewardId = new List<int>();

        //originGauge = expBar.gameObject.transform.localScale.x;
        expBar.gameObject.transform.localScale = new Vector3(0.0f, expBar.gameObject.transform.localScale.y, expBar.gameObject.transform.localScale.z);
    }

    private void Update()
    {
        if (bAciveUI)
        {
            //패널 이동
            tempTime += 0.005f;
            if (listPos.anchoredPosition.y != onListPos)
            {
                listPos.anchoredPosition = new Vector2(listPos.anchoredPosition.x, Mathf.Lerp(listPos.anchoredPosition.y, onListPos, tempTime));
            }

            if (buttonPos.anchoredPosition.y != onButtonPos)
            {
                buttonPos.anchoredPosition = new Vector2(buttonPos.anchoredPosition.x, Mathf.Lerp(buttonPos.anchoredPosition.y, onButtonPos, tempTime));
            }
        }        
    }

    private void FixedUpdate()
    {
        //경험치바 증가
        if (currentExp > 0.0f)
        {            
            //한번에 오름
            if((totalExp + currentExp) < 1)
            {
                print(currentExp + "만큼 오름");
                expBar.gameObject.transform.localScale += new Vector3(currentExp, 0.0f, 0.0f);
                expBar.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2((expBar.gameObject.transform.localScale.x * 50), expBar.gameObject.GetComponent<RectTransform>().anchoredPosition.y);
                totalExp += currentExp;
                currentExp = 0.0f;
            }
            else //게이지가 다 차고 남은 경험치가 있다면
            {
                expBar.gameObject.transform.localScale = new Vector3(1.0f, expBar.gameObject.transform.localScale.y, expBar.gameObject.transform.localScale.z);
                expBar.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2((expBar.gameObject.transform.localScale.x * 50), expBar.gameObject.GetComponent<RectTransform>().anchoredPosition.y);

                PlayerStatus st = player.GetComponent<PlayerStatus>();
                st.LevelUp();

                remain = (totalExp + currentExp) - 1;
                print("==reamin : " + remain);

                st.CalcEXPRatio();
                
                OnGrowthScene(rewardId);
                expBar.gameObject.transform.localScale = new Vector3(0.0f, expBar.gameObject.transform.localScale.y, expBar.gameObject.transform.localScale.z);
                totalExp = 0.0f;

                //expBar.gameObject.transform.localScale += new Vector3(currentExp, 0.0f, 0.0f);
                //expBar.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2((expBar.gameObject.transform.localScale.x * 50), expBar.gameObject.GetComponent<RectTransform>().anchoredPosition.y);
                //totalExp += currentExp;
                //currentExp = 0.0f;
            }

            print("======================위치 : " + expBar.transform.position);
            print("======================크기 : " + expBar.transform.localScale);

            //순차적으로 오름
            //expBar.gameObject.transform.localScale += new Vector3(0.25f * Time.deltaTime, 0.0f, 0.0f);
            //currentExp -= 0.25f * Time.deltaTime;

            //경험치 오른 후 위치 재조정
            //expBar.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2((expBar.gameObject.transform.localScale.x * 50), expBar.gameObject.GetComponent<RectTransform>().anchoredPosition.y);

            //if (expBar.gameObject.transform.localScale.x >= 1) //expBar.transform.localScale.x //경험치바가 다 찼다면
            //{
            //    bLevelUp = true;
            //    bGaugeUp = false;
            //    expBar.gameObject.transform.localScale = new Vector3(0.0f, expBar.gameObject.transform.localScale.y, expBar.gameObject.transform.localScale.z);
            //
            //    PlayerStatus st = player.GetComponent<PlayerStatus>();
            //    st.LevelUp();
            //    //st.CalcEXPRatio();
            //
            //    OnGrowthScene(rewardId);
            //}
        }
    }

    public void  OnLevelUpScene()
    {
        print("LevelUp");
        //경험치바 리셋
        expBar.gameObject.transform.localScale = new Vector3(0.0f, 1.0f, 1.0f);

        //패널 이동
        tempTime += 0.005f;
        if (listPos.anchoredPosition.y != onListPos)
        {
            listPos.anchoredPosition = new Vector2(listPos.anchoredPosition.x, Mathf.Lerp(listPos.anchoredPosition.y, onListPos, tempTime));
        }

        if (buttonPos.anchoredPosition.y != onButtonPos)
        {
            buttonPos.anchoredPosition = new Vector2(buttonPos.anchoredPosition.x, Mathf.Lerp(buttonPos.anchoredPosition.y, onButtonPos, tempTime));
        }


        //if (lvUpGauge < 1) bLevelUp = false;
    }

    public void OnGrowthScene(List<int> id)
    {
        rewardId = id;

        Time.timeScale = 0.0f;
        growthCanvas.gameObject.SetActive(true);
        bAciveUI = true;
    }

    public void OffGrowthScene()
    {
        growthCanvas.gameObject.SetActive(false);
        bAciveUI = false;
        bLevelUp = false;

        listPos.anchoredPosition = new Vector2(listPos.anchoredPosition.x, offListPos);
        buttonPos.anchoredPosition = new Vector2(buttonPos.anchoredPosition.x, offButtonPos);

        rewardImages.Clear();
        //rewardPanelImages.Clear();
        rewardText.Clear();
        rewardId.Clear();

        Time.timeScale = 1.0f;
        tempTime = 0.0f;
    }
}
