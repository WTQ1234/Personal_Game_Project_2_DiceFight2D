using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.Traps
{
    /// <summary>
    /// ����
    /// </summary>
    public class LandMine : TrapsItem
    {
        public AudioSource source;
        public float explosionForce = 10f;     // ��ը������
        public float explosionRadius = 10f;     // ��ը�İ뾶

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
            Debug.Log("��ײ��ը��");

            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.Damage(5);
            }

            source.Play();
            // ��ȡ��ײ��
            Vector2 explosionPosition = collision.contacts[0].point;

            // �������ڱ�ը�뾶�ڵ�Rigidbody2DӦ�ñ�ը��
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