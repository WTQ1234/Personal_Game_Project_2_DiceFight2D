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
        /// ��ʼ����ǰ���
        /// </summary>
        protected virtual void Init_View()
        {
            Init_Components();
            Init_ComponentsValue();
        }
        /// <summary>
        /// ��ʼ�����
        /// </summary>
        protected virtual void Init_Components() { }
        /// <summary>
        /// ��ʼ����ֵ
        /// </summary>
        protected virtual void Init_ComponentsValue() { }

        /// <summary>
        /// ��ʾUI����
        /// </summary>
        public virtual void Show() { window.SetActive(true); }
        /// <summary>
        /// ����UI����
        /// </summary>
        public virtual void Close() { window.SetActive(false); }

        /// <summary>
        /// ˢ��UI����
        /// </summary>
        public virtual void Refesh() { }
    }
}