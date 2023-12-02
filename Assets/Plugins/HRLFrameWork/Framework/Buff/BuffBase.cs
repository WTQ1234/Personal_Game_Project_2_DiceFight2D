using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HRL
{
    public class BuffBase
    {
        protected BuffInfo buff_info;
        protected BuffController buff_controller;

        public BuffBase(BuffInfo _buff_info, BuffController _buff_controller)
        {
            Debug.Log("BuffBase");
            buff_info = _buff_info;
            buff_controller = _buff_controller;
        }

        public BuffInfo GetBuffInfo()
        {
            return buff_info;
        }

        public int GetBuffId()
        {
            return GetBuffInfo().Id;
        }

        public virtual void OnExcute()
        {

        }

        public virtual void AfterExcute()
        {
            if (buff_info.ExcuteAtOnce)
            {
                _RemoveSelf();
            }
        }

        public virtual void OnRefresh()
        {

        }

        public virtual void OnFinished()
        {

        }

        public virtual void OnRemoved()
        {

        }

        private void _AddAttr()
        {

        }

        private void _RemoveAttr()
        {

        }

        private void _Refresh()
        {

        }

        private void _StartTick()
        {

        }

        private void _Tick()
        {

        }

        private void _RemoveSelf()
        {

        }
    }
}
