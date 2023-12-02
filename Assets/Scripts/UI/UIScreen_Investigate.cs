using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HRL;
using UnityEngine.Events;

public class UIScreen_Investigate : UIScreen
{
    public Text text;
    public string content;

    public Button btn_sure;
    public Text text_sure;
    public Button btn_cancel;
    public Text text_cancel;

    public string text_default_sure = "确认";

    protected override void Init()
    {
        base.Init();
    }

    public override void OnShown()
    {
        base.OnShown();
        //InputManager.Instance.canClick = false;
        content = (string)this.UIInfo.GetParam("content", typeof(string));
        text.text = content;
        string text_btn_sure = (string)this.UIInfo.GetParam("text_btn_sure", typeof(string));
        UnityAction action_sure = UIInfo.GetAction("on_click_sure");
        string text_btn_cancel = (string)this.UIInfo.GetParam("text_btn_cancel", typeof(string));
        UnityAction action_cancel = UIInfo.GetAction("on_click_cancel");

        if (string.IsNullOrEmpty(text_btn_sure) && string.IsNullOrEmpty(text_btn_cancel))
        {
            text_btn_sure = text_default_sure;
            action_sure = Remove;
        }


        if (!string.IsNullOrEmpty(text_btn_sure))
        {
            btn_sure.gameObject.SetActive(true);
            btn_sure.onClick.RemoveAllListeners();
            btn_sure.onClick.AddListener(action_sure);
            btn_sure.onClick.AddListener(OnClick_Sure);
            text_sure.text = text_btn_sure;
        }
        else
        {
            btn_sure.gameObject.SetActive(false);
        }
        print(text_btn_cancel);
        if (!string.IsNullOrEmpty(text_btn_cancel))
        {
            btn_cancel.gameObject.SetActive(true);
            btn_cancel.onClick.RemoveAllListeners();
            btn_cancel.onClick.AddListener(action_cancel);
            btn_cancel.onClick.AddListener(OnClick_Cancel);
            text_cancel.text = text_btn_cancel;
        }
        else
        {
            btn_cancel.gameObject.SetActive(false);
        }
    }

    public override void OnHide()
    {
        base.OnHide();
        //InputManager.Instance.canClick = true;
    }

    public override void OnClick_Mask()
    {
        // 此界面不对mask相应，因为可能涉及到传入的cancel没有调用等情况
    }

    private void OnClick_Sure()
    {
        Remove();
    }

    private void OnClick_Cancel()
    {
        Remove();
    }
}
