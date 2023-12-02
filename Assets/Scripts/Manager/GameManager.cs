using Game.UI;
using HRL;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Ϸ����
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
    /// ������Ϸ���
    /// </summary>
    private void Init_Framework()
    {
        //GameObject.Find("Canvas").AddComponent<ViewManager>();
        //gameObject.AddComponent<CombatManager>();
        gameObject.AddComponent<Messenger>();
    }

    /// <summary>
    /// ������Ϸ�߼�
    /// </summary>
    public void Init_GameLogic()
    {
        ViewManager.GetView(UIViewType.LogIn).Show();//��ʾ
    }
}