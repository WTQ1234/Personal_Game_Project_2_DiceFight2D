using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HRL;

public class Trigger_PickItem : TriggerInteractiveable
{
    public string title = "ʰȡ";

    public ItemInfo item;

    public int num;

    protected override void Awake()
    {
        base.Awake();
        uiInfo = new UIInfo();
        uiInfo.RegisterParam("title", title);
    }

    public override void OnInteract()
    {
        base.OnInteract();
        gameObject.SetActive(false);
        ItemManager.Instance.AddItem(item, num);
        // ���̽������������Կ�������ʰȡ����
        OnInteractEnd();
    }
}
