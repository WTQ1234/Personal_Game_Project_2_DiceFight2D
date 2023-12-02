using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace HRL
{
    // TODO: 如果要改键位的话，需要做成存档，将对应的一一映射
    public enum InputOccasion
    {
        Update,
        LateUpdate,
        FixedUpdate,
    }

    public enum ButtonType
    {
        Down = 1 << 0,
        Up = 1 << 1,
        Press = 1 << 2,
    }

    [System.Serializable]
    public class InputData
    {
        [OnValueChanged("SetInputCode")]
        public bool useKeyCode;

        [ShowIf("useKeyCode")]
        [OnValueChanged("SetInputCode")]
        public KeyCode keyCode;
        [HideIf("useKeyCode")]
        public int inputCode;          // 可能为 mouse 或 button
        public ButtonType buttonType;
        public InputOccasion inputOccasion;
        [HideInInspector]
        public UnityAction unityAction_Down;    // 需要主动赋值
        [HideInInspector]
        public UnityAction unityAction_Up;     // 需要主动赋值
        [HideInInspector]
        public UnityAction unityAction_Press;

        // 构造函数分为两种情况  传入 KeyCode 此时针对的是按钮，传入 int 此时针对的是鼠标，为了与键盘区分，鼠标的code取-1
        public InputData(int _inputCode, ButtonType _buttonType, InputOccasion _inputOccasion,
            UnityAction _unityAction_Down = null, UnityAction _unityActiion_Up = null, UnityAction _unityAction_Press = null)
        {
            this.inputCode = _inputCode * -1;
            this.buttonType = _buttonType;
            this.inputOccasion = _inputOccasion;
            this.unityAction_Down = _unityAction_Down;
            this.unityAction_Up = _unityActiion_Up;
            this.unityAction_Press = _unityAction_Press;
        }

        public InputData(KeyCode _keyCode, ButtonType _buttonType, InputOccasion _inputOccasion,
            UnityAction _unityAction_Down = null, UnityAction _unityActiion_Up = null, UnityAction _unityAction_Press = null)
        {
            this.inputCode = (int)_keyCode;
            this.buttonType = _buttonType;
            this.inputOccasion = _inputOccasion;
            this.unityAction_Down = _unityAction_Down;
            this.unityAction_Up = _unityActiion_Up;
            this.unityAction_Press = _unityAction_Press;
        }

        public void SetInputCode()
        {
            if (!useKeyCode)
            {
                return;
            }
            inputCode = (int)keyCode;
        }

        public UnityAction GetCurrentAction()
        {
            switch (buttonType)
            {
                case ButtonType.Down:
                    return unityAction_Down;
                case ButtonType.Up:
                    return unityAction_Up;
                case ButtonType.Press:
                    return unityAction_Press;
                default:
                    return null;
            }
        }
    }

    public class InputManager : MonoSingleton<InputManager>
    {
        // int 为 key
        [ShowInInspector]
        private Dictionary<int,
            Dictionary<ButtonType, UnityAction>> Dict_UpdateAction;
        [ShowInInspector]
        private Dictionary<int,
            Dictionary<ButtonType, UnityAction>> Dict_LateUpdateAction;
        [ShowInInspector]
        private Dictionary<int,
            Dictionary<ButtonType, UnityAction>> Dict_FixedUpdateAction;

        private Dictionary<InputOccasion, List<InputData>> inputDatas_ListToRemove;

        public int mLockPlayerInputNum = 0;

        protected override void Init()
        {
            base.Init();
            mLockPlayerInputNum = 0;
            Dict_UpdateAction = new Dictionary<int, Dictionary<ButtonType, UnityAction>>();
            Dict_LateUpdateAction = new Dictionary<int, Dictionary<ButtonType, UnityAction>>();
            Dict_FixedUpdateAction = new Dictionary<int, Dictionary<ButtonType, UnityAction>>();

            inputDatas_ListToRemove = new Dictionary<InputOccasion, List<InputData>>();
        }

        #region Register and Unregister Action
        public void RegisterInputAction(InputData inputData, bool override_bothButtonType = false)
        {
            if (inputData == null)
            {
                return;
            }
            InputOccasion occasion = inputData.inputOccasion;
            var target_dict = __GetTargetDict(occasion);
            if (override_bothButtonType)
            {
                if (inputData.unityAction_Down != null)
                {
                    __RegisterAction(target_dict, inputData.inputCode, ButtonType.Down, inputData.unityAction_Down);
                }
                if (inputData.unityAction_Up != null)
                {
                    __RegisterAction(target_dict, inputData.inputCode, ButtonType.Up, inputData.unityAction_Up);
                }
                if (inputData.unityAction_Press != null)
                {
                    __RegisterAction(target_dict, inputData.inputCode, ButtonType.Press, inputData.unityAction_Press);
                }
            }
            __RegisterAction(target_dict, inputData.inputCode, inputData.buttonType, inputData.GetCurrentAction());
        }

        public void UnRegisterInputAction(InputData inputData)
        {
            if (inputData == null)
            {
                return;
            }
            InputOccasion occasion = inputData.inputOccasion;
            if (!inputDatas_ListToRemove.ContainsKey(occasion))
            {
                inputDatas_ListToRemove.Add(occasion, new List<InputData>());
            }
            inputDatas_ListToRemove[occasion].Add(inputData);
        }

        private Dictionary<int, Dictionary<ButtonType, UnityAction>> __GetTargetDict(InputOccasion inputOccasion)
        {
            Dictionary<int, Dictionary<ButtonType, UnityAction>> target_dict = null;
            switch (inputOccasion)
            {
                case InputOccasion.Update:
                    target_dict = Dict_UpdateAction;
                    break;
                case InputOccasion.FixedUpdate:
                    target_dict = Dict_FixedUpdateAction;
                    break;
                case InputOccasion.LateUpdate:
                    target_dict = Dict_LateUpdateAction;
                    break;
            }
            return target_dict;
        }

        private void __RegisterAction(Dictionary<int, Dictionary<ButtonType, UnityAction>> target_dict, int code, ButtonType buttonType, UnityAction unityAction)
        {
            LogHelper.Info("Input", "Register Input Action", code, buttonType);
            if (target_dict.TryGetValue(code, out var dict_ButtonTypePairs))
            {
                if (dict_ButtonTypePairs.ContainsKey(buttonType))
                {
                    dict_ButtonTypePairs[buttonType] += unityAction;
                }
                else
                {
                    dict_ButtonTypePairs.Add(buttonType, unityAction);
                }
            }
            else
            {
                var dict = new Dictionary<ButtonType, UnityAction>();
                dict.Add(buttonType, unityAction);
                target_dict.Add(code, dict);
            }
        }

        private void __UnRegisterAction(Dictionary<int, Dictionary<ButtonType, UnityAction>> target_dict, int code, ButtonType buttonType, UnityAction unityAction)
        {
            LogHelper.Info("Input", "Unregister Input Action", code, buttonType);
            if (target_dict.ContainsKey(code))
            {
                var dict_ButtonTypePairs = target_dict[code];
                if (dict_ButtonTypePairs.ContainsKey(buttonType))
                {
                    dict_ButtonTypePairs[buttonType] -= unityAction;
                    if (dict_ButtonTypePairs[buttonType] == null)
                    {
                        dict_ButtonTypePairs.Remove(buttonType);
                    }
                }
                if (dict_ButtonTypePairs.Count == 0)
                {
                    target_dict.Remove(code);
                }
            }
        }
        #endregion

        #region Check Input and Remove
        private void Update()
        {
            CheckInput(Dict_UpdateAction);
            CheckRemove(InputOccasion.Update);
        }

        private void FixedUpdate()
        {
            CheckInput(Dict_FixedUpdateAction);
            CheckRemove(InputOccasion.FixedUpdate);
        }

        private void LateUpdate()
        {
            CheckInput(Dict_LateUpdateAction);
            CheckRemove(InputOccasion.LateUpdate);
        }

        private void CheckInput(Dictionary<int,
            Dictionary<ButtonType, UnityAction>> Dict_Input)
        {
            foreach (var pairs in Dict_Input)
            {
                int code = pairs.Key;
                var Dict_ButtonTypePairs = pairs.Value;
                foreach (var ButtonTypePairs in Dict_ButtonTypePairs)
                {
                    ButtonType buttonType = ButtonTypePairs.Key;
                    UnityAction unityAction = ButtonTypePairs.Value;
                    if (code <= 0)
                    {
                        switch (buttonType)
                        {
                            case ButtonType.Up:
                                if (Input.GetMouseButtonUp(code * -1)) {
                                    if (unityAction == null)
                                    {
                                        Debug.LogError($"unity action is null, buttonType is {buttonType} code is {code}");
                                        return;
                                    }
                                    unityAction.Invoke();
                                }
                                break;
                            case ButtonType.Down:
                                if (Input.GetMouseButtonDown(code * -1))
                                {
                                    if (unityAction == null)
                                    {
                                        Debug.LogError($"unity action is null, buttonType is {buttonType} code is {code}");
                                        return;
                                    }
                                    unityAction.Invoke();
                                }
                                break;
                            case ButtonType.Press:
                                if (Input.GetMouseButton(code * -1))
                                {
                                    if (unityAction == null)
                                    {
                                        Debug.LogError($"unity action is null, buttonType is {buttonType} code is {code}");
                                        return;
                                    }
                                    unityAction.Invoke();
                                }
                                break;
                        }
                    }
                    else
                    {
                        switch (buttonType)
                        {
                            case ButtonType.Up:
                                if (Input.GetKeyUp((KeyCode)code))
                                {
                                    if (unityAction == null)
                                    {
                                        Debug.LogError($"unity action is null, buttonType is {buttonType} code is {code}");
                                        return;
                                    }
                                    unityAction.Invoke();
                                }
                                break;
                            case ButtonType.Down:
                                if (Input.GetKeyDown((KeyCode)code))
                                {
                                    if (unityAction == null)
                                    {
                                        Debug.LogError($"unity action is null, buttonType is {buttonType} code is {code}");
                                        return;
                                    }
                                    unityAction.Invoke();
                                }
                                break;
                            case ButtonType.Press:
                                if (Input.GetKey((KeyCode)code))
                                {
                                    if (unityAction == null)
                                    {
                                        Debug.LogError($"unity action is null, buttonType is {buttonType} code is {code}");
                                        return;
                                    }
                                    unityAction.Invoke();
                                }
                                break;
                        }
                    }
                }
            }
        }

        private void CheckRemove(InputOccasion occasion)
        {
            if (!inputDatas_ListToRemove.ContainsKey(occasion))
            {
                return;
            }
            var list = inputDatas_ListToRemove[occasion];
            if (list.Count <= 0)
            {
                return;
            }
            var target_dict = __GetTargetDict(occasion);
            foreach (var inputData in list)
            {
                int code = inputData.inputCode;
                if (code > 0)
                {

                    __UnRegisterAction(target_dict, inputData.inputCode, inputData.buttonType, inputData.GetCurrentAction());
                }
                else
                {
                    __UnRegisterAction(target_dict, inputData.inputCode, inputData.buttonType, inputData.GetCurrentAction());
                }
            }
            list.Clear();
        }
        #endregion
    }
}