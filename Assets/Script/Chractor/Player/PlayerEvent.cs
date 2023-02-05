using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Player
{
   public class PlayerEvent : MonoBehaviour
    {
        PlayerStatus player;

        public UnityEvent MoveSceneEvent;
        public UnityEvent GameOverEvent;

        private void Start()
        {
            player = GetComponent<PlayerStatus>();
        }

        private void Update()
        {
            if (player.isDead == true)
            {
                GameOverEvent.Invoke();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Finish")
               MoveSceneEvent.Invoke();
        }
    }
}