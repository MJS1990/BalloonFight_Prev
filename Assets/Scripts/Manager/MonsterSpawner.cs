using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject prefab;
    MonsterStatus monsterStatus;

    GameObject monster;
    Status status;
    float time;

    List<DT_SpawnValues> spawnList;
    public Queue<int> spawnId;// { get; }

    List<int> currentRepeat;

    public struct DT_SpawnValues
    {
        public int id;
        public int stage;
        public int monster;
        public Vector2 spawnPosition;
        public float spawnDelayTime;
        public float spawnPeriod;
        public int spawnRepeat;
        public int spawnCount;
        public int currentRepeat;
        
        public void AddRepeat() { this.currentRepeat++; }
    }

    void Start()
    {
        spawnList = new List<DT_SpawnValues>();
        spawnId = new Queue<int>();

        currentRepeat = new List<int>();
        time = 0.0f;
    }

    public void ResetTime()
    {
        time = 0.0f;
    }

    private IEnumerator CoSpawn(DT_SpawnValues val)
    {
        if (prefab != null)
        {
            for (int i = 0; i < val.spawnRepeat; i++) //SpawnCount
            {
                //TODO : 인스턴스화 전에 몬스터 ID값 설정
                monsterStatus = prefab.gameObject.GetComponent<MonsterStatus>();
                monsterStatus.SetID(val.monster);

                monster = Instantiate(prefab, val.spawnPosition, Quaternion.identity);
                status = monster.transform.GetComponentInChildren<Status>();
                status.ReadStatus(val.monster);
                yield return new WaitForSeconds(val.spawnPeriod);
            }
        }
    }

    public void UpdateSpawn()
    {   
        time += Time.deltaTime;

        for(int i = 0; i < spawnList.Count; i++)
        {
            if (time >= spawnList[i].spawnDelayTime && currentRepeat[i] < spawnList[i].spawnCount)
            {
                StartCoroutine(CoSpawn(spawnList[i]));
                currentRepeat[i] += 1;
            }
        }
    }

    public void ReadSpawnDatas(int stageIndex)
    {
        List<Dictionary<string, object>> Table = CSVReader.Read("Table_Spawn");

        if (spawnList != null)
            spawnList.Clear();

        if (spawnId.Count > 0)
            spawnId.Clear();

        for(int i = 0; i < Table.Count; i++)
        {
            if ((System.Int32)Table[i]["Stage"] > stageIndex) return;

            DT_SpawnValues value;// = new DT_SpawnValues();

            value.id = (System.Int32)Table[i]["Id"];
            value.stage = (System.Int32)Table[i]["Stage"];
            value.monster = (System.Int32)Table[i]["Monster"];
            value.spawnPosition.x = float.Parse(Table[i]["PosX"].ToString());
            value.spawnPosition.y = float.Parse(Table[i]["PosY"].ToString());
            value.spawnDelayTime = float.Parse(Table[i]["SpawnDelayTime"].ToString());
            value.spawnPeriod = float.Parse(Table[i]["SpawnPeriod"].ToString());
            value.spawnRepeat = (System.Int32)Table[i]["SpawnRepeat"];
            value.spawnCount = (System.Int32)Table[i]["SpawnCount"];
            value.currentRepeat = 0;

            spawnList.Add(value);
            currentRepeat.Add(0);
        }
    }
}
