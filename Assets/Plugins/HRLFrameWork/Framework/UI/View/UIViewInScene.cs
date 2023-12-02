using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HRL
{
    public class UIViewInScene : UIView
    {
        #region LifeTime
        protected override void Update()
        {
            base.Update();
            FollowObj();
        }
        #endregion

        #region Follow Object
        public GameObject Follower;
        public bool isFollowing;
        public Vector3 offset;
        public Vector3 worldPosition;

        protected virtual void FollowObj()
        {
            if (!isFollowing)
            {
                return;
            }
            if (Follower != null)
            {
                
                Vector2 mouseDown = Camera.main.WorldToScreenPoint(Follower.transform.position + offset);
                Vector2 mouseUGUIPos = new Vector2();
                bool isRect = RectTransformUtility.ScreenPointToLocalPointInRectangle(UIManager.Instance.canvas_scene.transform as RectTransform, mouseDown, Camera.main, out mouseUGUIPos);
                if (isRect)
                {
                    GetComponent<RectTransform>().anchoredPosition = mouseUGUIPos;
                }
            }
            else if (worldPosition != null && worldPosition != Vector3.zero)
            {
                Vector2 mouseDown = Camera.main.WorldToScreenPoint(worldPosition + offset);
                Vector2 mouseUGUIPos = new Vector2();
                bool isRect = RectTransformUtility.ScreenPointToLocalPointInRectangle(UIManager.Instance.canvas_scene.transform as RectTransform, mouseDown, Camera.main, out mouseUGUIPos);
                if (isRect)
                {
                    GetComponent<RectTransform>().anchoredPosition = mouseUGUIPos;
                }
            }

            //Vector3 screenPos = Camera.main.WorldToScreenPoint(Follower.transform.position);
            //transform.position = screenPos + offset;
        }
        #endregion
    }
}