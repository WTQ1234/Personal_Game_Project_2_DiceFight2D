using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HRL;
using Sirenix.OdinInspector;

namespace HRL
{
    public class TriggerInteractiveable : Trigger, Interactive
    {
        public UIViewInScene prefab_UIViewInScene;
        public Vector3 override_offset;

        // TODO: 此处应当包装一个Input类，用于改建
        public KeyCode keyCode = KeyCode.E;
        public UIInfo uiInfo;
        public bool IsInteractiving;

        public Collider2D collider2D_trigger;

        protected UIViewInScene cur_interact_view_inscene;

        protected virtual void Awake()
        {
            collider2D_trigger = GetComponent<Collider2D>();
            IsInteractiving = false;
        }

        void Start()
        {
            cur_interact_view_inscene = UIManager.Instance.CreateFollowingUI(prefab_UIViewInScene, gameObject, false, uiInfo);
            if (override_offset != Vector3.zero)
            {
                cur_interact_view_inscene.offset = override_offset;
            }
        }

        protected override bool _Detect(Collider2D collision)
        {
            if (collision == null)
            {
                return true;
            }
            var entity = collision.GetComponent<Entity>();
            if (entity != null && entity.isPlayer())
            {
                return true;
            }
            return false;
        }

        protected override void _Execute(Collider2D collision)
        {
            if (IsInteractiving)
            {
                return;
            }
            cur_interact_view_inscene.OnShown();
            Player.Instance.OnSetInteractive(this, keyCode);
        }

        protected override void _Reset(Collider2D collision)
        {
            cur_interact_view_inscene.OnHide();
            Player.Instance.OnRemoveInteractive(this, keyCode);
        }

        // 暂时只支持一个交互选项
        public virtual void OnInteract()
        {
            if (IsInteractiving)
            {
                return;
            }
            IsInteractiving = true;
            cur_interact_view_inscene.OnHide();
        }

        public virtual void OnInteractEnd()
        {
            if (!IsInteractiving)
            {
                return;
            }
            IsInteractiving = false;
            List<Collider2D> collider2Ds = new List<Collider2D>();
            Physics2D.OverlapCollider(GetComponent<Collider2D>(), new ContactFilter2D(), collider2Ds);
            if (collider2Ds.Count > 0)
            {
                foreach (Collider2D collider in collider2Ds)
                {
                    _Execute(collider);
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (override_offset != Vector3.zero)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(override_offset + transform.position, 0.1f);
            }
            else if (prefab_UIViewInScene != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(prefab_UIViewInScene.offset + transform.position, 0.1f);
            }
        }
    }
}