using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace HRL
{
    // TODO: 需要对界面增加一个设置：是否锁定移动
    [System.Serializable]
    public class UIScreenInfo
    {
        public bool mIsFull = true;                 // 是否全屏，非全屏会生成一个黑色半透明背景
        public bool mIsConst = false;               // 是否长期存在，在移除所有界面时例外
        public bool mIsUnique = true;               // 是否独一无二，创建时检测并销毁同名界面
        public bool mHideHudUI = true;              // 隐藏主界面UI
        public bool mIsRenderWorld = false;         // 是否渲染场景
        public bool mHideLastDialog = true;         // 移除上一个界面
        public bool mTopDialog = false;             // 在顶端Canvas
        public bool mLockPlayerInput = true;
        //public bool mRemoveOtherDialog = true;      // 移除其他所有界面
        [HideInInspector]
        public bool mBtnRemove = true;              // 若不全屏，按背景按钮移除界面
        [HideInInspector]
        public float mAlpha = 0.5f;                 // 若不全屏，添加一个黑色背景，其透明度值
        [HideInInspector]
        public int mPriority = 1;                   // 显示优先级
    }

    public class UIScreen : UIBasic
    {
        [BoxGroup("Basic Screen Info")]
        [SerializeField] UIScreenInfo UIScreenInfo = new UIScreenInfo();

        [BoxGroup("Basic Comp Allow Null")]
        [SerializeField] UI_Comp_Mask comp_Mask;

        #region LifeTime
        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Remove()
        {
            base.Remove();
            UIManager.Instance.PopScreen(this);
        }

        public override void OnShown()
        {
            base.OnShown();
            if (UIScreenInfo.mLockPlayerInput)
            {
                InputManager.Instance.mLockPlayerInputNum++;
            }
        }

        public override void OnHide()
        {
            base.OnHide();
            if (UIScreenInfo.mLockPlayerInput)
            {
                InputManager.Instance.mLockPlayerInputNum--;
            }
        }
        #endregion

        #region Init
        // Init 只会在Awake的时候执行一次
        protected override void Init()
        {
            base.Init();
            InitUIScreenInfo();
            if (comp_Mask != null)
            {
                comp_Mask.Init(this);
            }
        }
        #endregion

        #region ScreenInfo
        protected virtual void InitUIScreenInfo()
        {
            //UIScreenInfo = new UIScreenInfo();
        }

        public virtual UIScreenInfo GetScreenInfo()
        {
            return UIScreenInfo;
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

        #region SubComp
        public virtual void OnClick_Mask()
        {
            Remove();
        }
        #endregion
    }
}