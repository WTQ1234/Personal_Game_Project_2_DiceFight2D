using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HRL;
//using PixelCrushers.DialogueSystem;

public class Trigger_NPC : TriggerInteractiveable
{
    public string title = "����";

    protected override void Awake()
    {
        base.Awake();
        uiInfo = new UIInfo();
        uiInfo.RegisterParam("title", title);
    }

    public override void OnInteract()
    {
        base.OnInteract();
        //DialogueManager.StartConversation("ʾ���Ի�1", Player.Instance.transform, transform, -1);
    }

    // ���� DialogueSystemMessages �ķ�����Ϣ������ֱ������
    public void OnConversationEnd()
    {
        OnInteractEnd();
    }
}
