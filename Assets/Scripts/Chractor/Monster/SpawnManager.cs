using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject prefab;
    GameObject enemy;
    Status status;
    float time;

    List<DT_SpawnValues> SpawnList;
    List<int> CurrentRepeat;

    public struct DT_SpawnValues
    {
        public int Id;
        public int Stage;
        public int Monster;
        public Vector2 SpawnPosition;
        public float SpawnDelayTime;
        public float SpawnPeriod;
        public int SpawnRepeat;
        public int SpawnCount;
        public int CurrentRepeat;

        public void AddRepeat() { this.CurrentRepeat++; }
    }

    void Start()
    {
        SpawnList = new List<DT_SpawnValues>();
        CurrentRepeat = new List<int>();
        //ReadSpawnDatas(1); //TODO : 패트롤 테스트 후 해제
        time = 0.0f;
    }

    public void ResetTime()
    {
        time = 0.0f;
    }

    //void Spawn(DT_SpawnValues val)
    //{
    //    if (time >= val.SpawnDelayTime)
    //    {
    //        while(val.CurrentRepeat < val.SpawnRepeat)
    //        {
    //            StartCoroutine(CoSpawn(val));
    //        }
    //    }
    //
    //    //InvokeRepeating("SpawnEnemy2", DelayTime, Period); //2초 후부터 2초마다 반복실행
    //}

    private IEnumerator CoSpawn(DT_SpawnValues val)
    {
        if (prefab != null)
        {
            for (int i = 0; i < val.SpawnRepeat; i++) //SpawnCount
            {
                enemy = Instantiate(prefab, val.SpawnPosition, Quaternion.identity);
                status = enemy.transform.GetComponentInChildren<Status>();
                status.ReadStatus(val.Monster);
                yield return new WaitForSeconds(val.SpawnPeriod);
            }
        }
    }

    public void UpdateSpawn()
    {   
        time += Time.deltaTime;

        for(int i = 0; i < SpawnList.Count; i++)
        {
            if (time >= SpawnList[i].SpawnDelayTime && CurrentRepeat[i] < SpawnList[i].SpawnCount)
            {
                StartCoroutine(CoSpawn(SpawnList[i]));
                CurrentRepeat[i] += 1;
            }
        }

        //if (SpawnCount >= 6) //SpawnRepeat
        //    CancelInvoke("SpawnEnemy2");
    }

    public void ReadSpawnDatas(int stageIndex)
    {
        List<Dictionary<string, object>> Table = CSVReader.Read("Table_Spawn");

        if (SpawnList != null)
            SpawnList.Clear();

        for(int i = 0; i < Table.Count; i++)
        {
            if ((System.Int32)Table[i]["Stage"] > stageIndex) return;

            DT_SpawnValues value;// = new DT_SpawnValues();

            value.Id = (System.Int32)Table[i]["Id"];
            value.Stage = (System.Int32)Table[i]["Stage"];
            value.Monster = (System.Int32)Table[i]["Monster"];
            value.SpawnPosition.x = float.Parse(Table[i]["PosX"].ToString());
            value.SpawnPosition.y = float.Parse(Table[i]["PosY"].ToString());
            value.SpawnDelayTime = float.Parse(Table[i]["SpawnDelayTime"].ToString());
            value.SpawnPeriod = float.Parse(Table[i]["SpawnPeriod"].ToString());
            value.SpawnRepeat = (System.Int32)Table[i]["SpawnRepeat"];
            value.SpawnCount = (System.Int32)Table[i]["SpawnCount"];
            value.CurrentRepeat = 0;

            SpawnList.Add(value);
            CurrentRepeat.Add(0);
        }
    }
}
