using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HRL
{
    public class UIViewInScene_Tips_PopFade_2 : UIViewInScene
    {
        public Text text;
        public string content;

        public Animator animator;

        public Color color_white;
        public Color color_green;
        public Color color_gold;
        public Color color_red;

        public override void OnShown()
        {
            base.OnShown();
            animator = GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger("UIShown");
            }
            content = (string)this.UIInfo.GetParam("content", typeof(string));
            WeaponQuality quality = (WeaponQuality)this.UIInfo.GetParam("quality", typeof(WeaponQuality));
            switch (quality)
            {
                case WeaponQuality.white:
                    text.color = color_white;
                    break;
                case WeaponQuality.green:
                    text.color = color_green;
                    break;
                case WeaponQuality.gold:
                    text.color = color_gold;
                    break;
                case WeaponQuality.red:
                    text.color = color_red;
                    break;
            }
            text.text = content;
        }
    }
}
