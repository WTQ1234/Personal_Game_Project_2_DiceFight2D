using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.Traps
{
    /// <summary>
    /// µØ´Ì
    /// </summary>
    public class GroundSpike : TrapsItem
    {
        public bool canAttack;
        public float currentCD;
        public float CD;
        public int damage = 1;
        public float force;
        public float length;
        public float high;
        protected override void Awake()
        {
            canAttack = true;
            currentCD = 0f;
            CD = 1f;
            base.Awake();
        }
        private void OnCollisionStay2D(Collision2D collision)
        {
            if (isLock || !isPlace)
                return;
            if (canAttack && collision.collider.CompareTag("Player"))
            {
                Transform playerTsf = collision.transform;
                playerTsf.GetComponent<Player>().Damage(damage);
                Debug.Log(playerTsf.rotation.y);
                Vector2 dir = new Vector2(length, playerTsf.position.y + high);
                switch (playerTsf.rotation.y)
                {
                    case -1f: { dir.x *= -1; } break;
                    case 0: { dir.x *= 1; } break;
                }
                dir.x *= -1;
                playerTsf.GetComponent<Rigidbody2D>().AddForce(dir * force);
                StartCoroutine(AttackCD());
            }
        }
        /// <summary>
        /// ½øÈë¹¥»÷CD
        /// </summary>
        /// <returns></returns>
        IEnumerator AttackCD()
        {
            canAttack = false;
            currentCD = 0;
            while (currentCD < CD)
            {
                currentCD += Time.deltaTime;
                yield return null;
            }
            canAttack = true;
        }
    }
}