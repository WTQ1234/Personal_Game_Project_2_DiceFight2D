using System.Linq;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.Utilities;
using Sirenix.OdinInspector;

[GlobalConfig("Assets/Resources/ScriptableObject/EditorOverviews")]
public class WeaponOverview : GlobalConfig<WeaponOverview>
{
    [ReadOnly]
    [ListDrawerSettings(Expanded = true)]
    public WeaponInfo[] AllInfos;

    [Button(ButtonSizes.Medium), PropertyOrder(-1)]
    public void UpdateOverview()
    {
        // Finds and assigns all scriptable objects of type Character
        this.AllInfos = AssetDatabase.FindAssets("t:WeaponInfo")
            .Select(guid => AssetDatabase.LoadAssetAtPath<WeaponInfo>(AssetDatabase.GUIDToAssetPath(guid)))
            .ToArray();
    }
}
