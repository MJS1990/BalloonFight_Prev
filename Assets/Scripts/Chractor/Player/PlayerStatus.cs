using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;

//TODO : player한정이 아니라 모든 캐릭터의 상태값을 관리하도록 변경
public class PlayerStatus : MonoBehaviour
{
    enum  EStateType //TODO : getter, setter 만들고 애니메이션 상태값과 매치되도록
    {
        Idle = 0,
    	Hitted,
    	Dead,
    	Action,
    	Max
    };

    enum EMonsterType
    {
        
    }

    enum EWeaponType
    {

    }

    public struct StatusValues
    {
        int Id;
        EMonsterType MonsterType;
        float Gravity;
        float Velocity;
        float Acceleration;
        float AccelerationMax;
        int HpCount;
        EWeaponType Weapon;
    }

    public int MaxHP;
    public int HP;
    //public bool isIdle { get; set; }
    //public bool isHitted { get; set; }
    public bool isDead { get; set; }
    //public bool isAction { get; set; }
    
    public int AttackDamage;

    public float MaxSpeed;
    public float JumpPower;
    public float DashSpeed;
    
    //Table Test
    private void Start()
    {
        List<Dictionary<string, object>> Table = CSVReader.Read("Table_Enemy");
        //List<Dictionary<string, object>> Table = CSVReader.Read("Test");

        //for (int i = 0; i < Table.Count; i++)
        //{
        //print(Table[0]["Id"].ToString());
        //print(Table[0]["MonsterType"].ToString());
        //print(Table[0]["Gravity"].ToString());
        //print(Table[0]["Velocity"].ToString());
        //print(Table[0]["Acceleration"].ToString());
        //print(Table[0]["AccelerationMax"].ToString());
        //print(Table[0]["HpCount"].ToString());
        //print(Table[0]["Weapon"].ToString());    
        //}
    }


    private void Awake()
    {
        HP = MaxHP;
        DashSpeed = 0.0f;
        isDead = false;
    }
}
