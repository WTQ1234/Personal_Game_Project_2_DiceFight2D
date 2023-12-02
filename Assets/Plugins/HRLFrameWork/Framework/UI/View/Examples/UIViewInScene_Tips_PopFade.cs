using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HRL
{
    public class UIViewInScene_Tips_PopFade : UIViewInScene
    {
        public Text text;
        public string content;

        public Animator animator;

        public override void OnShown()
        {
            base.OnShown();
            animator = GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger("UIShown");
            }
            content = (string)this.UIInfo.GetParam("content", typeof(string));
            text.text = content;
        }
    }
}
