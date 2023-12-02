using Game.UI.LogIn;
using HRL;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Combat
{
    public class CombatUI : ParentView
    {
        public SubView_Editor view_Editor;
        public SubView_Combating view_Combating;
        public SubView_RoundOver view_CombatOver;
        public SubView_EndWinner view_EndWinner;
        public SubView_ESC view_ESC;
        public SubView_Explain view_Explain;

        protected override void Init_Components()
        {
            view_Editor = views["SubView_Editor"].AddComponent<SubView_Editor>();
            view_Combating = views["SubView_Combating"].AddComponent<SubView_Combating>();
            view_CombatOver = views["SubView_RoundOver"].AddComponent<SubView_RoundOver>();
            view_EndWinner = views["SubView_EndWinner"].AddComponent<SubView_EndWinner>();
            view_ESC = views["SubView_ESC"].AddComponent<SubView_ESC>();
            view_Explain = views["SubView_Explain"].AddComponent<SubView_Explain>();

        }

        protected override void Init_ComponentsValue()
        {
            view_Editor.Init_SubView(this);
            view_Combating.Init_SubView(this);
            view_CombatOver.Init_SubView(this);
            view_EndWinner.Init_SubView(this);
            view_ESC.Init_SubView(this);
            view_Explain.Init_SubView(this);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!view_ESC.gameObject.activeInHierarchy)
                {
                    Time.timeScale = 0f;
                    view_ESC.Show();
                }
                else
                {
                    Time.timeScale = 1f;
                    view_ESC.Close();
                }
            }
        }
    }
}