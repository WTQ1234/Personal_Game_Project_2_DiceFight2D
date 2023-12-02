using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace HRL
{
    [RequireComponent(typeof(Collider2D))]
    public class Trigger : SceneEntity
    {
        [Title("触发引用计数")]
        public bool useTriggerCount = true;
        [ShowInInspector, ReadOnly]
        private int cur_trigger_count = 0;

        public bool canInteract = true;

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (!canInteract) { return; }
            if (!_Detect(collision)) { return; }
            if (useTriggerCount)
            {
                cur_trigger_count++;
                if (cur_trigger_count > 1)
                {
                    return;
                }
            }
            _Execute(collision);
        }

        protected virtual void OnTriggerStay2D(Collider2D collision)
        {

        }

        protected virtual void OnTriggerExit2D(Collider2D collision)
        {
            if (!canInteract) { return; }
            if (!_Detect(collision)) { return; }
            if (useTriggerCount)
            {
                cur_trigger_count--;
                if (cur_trigger_count > 0)
                {
                    return;
                }
            }
            _Reset(collision);
        }

        // 此函数涉及状态判断变化，不经过 _Detect 强制触发
        public virtual void AssumeTriggerEnter()
        {
            if (!canInteract)
            {
                return;
            }
            if (useTriggerCount)
            {
                cur_trigger_count++;
                if (cur_trigger_count > 1)
                {
                    return;
                }
            }
            _Execute(null);
        }

        public virtual void AssumeTriggerExit()
        {
            if (!canInteract)
            {
                return;
            }
            _Reset(null);
        }

        protected virtual bool _Detect(Collider2D collision) { return true; }

        protected virtual void _Execute(Collider2D collision) {}

        protected virtual void _Reset(Collider2D collision) {}

        public virtual void SetCanInteract(bool _canInteract)
        {
            canInteract = _canInteract;
        }
    }
}