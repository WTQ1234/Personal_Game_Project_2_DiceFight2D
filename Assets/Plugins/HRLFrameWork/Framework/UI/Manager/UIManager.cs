using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

namespace HRL
{
    public class UIManager : MonoSingleton<UIManager>
    {
        public Canvas canvas_scene;
        public Canvas canvas_hud;
        public Canvas canvas_screen;
        public Canvas canvas_top;
        public List<UIScreen> UIScreens = new List<UIScreen>();
        public Dictionary<string, UIScreen> Dict_UIScreens = new Dictionary<string, UIScreen>();

        #region Push
        public T PushScreen<T>(UIInfo uiInfo = null) where T : UIScreen
        {
            string uiName = typeof(T).Name;
            T screen = null;
            if (Dict_UIScreens.ContainsKey(uiName))
            {
                _PushScreen(Dict_UIScreens[uiName]);
                screen = (T)Dict_UIScreens[uiName];
            }
            else
            {
                UIScreen prefab = Resources.Load<UIScreen>($"Prefabs/UI/{uiName}");
                if (prefab != null)
                {
                    UIScreen obj = Instantiate(prefab, prefab.GetScreenInfo().mTopDialog ? canvas_top.transform : canvas_screen.transform);
                    obj.OnSetUIInfo(uiInfo);
                    _PushScreen(obj);
                    screen = (T)obj;
                }
            }
            Debug.Log($"Push Screen {screen}");
            return screen;
        }

        public void PushScreen(UIScreen uiScreen)
        {
            _PushScreen(uiScreen);
        }

        private void _PushScreen(UIScreen uiScreen)
        {
            string uiName = uiScreen.UIName;
            if (!Dict_UIScreens.ContainsKey(uiName))
            {
                Dict_UIScreens.Add(uiName, uiScreen);
            }
            else
            {
                Dict_UIScreens[uiName] = uiScreen;
            }

            // 根据 screeninfo 判定是否要隐藏上一个界面
            if (uiScreen.GetScreenInfo().mHideLastDialog)
            {
                if (UIScreens.Count > 0)
                {
                    UIScreens[UIScreens.Count - 1].OnHide();
                }
            }
            UIScreens.Add(uiScreen);
            uiScreen.OnShown();
        }
        #endregion

        #region Pop
        // 暂时不做销毁，UI界面不多，直接隐藏
        public void PopScreen<T>() where T : UIScreen
        {
            string uiName = typeof(T).Name;
            if (Dict_UIScreens.TryGetValue(uiName, out UIScreen uiScreen))
            {
                if (!UIScreens.Contains(uiScreen))
                {
                    return;
                }
                UIScreen screen = Dict_UIScreens[uiName];
                Debug.Log($"Pop Screen {screen}");
                screen.OnHide();
                UIScreens.Remove(screen);
                if (UIScreens.Count > 0)
                {
                    UIScreens[UIScreens.Count - 1].OnShown();
                }
            }
        }

        public void PopScreen(UIScreen uiScreen)
        {
            if (!UIScreens.Contains(uiScreen))
            {
                Debug.LogWarning("UIScreen is not in list");
                return;
            }
            uiScreen.OnHide();
            UIScreens.Remove(uiScreen);
            if (UIScreens.Count > 0)
            {
                UIScreens[UIScreens.Count - 1].OnShown();
            }
        }

        // 清除所有，用于返回主界面
        public void PopAllScreen()
        {
            foreach (var screen in UIScreens)
            {
                Debug.Log($"Pop Screen {screen}");
                screen.OnHide();
            }
            UIScreens.Clear();
        }
        #endregion

        public UIScreen GetTopScreen()
        {
            if (UIScreens.Count > 0)
            {
                return UIScreens[UIScreens.Count - 1];
            }
            return null;
        }

        public T GetScreen<T>() where T : UIScreen
        {
            string uiName = typeof(T).Name;
            Debug.Log(uiName);
            foreach (UIScreen screen in UIScreens)
            {
                Debug.Log(screen.UIName);
                if (screen.UIName == uiName)
                {
                    return (T)screen;
                }
            }
            return null;
        }

        /// <summary>
        /// 创建Following的UI
        /// </summary>
        /// <param name="prefab"></param>
        /// <param name="follower"></param>
        /// <param name="show"> 传递true时会自动初始化 </param>
        /// <returns></returns>
        /// 
        #region UIViewInScene
        public UIViewInScene CreateFollowingUI(UIViewInScene prefab, GameObject follower, bool show = true, UIInfo uiInfo = null)
        {
            UIViewInScene cur_view = Instantiate(prefab, canvas_scene.transform);
            cur_view.Follower = follower;
            cur_view.isFollowing = true;
            cur_view.OnSetUIInfo(uiInfo);
            if (show)
            {
                cur_view.OnShown();
            }
            else
            {
                cur_view.OnHide();
            }
            return cur_view;
        }

        public UIViewInScene CreateSceneUI(UIViewInScene prefab, Vector3 position, bool show = true, UIInfo uiInfo = null)
        {
            UIViewInScene cur_view = Instantiate(prefab, canvas_scene.transform);
            cur_view.worldPosition = position;
            cur_view.isFollowing = true;
            cur_view.OnSetUIInfo(uiInfo);
            if (show)
            {
                cur_view.OnShown();
            }
            else
            {
                cur_view.OnHide();
            }
            return cur_view;
        }
        #endregion

        public UIView CreateUIView(UIView prefab)
        {
            return null;
        }
    }
}