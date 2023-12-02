using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// �����
    /// </summary>
    public class SubView : ViewControl
    {
        protected ViewControl baseInterface;
        /// <summary>
        /// ��ʼ�������
        /// </summary>
        /// <param name="uiWindow">�����</param>
        public void Init_SubView(ViewControl uiWindow)
        {
            baseInterface = uiWindow;
            views = baseInterface.views;
            base.Init_View();
        }
    }
}