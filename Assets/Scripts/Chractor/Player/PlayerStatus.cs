using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using Unity.IO.LowLevel.Unsafe;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    public struct DT_Status
    {
        public int Id;
        public float Gravity;
        public float Velocity;
        public float JumpPower;
        public float Acceleration;
        public float AccelerationMax;
        public int HpCount;
    }

    public struct DT_ExpData
    {
        public int level;
        public int expToNextLv;
        public int rewardGroup;
        public int rewardCount;
    }

    public enum ERewardType
    {
        Weapon = 0,
        Projectile,
        PlayerHp,
    }

    public struct DT_RewardData
    {
        public int id;
        public int rewardGroup;
        public ERewardType rewardType;
        public string rewardParam;
        public int rewardPercentage;
    }


    DT_Status status;
    [HideInInspector]
    public int currentHp = 5;
    public int maxHp;

    //골드
    [HideInInspector]
    public int currentGold = 9;


    //List<DT_ExpData> expDatas;
    DT_ExpData[] expDatas;

    Queue<DT_RewardData> rewardDatas;
    //List<DT_RewardData> rewardDatats;

    List<DT_RewardData> rewardList;

    //UI
    public LevelUpScene lvUI;

    [SerializeField]
    Image hpBarBack;
    [SerializeField]
    Image hpBarFront;
    //

    int level = 1;
    int maxLevel = 10;
    float ratio;
    float inExp = 0.0f; //받은 경험치량
    int levelUpCount = 0;

    //임시 능력치
    [HideInInspector]
    public float jumpPower;
    [HideInInspector]
    public int hpCount;

    public DT_Status GetStatus() { return status; }
    public int GetCurrentHP() { return currentHp; }
    public int GetLevel() { return level; }
    //public int GetExp() { return (int)currentExp;}

    private void Awake()
    {
        status = new DT_Status();
        ratio = 0.0f;
        expDatas = new DT_ExpData[maxLevel];
        //expDatas = new List<DT_ExpData>();
        ReadExpTable();

        maxHp = currentHp;        

        rewardDatas = new Queue<DT_RewardData>();
        ReadRewardTable();
        rewardList = new List<DT_RewardData>();

        //ReadStatus(status);
        //currentHP = status.HpCount;

        //임시 능력치
        status.JumpPower = 8;//jumpPower;
        status.HpCount = hpCount;

        //hpBarBack.rectTransform.position = new Vector3(transform.position.x, transform.position.y + 5.0f, 0.0f);
    }

    public void SetExp(int exp)
    {
        if (level >= maxLevel) return;

        inExp += exp;
        ratio = 1.0f / expDatas[level - 1].expToNextLv;

        lvUI.currentExp = exp * ratio;
        lvUI.remain = 1 - (inExp * ratio);

        lvUI.bGaugeUp = true;
    }

    public void CalcEXPRatio()
    {
        //inExp -= expDatas[level - 2].expToNextLv;
        //ratio = 1 / expDatas[level - 1].expToNextLv;
        //lvUI.currentExp = inExp * ratio;

        float remainExp = lvUI.remain * expDatas[level - 2].expToNextLv;
        lvUI.currentExp = remainExp / expDatas[level - 1].expToNextLv;
        print("===currentExp : " + remainExp / expDatas[level - 1].expToNextLv);
    }

    private void FixedUpdate()
    {
        //if (level <= maxLevel && currentExp >= expDatas[level - 1].expToNextLv)
        //    LevelUp();

        //if (level <= maxLevel && levelUpCount > 0 && !lvUI.bAciveUI)
        //{
        //    LevelUp();
        //}        
    }

    public void LevelUp()
    {
        if (expDatas[level].rewardCount == 0 || level > maxLevel) return;

        //오른 레벨에 맞는 보상리스트 가져오기
        for (int i = 0; i < rewardDatas.Count(); i++)
        {
            if (rewardDatas.Peek().rewardGroup != expDatas[level].rewardGroup)
                break;

            rewardList.Add(rewardDatas.Dequeue());
        }

        //확률 높은순으로 보상리스트 정렬
        rewardList = rewardList.OrderByDescending(x => x.rewardPercentage).ToList();
        List<int> rewardId = new List<int>(); //UI에 넘겨줄 i값

        for(int i = 0; i < expDatas[level].rewardCount; i++)
        {
            print("==========================================");
            print("id : " + rewardList[i].id);
            print("RewardGroup : " + rewardList[i].rewardGroup);
            print("RewardType : " + rewardList[i].rewardType);
            print("RewardPercentage : " + rewardList[i].rewardPercentage);
            print("RewardParam : " + rewardList[i].rewardParam);
            print("==========================================");
        
            rewardId.Add(rewardList[i].id);
            //switch (rewardList[i].rewardType)
            //{ 
            //    case (ERewardType.Weapon):
            //        {
            //            
            //            break;
            //        }
            //    case (ERewardType.Projectile):
            //        {
            //
            //
            //            break;
            //        }
            //    case (ERewardType.PlayerHp):
            //        {
            //            status.HpCount++;
            //
            //            break;
            //        }            
            //} //switch()
        } //for()
        //lvUI.OnGrowthScene(rewardId);
        lvUI.SetRewardData(rewardId);
        //if (lvUI.lvUpGauge > 1)
        //{
        //    inExp -= expDatas[level - 1].expToNextLv;   
        //    currentExp -= expDatas[level - 1].expToNextLv;
        //    lvUI.lvUpGauge = inExp / expDatas[level].expToNextLv; //비율로 계산, 올라야 할 게이지량
        //    lvUI.currentExp = currentExp / expDatas[level].expToNextLv; //현재 오른 게이지량
        //
        //    lvUI.bGaugeUp = true;
        //}
        
        ////lvUI.OnGrowthScene(rewardId);

        level++;
        levelUpCount--;
        rewardList.Clear();
    }

    public void OnHpBarUI(float damage)
    {
        if(currentHp > 0)
        {
            hpBarFront.gameObject.transform.localScale -= new Vector3(hpBarFront.gameObject.transform.localScale.x * (damage / currentHp), 0.0f, 0.0f);
            hpBarFront.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2((hpBarFront.gameObject.transform.localScale.x * 1.5f), hpBarFront.gameObject.GetComponent<RectTransform>().anchoredPosition.y);
        }
    }

    //TODO : 플레이어 능력치 테이블 만든 후 수정
    //public void ReadStatusTable()
    //{
    //    List<Dictionary<string, object>> statusTable = CSVReader.Read("Table_Player");
    //
    //    //Read Status
    //    status.Id = (System.Int32)statusTable["Id"];
    //    status.Gravity = float.Parse(statusTable["Gravity"]);
    //    status.Velocity = float.Parse(statusTable["Velocity"]);
    //    status.JumpPower = float.Parse(statusTable["JumpPower"]);
    //    status.Acceleration = float.Parse(statusTable["Acceleration"]);
    //    status.AccelerationMax = float.Parse(statusTable["AccelerationMax"]);
    //    status.HpCount = (System.Int32)statusTable["HpCount"];
    //}

    public void ReadExpTable()
    {
        List<Dictionary<string, object>> levelUpTable = CSVReader.Read("Table_Player_Exp");

        for (int i = 0; i < levelUpTable.Count; i++)
        {
            DT_ExpData temp = new DT_ExpData();

            temp.level = (System.Int32)levelUpTable[i]["Level"];
            
            if((levelUpTable[i]["ExpToNextLv"].ToString() != ""))
                temp.expToNextLv = (System.Int32)levelUpTable[i]["ExpToNextLv"];
            
            temp.rewardGroup = (System.Int32)levelUpTable[i]["RewardGroup"];
            temp.rewardCount = (System.Int32)levelUpTable[i]["RewardCount"];

            //expDatas.Add(temp);
            expDatas[i] = temp;
        }
    }

    public void ReadRewardTable()
    {
        List<Dictionary<string, object>> rewardTable = CSVReader.Read("Table_Player_Reward");

        for (int i = 0; i < rewardTable.Count; i++)
        {
            DT_RewardData temp;
            
            temp.id = (System.Int32)rewardTable[i]["Id"];
            temp.rewardGroup = (System.Int32)rewardTable[i]["RewardGroup"];
            temp.rewardType = (ERewardType)Enum.Parse(typeof(ERewardType), rewardTable[i]["RewardType"].ToString());
            temp.rewardParam = rewardTable[i]["RewardParam"].ToString();            
            //확률 미리 계산
            int rewardWeights = (System.Int32)rewardTable[i]["RewardWeights"];
            temp.rewardPercentage = UnityEngine.Random.Range(1, 10) * rewardWeights;

            rewardDatas.Enqueue(temp);
            //rewardDatats.Add(temp);
        }
    }
}
