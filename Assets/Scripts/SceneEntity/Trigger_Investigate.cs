using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HRL;

public class Trigger_Investigate : TriggerInteractiveable
{
    public string title = "调查";

    public string content = "调查文本";

    public string text_btn_sure = "确认";
    public string text_btn_cancel = "取消";

    protected override void Awake()
    {
        base.Awake();
        uiInfo = new UIInfo();
        uiInfo.RegisterParam("title", title);
    }

    public override void OnInteract()
    {
        base.OnInteract();
        UIInfo screen_ui_info = new UIInfo();
        screen_ui_info.RegisterParam("content", content);
        
        if (text_btn_sure != "")
        {
            screen_ui_info.RegisterParam("text_btn_sure", text_btn_sure);
            screen_ui_info.RegisterAction("on_click_sure", OnClickSure);
        }
        if (text_btn_cancel != "")
        {
            screen_ui_info.RegisterParam("text_btn_cancel", text_btn_cancel);
            screen_ui_info.RegisterAction("on_click_cancel", OnClickCancel);
        }
        UIManager.Instance.PushScreen<UIScreen_Investigate>(screen_ui_info);
        //OnInteractEnd();
    }

    private void OnClickSure()
    {
        OnInteractEnd();
    }

    private void OnClickCancel()
    {
        OnInteractEnd();
    }
}
