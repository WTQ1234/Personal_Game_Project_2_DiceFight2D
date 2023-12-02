using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class ViewControl : MonoBehaviour
    {
        public GameObject window;
        public Dictionary<string, GameObject> views;
        public virtual void Awake()
        {
            window = gameObject;
        }
        /// <summary>
        /// 初始化当前面板
        /// </summary>
        protected virtual void Init_View()
        {
            Init_Components();
            Init_ComponentsValue();
        }
        /// <summary>
        /// 初始化组件
        /// </summary>
        protected virtual void Init_Components() { }
        /// <summary>
        /// 初始化数值
        /// </summary>
        protected virtual void Init_ComponentsValue() { }

        /// <summary>
        /// 显示UI界面
        /// </summary>
        public virtual void Show() { window.SetActive(true); }
        /// <summary>
        /// 隐藏UI界面
        /// </summary>
        public virtual void Close() { window.SetActive(false); }

        /// <summary>
        /// 刷新UI界面
        /// </summary>
        public virtual void Refesh() { }
    }
}