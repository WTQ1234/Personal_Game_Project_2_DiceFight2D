using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.LogIn
{
    public class SubView_Explain : SubView
    {
        public Button btn_ComfirmExplain;
        protected override void Init_Components()
        {
            btn_ComfirmExplain = views["Btn_ComfirmExplain"].GetComponent<Button>();
        }
        protected override void Init_ComponentsValue()
        {
            btn_ComfirmExplain.onClick.AddListener(()=> 
            { 
                Close(); 
                if (CombatManager.Instance.fightState == FightState.ReadyEnterEditor)
                {
                    Time.timeScale = 1f;
                    CombatManager.Instance.ShowEditorUI();
                    CombatManager.Instance.fightState = FightState.InEditor;
                    CombatManager.Instance.isFirstShowRule = false;
                }
            });
            Close();
        }
    }
}