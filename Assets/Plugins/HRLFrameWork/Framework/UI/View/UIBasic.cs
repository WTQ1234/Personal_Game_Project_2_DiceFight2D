using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

namespace HRL
{
    public class UIInfo
    {
        private Dictionary<string, UnityAction> params_action;
        private Dictionary<System.Type, Dictionary<string, object>> params_dict;

        public UIInfo()
        {
            params_action = new Dictionary<string, UnityAction>();
            params_dict = new Dictionary<Type, Dictionary<string, object>>();
        }

        public void RegisterParam(string name, object param)
        {
            Type type = param.GetType();
            if (!params_dict.ContainsKey(type))
            {
                params_dict.Add(type, new Dictionary<string, object>());
            }
            var sub_dict = params_dict[type];
            if (sub_dict.ContainsKey(name))
            {
                sub_dict[name] = param;
            }
            else
            {
                sub_dict.Add(name, param);
            }
        }

        public void RegisterAction(string name, UnityAction action)
        {
            if (!params_action.ContainsKey(name))
            {
                params_action.Add(name, action);
            }
            else
            {
                Debug.LogError($"RegisterAction Duplicate: {name}");
            }
        }

        public object GetParam(string name, Type type)
        {
            if (params_dict.ContainsKey(type))
            {
                if (params_dict[type].ContainsKey(name))
                {
                    return params_dict[type][name];
                }
            }
            return null;
        }

        public UnityAction GetAction(string name)
        {
            if (params_action.ContainsKey(name))
            {
                return params_action[name];
            }
            return null;
        }
    }

    public class UIBasic : MonoBehaviour
    {
        public string UIName => this.GetType().Name;
        public UIInfo UIInfo;

        #region LifeTime
        /// <summary>
        /// 此处UIInfo还没赋值
        /// </summary>
        protected virtual void Awake()
        {
            Init();
        }

        protected virtual void Remove()
        {

        }

        /// <summary>
        /// 此处UIInfo赋值
        /// </summary>
        public virtual void OnShown()
        {
            gameObject.SetActive(true);
        }

        public virtual void OnHide()
        {
            gameObject.SetActive(false);
        }

        protected virtual void Update()
        {

        }
        #endregion

        #region Init
        // Init 只会在Awake的时候执行一次
        protected virtual void Init()
        {
            Debug.Log($"Init UIBasic {UIName}");
        }

        public void OnSetUIInfo(UIInfo uiInfo)
        {
            this.UIInfo = uiInfo;
        }
        #endregion

        #region Animation
        protected virtual void DoShowAnimation()
        {

        }

        protected virtual void DoHideAnimation()
        {

        }
        #endregion

        #region Event
        protected virtual void AddListener(string messengeEventType, System.Delegate handler)
        {
            Messenger.Instance.AddListener(messengeEventType, handler);
        }

        protected virtual void RemoveListener(string messengeEventType, System.Delegate handler)
        {
            Messenger.Instance.RemoveListener(messengeEventType, handler);
        }
        #endregion
    }
}