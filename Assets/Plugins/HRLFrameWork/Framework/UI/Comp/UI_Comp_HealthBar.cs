using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HRL;
using Sirenix.OdinInspector;

namespace HRL
{
    public class UI_Comp_HealthBar : MonoBehaviour
    {
        public Text healthText;
        public Slider healthSlider;
        //public Image healthBar;

        [ShowInInspector, ReadOnly]
        private int HealthCurrent;
        [ShowInInspector, ReadOnly]
        private int HealthMax;

        public void SetMaxHealth(int _HealthMax)
        {
            HealthMax = _HealthMax;
            _RefreshHealthShow();
        }

        public void SetCurrentHealth(int _HealthCurrent)
        {
            HealthCurrent = _HealthCurrent;
            _RefreshHealthShow();
        }

        private void _RefreshHealthShow()
        {
            healthSlider.value = (float)HealthCurrent / (float)HealthMax;
            //healthBar.fillAmount = (float)HealthCurrent / (float)HealthMax;
            healthText.text = HealthCurrent.ToString() + "/" + HealthMax.ToString();
        }
    }
}
