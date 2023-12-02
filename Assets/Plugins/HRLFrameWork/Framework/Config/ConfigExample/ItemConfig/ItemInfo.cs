using System.Linq;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.Utilities;
using Sirenix.OdinInspector;
using HRL;

[SerializeField]
public class ItemInfo : BasicInfo
{
    public string ItemName;

    public string ItemDesc;

    // 堆叠数量
    public int ItemNum;

    // 暂时使用向单例发送消息的形式触发回调，不用反射
    public string ItemHandler;

    public Sprite Icon;
}
