using System.Linq;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.Utilities;
using Sirenix.OdinInspector;

[GlobalConfig("Assets/Resources/ScriptableObject/EditorOverviews")]
public class PickupOverview : GlobalConfig<PickupOverview>
{
    [ReadOnly]
    [ListDrawerSettings(Expanded = true)]
    public PickupInfo[] AllInfos;

    [Button(ButtonSizes.Medium), PropertyOrder(-1)]
    public void UpdateOverview()
    {
        // Finds and assigns all scriptable objects of type Character
        this.AllInfos = AssetDatabase.FindAssets("t:PickupInfo")
            .Select(guid => AssetDatabase.LoadAssetAtPath<PickupInfo>(AssetDatabase.GUIDToAssetPath(guid)))
            .ToArray();
    }
}
