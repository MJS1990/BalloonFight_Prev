using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DataManager  : MonoBehaviour
{
    private static DataManager instance = null;


    void Awake()
    {
        instance = this;
    }

    public static DataManager Get()
    {
        if (!instance)
            return null;

        return instance;
    }

    void FixedUpdate()
    {
        
    }

    public void SavePlayerData()
    {
        PlayerStatus playerStatus = GameManager.Get().GetPlayer().GetComponent<PlayerStatus>();
        PlayerPrefs.SetInt("Gold", playerStatus.currentGold);
        PlayerPrefs.SetInt("MaxHP", playerStatus.maxHp);
    }

    public void SaveWeaponData()
    {

    }


    public void SaveStoreData(StoreManager.DT_Item itemData)
    {
        PlayerPrefs.SetInt(itemData.name, itemData.count);
        print("æ∆¿Ã≈€ : " + PlayerPrefs.GetInt(itemData.name));
    }

    public void SaveStageData()
    {
        PlayerPrefs.SetInt("ClearStageIndex", GameManager.Get().stageClearIndex);
        PlayerPrefs.SetInt("ClearKeyStageIndex", GameManager.Get().keyStageClearIndex);
    }


    //Load///////////////////////////////////////////////////////////////////////////////
    public void LoadPlayerData()
    {
        PlayerStatus playerStatus = GameManager.Get().GetPlayer().GetComponent<PlayerStatus>();

        playerStatus.currentGold = PlayerPrefs.GetInt("Gold");
        playerStatus.maxHp = PlayerPrefs.GetInt("MaxHP");
    }

    public void LoadWeaponData()
    {

    }


    public StoreManager.DT_Item LoadStoreData(StoreManager.DT_Item itemData)
    {
        //itemData.count = PlayerPrefs.GetInt(itemData.name);
        
        StoreManager.DT_Item temp;
        temp = itemData;
        temp.count = PlayerPrefs.GetInt(itemData.name);

        return temp;
    }

    public void LoadStageData()
    {
        GameManager.Get().stageClearIndex = PlayerPrefs.GetInt("ClearStageIndex");
        GameManager.Get().keyStageClearIndex = PlayerPrefs.GetInt("ClearKeyStageIndex");
    }

}
