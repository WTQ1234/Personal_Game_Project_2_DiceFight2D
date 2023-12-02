using Game.UI;
using HRL;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏管理
/// </summary>
public class GameManager : MonoSingleton<GameManager>
{
    protected override void Awake()
    {
        base.Awake();
        Init_Framework();
        Init_GameLogic();
    }
    /// <summary>
    /// 载入游戏组件
    /// </summary>
    private void Init_Framework()
    {
        //GameObject.Find("Canvas").AddComponent<ViewManager>();
        //gameObject.AddComponent<CombatManager>();
        gameObject.AddComponent<Messenger>();
    }

    /// <summary>
    /// 载入游戏逻辑
    /// </summary>
    public void Init_GameLogic()
    {
        ViewManager.GetView(UIViewType.LogIn).Show();//显示
    }
}