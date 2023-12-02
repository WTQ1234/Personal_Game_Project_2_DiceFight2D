using System.Linq;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.Utilities;
using Sirenix.OdinInspector;

[GlobalConfig("Assets/Resources/ScriptableObject/EditorOverviews")]
public class ItemOverview : GlobalConfig<ItemOverview>
{
    [ReadOnly]
    [ListDrawerSettings(Expanded = true)]
    public ItemInfo[] AllInfos;

    [Button(ButtonSizes.Medium), PropertyOrder(-1)]
    public void UpdateOverview()
    {
        // Finds and assigns all scriptable objects of type Character
        this.AllInfos = AssetDatabase.FindAssets("t:ItemInfo")
            .Select(guid => AssetDatabase.LoadAssetAtPath<ItemInfo>(AssetDatabase.GUIDToAssetPath(guid)))
            .ToArray();
    }
}
