#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using Sirenix.Utilities;
using Sirenix.Serialization;

namespace HRL
{
    // 基类，其具体实现可参考 AttrEditorWindow.cs
    public class BasicConfigWindow : OdinMenuEditorWindow
    {
        [InfoBox("此页数据只读 请在 BasicConfigWindow.cs 及其基类中修改代码")]
        [Title("路径相关配置")]
        [ShowInInspector, ReadOnly]
        protected static Dictionary<string, string> AssetPath = new Dictionary<string, string>() {
        {"整体配置路径", "Assets/Resources/ScriptableObject"}
    };

        protected OdinMenuTree mCurTree;

        protected override OdinMenuTree BuildMenuTree()
        {
            mCurTree = new OdinMenuTree(true);
            mCurTree.DefaultMenuStyle.IconSize = 28.00f;
            mCurTree.Config.DrawSearchToolbar = true;
            mCurTree.Add("0.基础配置", this);
            return mCurTree;
        }

        // 绘制目录后处理
        protected void AfterCreateBuildMenuTree()
        {
            foreach (OdinMenuItem root in mCurTree.MenuItems)
            {
                foreach (OdinMenuItem child in root.GetChildMenuItemsRecursive(true))
                {
                    if (child.Value != null)
                    {
                        if (child.Value.GetType().IsSubclassOf(typeof(BasicInfo)))
                        {
                            child.Name = child.Name + " " + child.Value.ToString();
                        }
                    }
                }
            }
            mCurTree.SortMenuItemsByName(false);
        }

        // 找到当前文件数目
        protected int FindAssetNumber(string mAssetPath, string mFileName)
        {
            int count = 0;
            int assetNumber = 0;
            while (true)
            {
                count++;
                // 如果asset存在，就跳到下一个，直到有某个数字编号的文件不存在
                if (File.Exists($"{mAssetPath}/{string.Format(mFileName, assetNumber)}.asset"))
                {
                    assetNumber++;
                }
                else
                {
                    return assetNumber;
                }
                if (count > 1000)
                {
                    break;
                }
            }
            return assetNumber;
        }

        // 选中当前文件
        protected void SelectCurAssetFile()
        {
            if (mCurTree != null)
            {
                if (mCurTree.Selection != null)
                {
                    OdinMenuItem firstSelection = mCurTree.Selection.FirstOrDefault();
                    if (firstSelection != null)
                    {
                        object obj = firstSelection.Value;
                        if (obj != null)
                        {
                            EditorGUIUtility.PingObject((UnityEngine.Object)obj);
                        }
                    }
                }
            }
        }

        // 绘制工具栏
        protected void DrawAssetToolbar<T>(string mAssetPath, string mFileName) where T : BasicInfo
        {
            if (this.MenuTree == null)
            {
                return;
            }
            var selected = this.MenuTree?.Selection?.FirstOrDefault();
            var toolbarHeight = this.MenuTree.Config.SearchToolbarHeight;
            // 绘制工具栏
            SirenixEditorGUI.BeginHorizontalToolbar(toolbarHeight);
            {
                if (selected != null)
                {
                    GUILayout.Label(selected.Name);
                }
                if (SirenixEditorGUI.ToolbarButton(new GUIContent("选中当前文件")))
                {
                    SelectCurAssetFile();
                }
                if (SirenixEditorGUI.ToolbarButton(new GUIContent("创建新文件")))
                {
                    int assetNumber = FindAssetNumber(mAssetPath, mFileName);
                    string curFileName = string.Format(mFileName, assetNumber);
                    ScriptableObjectCreator.ShowDialog<T>(mAssetPath, curFileName, (obj, fileName) =>
                    {
                        obj.Id = assetNumber;
                        obj.Name = obj.name;
                        obj.FileName = fileName;
                        obj.InitAfterCreateFile();
                        base.TrySelectMenuItemWithObject(obj);
                    });
                }
            }
            SirenixEditorGUI.EndHorizontalToolbar();
        }

        #region 示例中的拖拽，暂时没用
        private void AddDragHandles(OdinMenuItem menuItem)
        {
            menuItem.OnDrawItem += x => DragAndDropUtilities.DragZone(menuItem.Rect, menuItem.Value, false, false);
        }
        #endregion
    }
}
#endif