using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Combat
{
    /// <summary>
    /// 最终获胜者结算界面
    /// </summary>
    public class SubView_EndWinner : SubView
    {
        private GameObject view_AngelWin;
        private GameObject view_DemoWin;

        protected override void Init_Components()
        {
            view_AngelWin = views["AngelWin"];
            view_DemoWin = views["DemoWin"];
        }

        protected override void Init_ComponentsValue()
        {
            Close();   
        }

        public void ShowAngelWin() { view_AngelWin.SetActive(true); }
        private void CloseAngelWin() { view_AngelWin.SetActive(false); }
        public void ShowDemoWin() { view_DemoWin.SetActive(true); }
        private void CloseDemoWin() { view_DemoWin.SetActive(false); }
        public override void Close()
        {
            base.Close();
            CloseAngelWin();
            CloseDemoWin();
        }
    }
}