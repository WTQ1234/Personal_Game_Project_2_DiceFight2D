using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HRL
{
    public class UIViewInScene_ShowInteract : UIViewInScene
    {
        public Text text;
        public string interact_title;

        public override void OnShown()
        {
            base.OnShown();
            interact_title = (string)this.UIInfo.GetParam("title", typeof(string));
            text.text = interact_title;
        }
    }
}
