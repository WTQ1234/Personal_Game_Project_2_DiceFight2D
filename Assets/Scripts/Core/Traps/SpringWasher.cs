using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.Traps
{
    /// <summary>
    /// ���ɵ�
    /// </summary>
    public class SpringWasher : TrapsItem
    {
        public float force;
        public float high;
        protected override void Awake()
        {
            force = 50f;
            high = 20f;
            base.Awake();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (isLock || !isPlace)//��������δ������״̬�²���������
                return;
            if (collision.transform.CompareTag("Player"))
            {
                collision.transform.GetComponent<Rigidbody2D>().AddForce(Vector2.up * (high * force));
            }
        }
    }
}