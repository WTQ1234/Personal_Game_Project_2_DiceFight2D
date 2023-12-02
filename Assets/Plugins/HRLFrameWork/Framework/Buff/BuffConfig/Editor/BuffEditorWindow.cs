#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using System.Linq;
using HRL;
using Sirenix.OdinInspector;

namespace HRL
{
    public class BuffEditorWindow : BasicConfigWindow
    {
        private static string mFileName_Buff = "Buff[{0}]";
        private static string mAssetPath_Buff = "Assets/Resources/ScriptableObject/BuffInfo";
        private static string mTitle_AllAssets_Buff = "1.所有Buff";

        [MenuItem("配置/主流程/Buff")]
        private static void Open()
        {
            var window = GetWindow<BuffEditorWindow>();
            // 设置标题
            GUIContent titleContent = new GUIContent();
            titleContent.text = "Buff配置";
            window.titleContent = titleContent;
            // 设置位置
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 500);
            // 添加基础配置
            if (!AssetPath.ContainsKey("属性路径"))
            {
                AssetPath.Add("默认数据名", mFileName_Buff);
                AssetPath.Add("属性路径", mAssetPath_Buff);
            }
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            base.BuildMenuTree();
            // 浏览当前所有属性
            BuffOverview.Instance.UpdateOverview();
            // 将具体属性添加到列表
            if (BuffOverview.Instance.AllInfos.Length > 0)
            {
                mCurTree.Add(mTitle_AllAssets_Buff, new BasicInfoTable<BuffInfo>(BuffOverview.Instance.AllInfos));
                mCurTree.AddAllAssetsAtPath(mTitle_AllAssets_Buff, mAssetPath_Buff, typeof(BuffInfo), true, true);
            }
            // 后续处理
            AfterCreateBuildMenuTree();
            return mCurTree;
        }

        protected override void OnBeginDrawEditors()
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
                if (SirenixEditorGUI.ToolbarButton(new GUIContent("创建新语言配置")))
                {
                    int assetNumber = FindAssetNumber(mAssetPath_Buff, mFileName_Buff);
                    Debug.Log(assetNumber);
                    string curFileName = string.Format(mFileName_Buff, assetNumber);
                    ScriptableObjectCreator.ShowDialog<BuffInfo>(mAssetPath_Buff, curFileName, (obj, fileName) =>
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
    }
}
#endif