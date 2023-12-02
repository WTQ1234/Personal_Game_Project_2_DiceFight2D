using HRL;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.LogIn
{
    public class SubView_LogIn : SubView
    {
        public Button btn_StartGame;
        public Button btn_Explain;
        public Button btn_Quit;

        protected override void Init_Components()
        {
            btn_StartGame = views["Btn_StartGame"].GetComponent<Button>();
            btn_Explain = views["Btn_Explain"].GetComponent<Button>();
            btn_Quit = views["Btn_QuitGame"].GetComponent<Button>();
        }
        protected override void Init_ComponentsValue()
        {
            btn_StartGame.onClick.AddListener(() => { Messenger.Instance.BroadcastMsg("StartFight"); });
            btn_Explain.onClick.AddListener(((LogInUI)baseInterface).view_Explain.Show);
            btn_Quit.onClick.AddListener(() => { Application.Quit(); });
        }
    }
}