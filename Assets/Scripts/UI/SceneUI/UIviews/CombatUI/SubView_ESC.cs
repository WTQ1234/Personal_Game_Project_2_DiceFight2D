using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Combat
{
    public class SubView_ESC : SubView
    {
        public Button btn_JiXu;
        public Button btn_GameRule;
        public Button btn_Quit;
        
        protected override void Init_Components()
        {
            btn_JiXu = views["Btn_JiXu"].GetComponent<Button>();
            btn_GameRule = views["Btn_GameRule"].GetComponent<Button>();
            btn_Quit = views["Btn_Quit"].GetComponent<Button>();
        }
        protected override void Init_ComponentsValue()
        {
            
            btn_JiXu.onClick.AddListener(() => { Time.timeScale = 1f; Close(); });
            btn_GameRule.onClick.AddListener(()=> { ((CombatUI)baseInterface).view_Explain.Show(); });
            btn_Quit.onClick.AddListener(Application.Quit);
            Close();
        }

        public void ShowGameRule()
        {
            btn_GameRule.onClick.Invoke();
            Time.timeScale = 0f;
        }
    }
}