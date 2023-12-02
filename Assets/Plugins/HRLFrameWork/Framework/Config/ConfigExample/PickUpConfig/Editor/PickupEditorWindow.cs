#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using System.Linq;
using HRL;
using Sirenix.OdinInspector;

public class PickupEditorWindow : BasicConfigWindow
{
    private static string mFileName_Pickup = "Pickup[{0}]";
    private static string mAssetPath_Pickup = "Assets/Resources/ScriptableObject/PickupInfo";
    private static string mTitle_AllAssets_Pickup = "1.所有拾取物";

    [MenuItem("配置/主流程/拾取物")]
    private static void Open()
    {
        var window = GetWindow<PickupEditorWindow>();
        // 设置标题
        GUIContent titleContent = new GUIContent();
        titleContent.text = "拾取物配置";
        window.titleContent = titleContent;
        // 设置位置
        window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 500);
        // 添加基础配置
        if (!AssetPath.ContainsKey("属性路径"))
        {
            AssetPath.Add("默认数据名", mFileName_Pickup);
            AssetPath.Add("属性路径", mAssetPath_Pickup);
        }
    }

    protected override OdinMenuTree BuildMenuTree()
    {
        base.BuildMenuTree();
        // 浏览当前所有属性
        PickupOverview.Instance.UpdateOverview();
        // 将具体属性添加到列表
        if (PickupOverview.Instance.AllInfos.Length > 0)
        {
            mCurTree.Add(mTitle_AllAssets_Pickup, new BasicInfoTable<PickupInfo>(PickupOverview.Instance.AllInfos));
            mCurTree.AddAllAssetsAtPath(mTitle_AllAssets_Pickup, mAssetPath_Pickup, typeof(PickupInfo), true, true);
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
            if (SirenixEditorGUI.ToolbarButton(new GUIContent("创建新物品配置")))
            {
                int assetNumber = FindAssetNumber(mAssetPath_Pickup, mFileName_Pickup);
                Debug.Log(assetNumber);
                string curFileName = string.Format(mFileName_Pickup, assetNumber);
                ScriptableObjectCreator.ShowDialog<PickupInfo>(mAssetPath_Pickup, curFileName, (obj, fileName) =>
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
#endif
