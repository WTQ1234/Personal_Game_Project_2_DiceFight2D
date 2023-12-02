using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HRL
{
    public class UI_Comp_Mask : MonoBehaviour
    {
        Button btn_Mask;
        UIScreen screen;

        public void Init(UIScreen _screen)
        {
            screen = _screen;
            btn_Mask = GetComponent<Button>();
            btn_Mask.onClick.AddListener(OnClick_Mask);
        }

        private void OnClick_Mask()
        {
            screen?.OnClick_Mask();
        }
    }
}