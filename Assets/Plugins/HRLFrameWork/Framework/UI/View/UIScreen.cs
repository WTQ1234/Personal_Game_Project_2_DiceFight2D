using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace HRL
{
    // TODO: ��Ҫ�Խ�������һ�����ã��Ƿ������ƶ�
    [System.Serializable]
    public class UIScreenInfo
    {
        public bool mIsFull = true;                 // �Ƿ�ȫ������ȫ��������һ����ɫ��͸������
        public bool mIsConst = false;               // �Ƿ��ڴ��ڣ����Ƴ����н���ʱ����
        public bool mIsUnique = true;               // �Ƿ��һ�޶�������ʱ��Ⲣ����ͬ������
        public bool mHideHudUI = true;              // ����������UI
        public bool mIsRenderWorld = false;         // �Ƿ���Ⱦ����
        public bool mHideLastDialog = true;         // �Ƴ���һ������
        public bool mTopDialog = false;             // �ڶ���Canvas
        public bool mLockPlayerInput = true;
        //public bool mRemoveOtherDialog = true;      // �Ƴ��������н���
        [HideInInspector]
        public bool mBtnRemove = true;              // ����ȫ������������ť�Ƴ�����
        [HideInInspector]
        public float mAlpha = 0.5f;                 // ����ȫ�������һ����ɫ��������͸����ֵ
        [HideInInspector]
        public int mPriority = 1;                   // ��ʾ���ȼ�
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
        // Init ֻ����Awake��ʱ��ִ��һ��
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