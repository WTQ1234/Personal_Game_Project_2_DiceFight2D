using Game.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// UI面板主视图管理
    /// </summary>
    public class ViewManager : MonoSingleton<ViewManager>
    {
        public GameObject canvas;
        private static Dictionary<UIViewType, ViewControl> viewControls;

        protected override void Awake()
        {
            base.Awake();
            canvas = gameObject;
            viewControls = new Dictionary<UIViewType, ViewControl>();
            Init_UIView();
        }

        private void Init_UIView()
        {
            viewControls.Add(UIViewType.LogIn, GetUIView("UIView_LogIn").AddComponent<LogIn.LogInUI>());
            viewControls.Add(UIViewType.Combat, GetUIView("UIView_Combat").AddComponent<Combat.CombatUI>());

            foreach (KeyValuePair<UIViewType, ViewControl> view in viewControls)
                view.Value.Close();
        }

        public static ViewControl GetView(UIViewType viewType) { return viewControls[viewType]; }

        private GameObject GetUIView(string viewName)
        {
            GameObject uiViewPrefab = Resources.Load<GameObject>($"Prefabs/UI/UIViews/{viewName}");
            GameObject view = Instantiate(uiViewPrefab, canvas.transform);
            view.name = uiViewPrefab.name;
            return view;
        }
    }
    public enum UIViewType
    {
        LogIn,
        Combat,
    }
}