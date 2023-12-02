using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HRL
{
    public class UIViewInfo
    {
        public bool mIsFull = true;                 // �Ƿ�ȫ������ȫ��������һ����ɫ��͸������
        public bool mIsConst = false;               // �Ƿ��ڴ��ڣ����Ƴ����н���ʱ����
        public bool mIsUnique = true;               // �Ƿ��һ�޶�������ʱ��Ⲣ����ͬ������
        public bool mHideHudUI = true;              // ����������UI
        public bool mIsRenderWorld = false;         // �Ƿ���Ⱦ����
        public bool mRemoveOtherDialog = true;      // �Ƴ��������н���
        public bool mBtnRemove = true;              // ����ȫ������������ť�Ƴ�����
        public float mAlpha = 0.5f;                 // ����ȫ�������һ����ɫ��������͸����ֵ
        public int mPriority = 1;                   // ��ʾ���ȼ�
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
        // Init ֻ����Awake��ʱ��ִ��һ��
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