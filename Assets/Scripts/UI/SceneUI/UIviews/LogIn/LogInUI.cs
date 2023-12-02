using HRL;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.LogIn
{
    public class LogInUI : ParentView
    {
        public SubView_LogIn view_LogIn;
        public SubView_Explain view_Explain;
        protected override void Init_Components()
        {
            view_LogIn = views["SubView_LogIn"].AddComponent<SubView_LogIn>();
            view_Explain = views["SubView_Explain"].AddComponent<SubView_Explain>();
            view_LogIn.Init_SubView(this);
            view_Explain.Init_SubView(this);
        }
    }
}