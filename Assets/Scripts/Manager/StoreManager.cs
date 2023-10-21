using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class StoreManager : MonoBehaviour
{
    [SerializeField]
    List<Item> items;
    
    int currentGold;

    PlayerStatus playerStatus;

    [SerializeField]
    Text goldText;

    [SerializeField]
    Text itemComment;


    public struct DT_Item
    {
        public int id;
        public string name;
        public string comment;
        public int gold;
        public int count;
        public int index;
    }

    List<DT_Item> itemList = null;

    int selectedIndex;

    void Start()
    {        
        if (itemList == null) //Test
        {
            itemList = new List<DT_Item>();
            print("할당");
            //Test 추후 여기서 테이블 데이터 읽어서 items에 저장
            DT_Item itemData = new DT_Item();
            itemData.id = 0005;
            itemData.name = "hpUp";
            itemData.comment = "HpUp 10!";
            itemData.gold = 2;
            itemData.count = 5;
            itemData.index = 0;
            itemList.Add(itemData);

            DT_Item itemData2 = new DT_Item();
            itemData2.id = 0105;
            itemData2.name = "hpUp2";
            itemData2.comment = "HpUp 5!";
            itemData2.gold = 1;
            itemData2.count = 5;
            itemData2.index = 1;
            itemList.Add(itemData2);
        }



        playerStatus = GameManager.Get().GetPlayer().GetComponent<PlayerStatus>();
        playerStatus.currentGold = 10;
        goldText.text = playerStatus.currentGold.ToString();
        
        for(int i = 0; i < itemList.Count; i++)
        {
            //DataManager.Get().LoadStoreData(itemList[i]);
            itemList[i] = DataManager.Get().LoadStoreData(itemList[i]);
            items[i].ApplyAmount(itemList[i].count);
        }

        print("0번 아이템 : " + itemList[0].count);
        print("1번 아이템 : " + itemList[1].count);
    }

    void FixedUpdate()
    {
    }

    public void GetItem(int itemIndex)
    {
        selectedIndex = itemIndex;
        itemComment.text = itemList[itemIndex].comment;
    }

    public void BuyItem()
    {
        if (itemList[selectedIndex].count == 0)
        {
            itemComment.text = "not enough item!";
            return;
        }
        else if (itemList[selectedIndex].gold > playerStatus.currentGold)
        {
            itemComment.text = "not enough gold!";
            return;
        }

        playerStatus.currentGold -= itemList[selectedIndex].gold;
        goldText.text = playerStatus.currentGold.ToString();

        //TODO : 수정할것
        items[selectedIndex].remainImages[itemList[selectedIndex].count - items[selectedIndex].currentCount].color = new Color(1, 0, 0, 1);
        items[selectedIndex].currentCount -= 1;

        //items[selectedIndex].Buy();

        if(selectedIndex == 0)
        {
            //Test
            playerStatus.maxHp += 10; //임시값
        }
        else if (selectedIndex == 1)
        {
            //Test
            playerStatus.maxHp += 5; //임시값
        }

        DT_Item temp = itemList[selectedIndex];
        temp.count--;
        itemList[selectedIndex] = temp;
        
        print("ITEM : " + itemList[selectedIndex].count);

        DataManager.Get().SaveStoreData(itemList[selectedIndex]);
        DataManager.Get().SavePlayerData();
    }
}