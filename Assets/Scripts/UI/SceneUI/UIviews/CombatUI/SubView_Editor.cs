using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Combat
{
    public class SubView_Editor : SubView
    {
        private Text txt_GameHelpInfo;
        private GameObject view_LastTime;
        private Text txt_LastTime;
        public Button btn_SkipEditorTime;

        private Text txt_Angel_Items;
        private Text txt_Demo_Items;

        public GameObject view_HeaderInfo;
        protected override void Init_Components()
        {
            txt_GameHelpInfo = views["Txt_GameHelpInfo"].GetComponent<Text>();
            txt_LastTime = views["Txt_TimeValue"].GetComponent<Text>();

            view_LastTime = views["LastTimeInfo"];
            btn_SkipEditorTime = views["Btn_SkipEditor"].GetComponent<Button>();

            txt_Angel_Items = views["Txt_Angel_Items"].GetComponent<Text>();
            txt_Demo_Items = views["Txt_Demo_Items"].GetComponent<Text>();

            view_HeaderInfo = views["PlayerHeaderInfo"];
        }
        protected override void Init_ComponentsValue()
        {
            btn_SkipEditorTime.onClick.AddListener(() => { CombatManager.Instance.ClearEditorTime(); });
            CloseLastTime();
            Close();
        }
        public void SetTimeValue(int time)
        {
            txt_LastTime.text = time.ToString();
        }
        public void SetHelpInfo(string message)
        {
            txt_GameHelpInfo.text = message;
        }
        public void ShowLastTime()
        {
            view_LastTime.SetActive(true);
            btn_SkipEditorTime.gameObject.SetActive(true);
        }
        public void CloseLastTime()
        {
            view_LastTime.SetActive(false);
            btn_SkipEditorTime.gameObject.SetActive(false);
        }

        public override void Show()
        {
            base.Show();
            SetHelpInfo("");
            view_HeaderInfo.SetActive(true);
        }
        public override void Close()
        {
            base.Close();
            SetHelpInfo("");
            view_HeaderInfo.SetActive(false);
        }

        public void SetCurrentItemInfo(PlayerType playerType, string message)
        {
            switch (playerType)
            {
                case PlayerType.Player_1: { txt_Demo_Items.text = message; } break;
                case PlayerType.Player_2: { txt_Angel_Items.text = message; } break;
            }
        }
    }
}