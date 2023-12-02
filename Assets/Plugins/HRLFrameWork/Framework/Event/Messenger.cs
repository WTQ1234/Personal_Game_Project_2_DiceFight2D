using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HRL
{
    public partial class MessengeEventType
    {
        public static string TEST = "TEST";
    }

    public delegate int MessengeDelegate(params object[] values);

    public class Messenger : MonoSingleton<Messenger>
    {
        private Dictionary<string, Delegate> Dict_messengeEventType;

        protected override void Awake()
        {
            Dict_messengeEventType = new Dictionary<string, Delegate>();
        }

        private Delegate messageHandler;

        public void AddListener(string messengeEventType, Delegate handler)
        {
            if (Dict_messengeEventType.TryGetValue(messengeEventType, out Delegate func))
            {
                func = Delegate.Combine(func, handler);
                Dict_messengeEventType[messengeEventType] = func;
            }
            else
            {
                Dict_messengeEventType.Add(messengeEventType, handler);
            }
        }

        public void RemoveListener(string messengeEventType, Delegate handler)
        {
            if (Dict_messengeEventType.TryGetValue(messengeEventType, out Delegate func))
            {
                func = Delegate.Remove(func, handler);
                Dict_messengeEventType[messengeEventType] = func;
            }
        }

        public object BroadcastMsg(string messengeEventType, params object[] args)
        {
            if (Dict_messengeEventType.TryGetValue(messengeEventType, out Delegate func))
            {
                return func?.DynamicInvoke(args);
            }
            else
            {
                return null;
            }
        }
    }
}

