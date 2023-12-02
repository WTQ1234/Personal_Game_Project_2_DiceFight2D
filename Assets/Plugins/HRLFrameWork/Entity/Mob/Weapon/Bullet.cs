using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HRL
{
    public class Bullet : Entity
    {
        public Rigidbody2D rigidbody2D;

        public PlayerAbility_TouchToHurt ability_TouchToHurt;

        public int time_auto_destroy = 10;
        protected int timer_auto_destroy = -1;

        public override EntityType getEntityType()
        {
            return EntityType.Weapon;
        }

        public virtual void OnSetDamage(int _damage)
        {
            if (ability_TouchToHurt != null)
            {
                ability_TouchToHurt.damage = _damage;
            }
        }

        //protected override void Awake()
        //{
        //    base.Awake();
        //    timer_auto_destroy = TimerManager.Instance.AddTimer(_OnTimeOut, time_auto_destroy);
        //}

        protected override void Update()
        {
            if (transform.position.y < -20)
            {
                LogHelper.Info("Destroy", "Destroy GameObject", transform.name, transform.position);
                Destroy(gameObject);
            }
            if (transform.position.y > 20)
            {
                LogHelper.Info("Destroy", "Destroy GameObject", transform.name, transform.position);
                Destroy(gameObject);
            }
        }

        public void OnSetDirection(Vector3 dir)
        {

        }

        public void OnSetVelocity(Vector2 dir, float vel)
        {
            dir.Normalize();
            rigidbody2D.velocity = dir * vel;
        }

        private void OnDisable()
        {
            if (timer_auto_destroy != -1)
            {
                TimerManager.Instance.CancelTimer(timer_auto_destroy);
            }
        }
    }
}