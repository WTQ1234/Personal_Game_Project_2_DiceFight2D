using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 子面板
    /// </summary>
    public class SubView : ViewControl
    {
        protected ViewControl baseInterface;
        /// <summary>
        /// 初始化子面板
        /// </summary>
        /// <param name="uiWindow">父面板</param>
        public void Init_SubView(ViewControl uiWindow)
        {
            baseInterface = uiWindow;
            views = baseInterface.views;
            base.Init_View();
        }
    }
}