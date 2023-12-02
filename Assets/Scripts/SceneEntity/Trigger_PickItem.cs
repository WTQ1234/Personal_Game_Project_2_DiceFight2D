using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HRL;

public class Trigger_PickItem : TriggerInteractiveable
{
    public string title = "拾取";

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
        // 立刻结束，后续可以考虑做个拾取动画
        OnInteractEnd();
    }
}
