using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// ¸¸Ãæ°å
    /// </summary>
    public class ParentView : ViewControl
    {
        public override void Awake()
        {
            base.Awake();
            Init_View();
        }
        protected override void Init_View()
        {
            views = new Dictionary<string, GameObject>();
            Init_UIViewNode(gameObject, "");
            base.Init_View();
        }
        private void Init_UIViewNode(GameObject rootNode, string path)
        {
            foreach (Transform currentNode in rootNode.transform)
            {
                if (views.ContainsKey(currentNode.name))
                    continue;
                views.Add(currentNode.name, currentNode.gameObject);
                Init_UIViewNode(currentNode.gameObject, path + currentNode.name + "/");
            }
        }
    }
}