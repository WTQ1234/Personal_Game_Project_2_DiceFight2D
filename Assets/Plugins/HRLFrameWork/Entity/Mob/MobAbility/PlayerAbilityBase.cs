using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace HRL
{
    public class PlayerAbilityBase : MonoBehaviour
    {
        public bool invokeByInput = true;
        [ShowIf("invokeByInput")]
        public InputData inputData;

        public bool invokeByLongPress = true;
        [ShowIf("invokeByLongPress")]
        public bool isCurrentLongPress = false;
        [ShowIf("invokeByLongPress")]
        public float tickTimeLongPress = 0.1f;
        [ShowIf("invokeByLongPress")]
        public float currentTickTimeLongPress = 0f;

        public bool invokeByCD = true;
        [ShowIf("invokeByCD")]
        public float CD_Max = 0.1f;
        [ShowIf("invokeByCD")]
        public float CD_Current = 0f;

        [ReadOnly]
        public string Name;

        public bool isEnable;

        public Entity Owner
        {
            get
            {
                if (_owner == null)
                {
                    _owner = GetComponent<Entity>();
                }
                return _owner;
            }
        }
        protected Entity _owner;

        void Awake() { OnAbilityAdd(); }

        void OnEnable() { OnAbilityEnable(); }

        void OnDisable() { OnAbilityDisable(); }

        void Update()
        {
            if (invokeByLongPress && isCurrentLongPress)
            {
                currentTickTimeLongPress += Time.deltaTime;
                if (currentTickTimeLongPress > tickTimeLongPress)
                {
                    currentTickTimeLongPress = 0;
                    _TickLongPress();
                }
            }
            if (invokeByCD && (CD_Current < CD_Max))
            {
                CD_Current += Time.deltaTime;
            }
        }

        protected virtual void OnAbilityAdd()
        {
            _SetName();
            if (invokeByInput)
            {
                inputData.unityAction_Down = OnAbilityCall;
                inputData.unityAction_Up = OnAbilityCancel;
                InputManager.Instance.RegisterInputAction(inputData, invokeByLongPress);
            }
        }

        protected virtual void OnAbilityRemove()
        {
            InputManager.Instance.UnRegisterInputAction(inputData);
        }

        protected virtual void OnAbilityEnable()
        {
            LogHelper.Info("Ability", "Ability Enable", Name);
            if (invokeByCD)
            {
                CD_Current = CD_Max;
            }
            if (invokeByLongPress)
            {
                isCurrentLongPress = false;
                currentTickTimeLongPress = 0f;
            }
        }

        protected virtual void OnAbilityDisable()
        {
            LogHelper.Info("Ability", "Ability Disable", Name);
        }

        public virtual void OnAbilityCall()
        {
            // TODO 冷却时间，魔力消耗等检测  魔力消耗，次数等等考虑使用OnAbilityDisable？
            // 禁用分为多种情况，如区域禁用，次数不足等，考虑到次数不足可能需要播放动画等，所以不能在disable里return
            if (!this.enabled)
            {
                return;
            }
            if (invokeByCD && (CD_Max > 0))
            {
                if (CD_Current < CD_Max)
                {
                    return;
                }
            }
            bool res = _Execute();
            if (res && invokeByCD)
            {
                if (CD_Max > 0)
                {
                    CD_Current = 0;
                }
            }
        }

        public virtual void OnAbilityCancel()
        {
            if (!this.enabled)
            {
                return;
            }
            _Cancel();
        }

        protected virtual void _SetName() { Name = "Base and Valid"; }

        protected virtual bool _Execute() { return true; }

        protected virtual void _Cancel() { }

        protected virtual void _TickLongPress() { _Execute(); }


        public bool invokeByTarget = false;
        [ShowIf("invokeByTarget")]
        public Entity currentTarget;

        public void OnSetEntityTarget(Entity target)
        {
            if (!invokeByTarget)
            {
                return;
            }
            currentTarget = target;
        }
    }
}