using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.Traps
{
    /// <summary>
    /// ¼õËÙ´ø
    /// </summary>
    public class SpeedBump : TrapsItem
    {
        public float speed;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (isLock || !isPlace)
                return;
            if (collision.CompareTag("Player"))
            {
                collision.GetComponent<PlayerController>().runSpeed -= speed;
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (isLock || !isPlace)
                return;
            if (collision.CompareTag("Player"))
            {
                collision.GetComponent<PlayerController>().runSpeed += speed;
            }
        }
    }
}