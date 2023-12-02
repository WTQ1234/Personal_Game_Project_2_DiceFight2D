#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using System.Linq;
using HRL;
using Sirenix.OdinInspector;

public class WeaponEditorWindow : BasicConfigWindow
{
    private static string mFileName_Weapon = "Weapon[{0}]";
    private static string mAssetPath_Weapon = "Assets/Resources/ScriptableObject/WeaponInfo";
    private static string mTitle_AllAssets_Weapon = "1.所有武器";

    [MenuItem("配置/主流程/武器")]
    private static void Open()
    {
        var window = GetWindow<WeaponEditorWindow>();
        // 设置标题
        GUIContent titleContent = new GUIContent();
        titleContent.text = "武器配置";
        window.titleContent = titleContent;
        // 设置位置
        window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 500);
        // 添加基础配置
        if (!AssetPath.ContainsKey("属性路径"))
        {
            AssetPath.Add("默认数据名", mFileName_Weapon);
            AssetPath.Add("属性路径", mAssetPath_Weapon);
        }
    }

    protected override OdinMenuTree BuildMenuTree()
    {
        base.BuildMenuTree();
        // 浏览当前所有属性
        WeaponOverview.Instance.UpdateOverview();
        // 将具体属性添加到列表
        if (WeaponOverview.Instance.AllInfos.Length > 0)
        {
            mCurTree.Add(mTitle_AllAssets_Weapon, new BasicInfoTable<WeaponInfo>(WeaponOverview.Instance.AllInfos));
            mCurTree.AddAllAssetsAtPath(mTitle_AllAssets_Weapon, mAssetPath_Weapon, typeof(WeaponInfo), true, true);
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
                int assetNumber = FindAssetNumber(mAssetPath_Weapon, mFileName_Weapon);
                Debug.Log(assetNumber);
                string curFileName = string.Format(mFileName_Weapon, assetNumber);
                ScriptableObjectCreator.ShowDialog<WeaponInfo>(mAssetPath_Weapon, curFileName, (obj, fileName) =>
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
