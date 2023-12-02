#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using System.Linq;
using HRL;
using Sirenix.OdinInspector;

public class ItemEditorWindow : BasicConfigWindow
{
    private static string mFileName_Item = "Item[{0}]";
    private static string mAssetPath_Item = "Assets/Resources/ScriptableObject/ItemInfo";
    private static string mTitle_AllAssets_Item = "1.所有物品";

    [MenuItem("配置/主流程/物品")]
    private static void Open()
    {
        var window = GetWindow<ItemEditorWindow>();
        // 设置标题
        GUIContent titleContent = new GUIContent();
        titleContent.text = "物品配置";
        window.titleContent = titleContent;
        // 设置位置
        window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 500);
        // 添加基础配置
        if (!AssetPath.ContainsKey("属性路径"))
        {
            AssetPath.Add("默认数据名", mFileName_Item);
            AssetPath.Add("属性路径", mAssetPath_Item);
        }
    }

    protected override OdinMenuTree BuildMenuTree()
    {
        base.BuildMenuTree();
        // 浏览当前所有属性
        ItemOverview.Instance.UpdateOverview();
        // 将具体属性添加到列表
        if (ItemOverview.Instance.AllInfos.Length > 0)
        {
            mCurTree.Add(mTitle_AllAssets_Item, new BasicInfoTable<ItemInfo>(ItemOverview.Instance.AllInfos));
            mCurTree.AddAllAssetsAtPath(mTitle_AllAssets_Item, mAssetPath_Item, typeof(ItemInfo), true, true);
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
                int assetNumber = FindAssetNumber(mAssetPath_Item, mFileName_Item);
                Debug.Log(assetNumber);
                string curFileName = string.Format(mFileName_Item, assetNumber);
                ScriptableObjectCreator.ShowDialog<ItemInfo>(mAssetPath_Item, curFileName, (obj, fileName) =>
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
