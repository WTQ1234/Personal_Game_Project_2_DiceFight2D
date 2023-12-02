using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HRL
{
    public class BuffController : MonoBehaviour
    {
        private Dictionary<int, BuffBase> buff_dict;

        private void Awake()
        {
            buff_dict = new Dictionary<int, BuffBase>();
        }

        public BuffBase AddBuffByCfgId(BuffInfo _buff_info)
        {
            BuffBase buff = (BuffBase)Activator.CreateInstance(_buff_info.Buff.GetType(), _buff_info, this);
            buff.OnExcute();
            buff_dict.Add(buff.GetBuffId(), buff);
            buff.AfterExcute();
            return buff;
        }

        public void RemoveBuffByCfgId(int _cfg_id)
        {
            if (buff_dict.TryGetValue(_cfg_id, out BuffBase buff))
            {
                buff.OnRemoved();
                buff_dict.Remove(buff.GetBuffId());
            }
        }

        private void OnDestroy()
        {
            foreach(BuffBase buff in buff_dict.Values)
            {
                buff.OnRemoved();
            }
            buff_dict.Clear();
        }
    }
}
