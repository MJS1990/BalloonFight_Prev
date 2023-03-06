using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
 {
     PlayerStatus player;

     public Image[] HPUI;
     int index;
     int prev;

     void Awake()
     {
         player = GetComponent<PlayerStatus>();

         index = player.HP - 1;
         prev = player.HP;
     }

     void FixedUpdate()
     {
         if (player.HP != prev && player.HP >= 0)
         {
             HPUI[index].enabled = false;
             index--;
             prev--;
         }
     }
}
