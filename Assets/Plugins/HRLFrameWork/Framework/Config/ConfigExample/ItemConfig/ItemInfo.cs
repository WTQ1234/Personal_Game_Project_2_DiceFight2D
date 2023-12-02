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

    // �ѵ�����
    public int ItemNum;

    // ��ʱʹ������������Ϣ����ʽ�����ص������÷���
    public string ItemHandler;

    public Sprite Icon;
}
