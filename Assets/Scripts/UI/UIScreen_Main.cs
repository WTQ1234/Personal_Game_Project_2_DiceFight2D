using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HRL;
//using PixelCrushers.DialogueSystem;

public class UIScreen_Main : UIScreen
{
    [SerializeField] Button btn_Start;
    [SerializeField] Button btn_Exit;
    [SerializeField] Button btn_Language;
    [SerializeField] Button btn_Setting;

    protected override void Init()
    {
        base.Init();
        btn_Start.onClick.AddListener(OnClick_Start);
        btn_Exit.onClick.AddListener(OnClick_Exit);
        btn_Language.onClick.AddListener(OnClick_Language);
        btn_Setting.onClick.AddListener(() => UIManager.Instance.PushScreen<UIScreen_Setting>());
    }

    private void OnClick_Start()
    {
        // 对话测试
        print("111");
        //DialogueManager.StartConversation("示例对话1", null, null, -1);
        //DialogueManager.instance.conversationEnded += OnConversationEnd_Before;
        //UIManager.Instance.PushScreen<UIScreen_Chapter>();
    }

    private void OnConversationEnd_Before(Transform t)
    {
        print("llll");
        Remove();
    }

    private void OnClick_Language()
    {
        UIManager.Instance.PushScreen<UIScreen_Setting>();
        //UIManager.Instance.PushScreen<UIScreen_Language>();
    }

    private void OnClick_Exit()
    {
        Application.Quit();
    }
}
