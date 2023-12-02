
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.Traps
{
    /// <summary>
    /// 可放置物品父类
    /// </summary>
    public class TrapsItem : MonoBehaviour
    {
        public GameObject body;
        private BoxCollider2D boxCollider2D;
        private bool currentTrggerState;
        public bool isPlace;
        public bool isLock;

        protected virtual void Awake()
        {
            boxCollider2D = GetComponent<BoxCollider2D>();
            currentTrggerState = boxCollider2D.isTrigger;
            Lock();
        }

        /// <summary>
        /// 锁定道具物品(防止物品碰撞 穿模等)
        /// </summary>
        public void Lock()
        {
            //boxCollider2D.isTrigger = true;
            isPlace = false;
            isLock = true;
        }
        public void PlaceLock()
        {
            isLock = true;
        }

        /// <summary>
        /// 解除对道具物品编辑锁定(模型碰撞 道具触发等)
        /// </summary>
        public void UnLock()
        {
            isLock = false;
        }
        public void ResetColliderState()
        {
            boxCollider2D.isTrigger = currentTrggerState;
        }
    }
}