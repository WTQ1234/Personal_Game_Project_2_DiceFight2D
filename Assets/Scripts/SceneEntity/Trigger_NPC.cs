using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HRL;
//using PixelCrushers.DialogueSystem;

public class Trigger_NPC : TriggerInteractiveable
{
    public string title = "聆听";

    protected override void Awake()
    {
        base.Awake();
        uiInfo = new UIInfo();
        uiInfo.RegisterParam("title", title);
    }

    public override void OnInteract()
    {
        base.OnInteract();
        //DialogueManager.StartConversation("示例对话1", Player.Instance.transform, transform, -1);
    }

    // 来自 DialogueSystemMessages 的发送消息，并无直接引用
    public void OnConversationEnd()
    {
        OnInteractEnd();
    }
}
