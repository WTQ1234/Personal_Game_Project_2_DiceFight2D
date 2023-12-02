using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.Traps
{
    /// <summary>
    /// 地雷
    /// </summary>
    public class LandMine : TrapsItem
    {
        public AudioSource source;
        public float explosionForce = 10f;     // 爆炸的力度
        public float explosionRadius = 10f;     // 爆炸的半径

        public ParticleSystem ExploObj;

        protected override void Awake()
        {
            base.Awake();
            source = GetComponent<AudioSource>();
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (isLock || !isPlace)
                return;
            if (collision.transform.tag != "Player") return;
            Debug.Log("碰撞到炸弹");

            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.Damage(5);
            }

            source.Play();
            // 获取碰撞点
            Vector2 explosionPosition = collision.contacts[0].point;

            // 对所有在爆炸半径内的Rigidbody2D应用爆炸力
            Collider2D[] colliders = Physics2D.OverlapCircleAll(explosionPosition, explosionRadius);
            foreach (Collider2D nearbyObject in colliders)
            {
                Rigidbody2D rb = nearbyObject.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    Vector2 directionToNearbyObject = (rb.position - explosionPosition).normalized;
                    rb.AddForce(directionToNearbyObject * explosionForce, ForceMode2D.Impulse);
                }
            }

            var obj = Instantiate(ExploObj);
            obj.transform.position = transform.position;
            obj.Play();

            Destroy(body);
        }
    }
}