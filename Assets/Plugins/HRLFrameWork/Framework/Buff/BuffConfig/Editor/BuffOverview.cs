using System.Linq;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.Utilities;
using Sirenix.OdinInspector;

namespace HRL
{
    [GlobalConfig("Assets/Resources/ScriptableObject/EditorOverviews")]
    public class BuffOverview : GlobalConfig<BuffOverview>
    {
        [ReadOnly]
        [ListDrawerSettings(Expanded = true)]
        public BuffInfo[] AllInfos;

        [Button(ButtonSizes.Medium), PropertyOrder(-1)]
        public void UpdateOverview()
        {
            // Finds and assigns all scriptable objects of type Character
            this.AllInfos = AssetDatabase.FindAssets("t:BuffInfo")
                .Select(guid => AssetDatabase.LoadAssetAtPath<BuffInfo>(AssetDatabase.GUIDToAssetPath(guid)))
                .ToArray();
        }
    }

}
