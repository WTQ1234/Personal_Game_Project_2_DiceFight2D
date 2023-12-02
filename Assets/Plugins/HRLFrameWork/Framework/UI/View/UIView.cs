using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HRL
{
    public class UIViewInfo
    {
        public bool mIsFull = true;                 // 是否全屏，非全屏会生成一个黑色半透明背景
        public bool mIsConst = false;               // 是否长期存在，在移除所有界面时例外
        public bool mIsUnique = true;               // 是否独一无二，创建时检测并销毁同名界面
        public bool mHideHudUI = true;              // 隐藏主界面UI
        public bool mIsRenderWorld = false;         // 是否渲染场景
        public bool mRemoveOtherDialog = true;      // 移除其他所有界面
        public bool mBtnRemove = true;              // 若不全屏，按背景按钮移除界面
        public float mAlpha = 0.5f;                 // 若不全屏，添加一个黑色背景，其透明度值
        public int mPriority = 1;                   // 显示优先级
    }

    public class UIView : UIBasic
    {
        #region LifeTime
        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Remove()
        {
            base.Remove();
            Destroy(gameObject);
        }

        public override void OnShown()
        {
            base.OnShown();
        }

        public override void OnHide()
        {
            base.OnHide();
        }
        #endregion

        #region Init
        // Init 只会在Awake的时候执行一次
        protected override void Init()
        {
            base.Init();
        }
        #endregion

        #region Animation
        protected override void DoShowAnimation()
        {
            base.DoShowAnimation();
        }

        protected override void DoHideAnimation()
        {
            base.DoHideAnimation();
        }
        #endregion
    }
}