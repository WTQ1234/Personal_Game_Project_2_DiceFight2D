using Game.Core.Traps;
using Game.UI;
using Game.UI.Combat;
using HRL;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void CombatOver(int winner);
public delegate void StartFight();

/// <summary>
/// 战斗管理
/// </summary>
public class CombatManager : MonoSingleton<CombatManager>
{
    /// <summary>
    /// 最大回合数
    /// </summary>
    public readonly int MAXROUND = 3;
    /// <summary>
    /// 当前回合
    /// </summary>
    public int current_Round = 0;
    public int player1_Score;
    public int player2_Score;
    public CombatOver combatOver;
    public StartFight startFight;
    //private GameObject currentLevel;

    public CombatUI combatUI;
    public GameObject player1ItemData;
    public GameObject player2ItemData;
    public Player player1;
    public Player player2;
    public GameObject ClockParent;
    public Vector3 init_pos_1;
    public Vector3 init_pos_2;

    public bool isEnd;
    public float EDITOR_TIME;
    public float currentEditorTime;
    public FightState fightState;

    public bool isFirstShowRule;

    public List<TrapsItem> placeItems;

    protected override void Awake()
    {
        base.Awake();
        Init_Combat();
    }

    public float MaxWaitTime = 25;
    public float MaxFightTime = 120;
    public float currentWaitTime = 0;
    public float currentFightTime = 0;

    public List<GameObject> Tou_1 = new List<GameObject>();
    public List<GameObject> Tou_2 = new List<GameObject>();

    private void Update()
    {
        if (fightState == FightState.Fighting)
        {
            currentWaitTime += Time.deltaTime;
            currentFightTime += Time.deltaTime;

            if (currentWaitTime > MaxWaitTime)
            {
                currentWaitTime = 0;
                _OnRefreshPlayerWeapon();
            }
            if (currentFightTime > MaxFightTime)
            {
                if (player1.playerHealth.health > player2.playerHealth.health)
                {
                    CombatOver(player1.teamController.teamId);
                }
                else if (player1.playerHealth.health < player2.playerHealth.health)
                {
                    CombatOver(player2.teamController.teamId);
                }
            }
        }
    }

    public float each_damage_time;
    public void OnDamageClock()
    {
        currentWaitTime += each_damage_time;
    }

    /// <summary>
    /// 初始化战斗管理
    /// </summary>
    private void Init_Combat()
    {
        OnRoundInit();

        combatOver = new CombatOver(CombatOver);
        Messenger.Instance.AddListener("CombatOver", combatOver);

        startFight = new StartFight(Fight);
        Messenger.Instance.AddListener("StartFight", startFight);

        // 创建一个场景
        //GameObject levelPrefab = Resources.Load<GameObject>("Prefabs/Level/Env");
        //currentLevel = Instantiate(levelPrefab);
        //currentLevel.SetActive(false);
        init_pos_1 = player1.transform.position;
        init_pos_2 = player2.transform.position;
        //获取玩家GameObject
        //player1 = player1Obj.GetComponent<Player>();
        //player2 = player2Obj.GetComponent<Player>();

        EDITOR_TIME = 20f;
        currentEditorTime = 0f;
        isEnd = false;
        isFirstShowRule = true;
        fightState = FightState.UnFight;

        placeItems = new List<TrapsItem>();

        combatUI = (CombatUI)ViewManager.GetView(UIViewType.Combat);
    }
    /// <summary>
    /// 开启战斗
    /// </summary>
    private void Fight()
    {
        StartCoroutine(Fig());
    }
    private void OnRoundInit()
    {
        Debug.Log("OnRoundInit");
        // 锁定输入
        InputManager.Instance.mLockPlayerInputNum = 1;
        // 隐藏血条
        player1.playerHealth.comp_HealthBar.gameObject.SetActive(false);
        player2.playerHealth.comp_HealthBar.gameObject.SetActive(false);
        // 清理掉落物
        PickupItemManager.Instance.ClearPickUpItem();
        PickupItemManager.Instance.enabled = false;

        //
        ClockParent.gameObject.SetActive(false);
    }

    /// <summary>
    /// 当回合是编辑器时
    /// </summary>
    private void OnRoundIsEditor()
    {
        Debug.Log("OnRoundIsEditor");

        currentWaitTime = 0;
        currentFightTime = 0;
        fightState = FightState.ReadyEnterEditor;
        if (isFirstShowRule)//如果是第一次进入游戏, 由关闭游戏说明后, 显示编辑器UI
        {
            combatUI.view_ESC.ShowGameRule();
        }
        else
            ShowEditorUI();//否则常规显示

        LockPlaceItem();//编辑器模式下, 对已经编辑的道具进行锁定

        // 复活
        player1.playerHealth.RebornPlayer();
        player2.playerHealth.RebornPlayer();
        // 锁定血量
        player1.playerHealth.avoidHit = true;
        player2.playerHealth.avoidHit = true;
        // 隐藏血条
        player1.playerHealth.comp_HealthBar.gameObject.SetActive(false);
        player2.playerHealth.comp_HealthBar.gameObject.SetActive(false);
        // 清理掉落物
        PickupItemManager.Instance.ClearPickUpItem();
        PickupItemManager.Instance.enabled = false;
        // 重置武器
        player1.ClearWeapon();
        player2.ClearWeapon();
        //
        ClockParent.gameObject.SetActive(true);

        // 锁定输入
        InputManager.Instance.mLockPlayerInputNum = 0;

        // 设置位置
        player1.transform.position = init_pos_1;
        player2.transform.position = init_pos_2;

        GameObject itemDataPrefab = Resources.Load<GameObject>("Prefabs/Items/ItemData");
        player1ItemData = Instantiate(itemDataPrefab, player1.transform);
        player1ItemData.GetComponent<TrapItemEditor>().OpenPlayerInput(KeyCode.DownArrow, KeyCode.KeypadEnter);
        player2ItemData = Instantiate(itemDataPrefab, player2.transform);
        player2ItemData.GetComponent<TrapItemEditor>().OpenPlayerInput(KeyCode.S, KeyCode.G);
    }
    /// <summary>
    /// 当回合是战斗开始时
    /// </summary>
    private void OnRoundIsCombatStart()
    {
        currentWaitTime = 0;
        currentFightTime = 0;

        Debug.Log("OnRoundIsCombatStart");
        fightState = FightState.Fighting;//编辑器模式结束, 开启战斗
        UnLockPlaceItem();//战斗模式下解除对已编辑物品的锁定

        // 显示血条
        player1.playerHealth.comp_HealthBar.gameObject.SetActive(true);
        player2.playerHealth.comp_HealthBar.gameObject.SetActive(true);
        // 复活
        player1.playerHealth.RebornPlayer();
        player2.playerHealth.RebornPlayer();
        // 锁定血量
        player1.playerHealth.avoidHit = false;
        player2.playerHealth.avoidHit = false;
        // 清理掉落物
        PickupItemManager.Instance.ClearPickUpItem();
        PickupItemManager.Instance.enabled = true;
        // 重置武器
        player1.ClearWeapon();
        player2.ClearWeapon();

        // 重新随机武器
        _OnRefreshPlayerWeapon();
        //
        ClockParent.gameObject.SetActive(true);

        // 设置位置
        player1.transform.position = init_pos_1;
        player2.transform.position = init_pos_2;
        // 锁定攻击
        player1.playerController.LockAttack = false;
        player2.playerController.LockAttack = false;
        combatUI.view_Combating.Show();
    }
    /// <summary>
    /// 当回合是战斗结束时
    /// </summary>
    private void OnRoundIsCombatOver()
    {
        currentWaitTime = 0;
        currentFightTime = 0;

        Debug.Log("OnRoundIsCombatOver");
        // 锁定输入
        InputManager.Instance.mLockPlayerInputNum = 1;
        // 锁定攻击
        player1.playerController.LockAttack = true;
        player1.playerController.Attack_Release();
        player1.playerController.Attack_Release_Far();
        player2.playerController.LockAttack = true;
        player2.playerController.Attack_Release();
        player2.playerController.Attack_Release_Far();
        // 锁定血量
        player1.playerHealth.avoidHit = true;
        player2.playerHealth.avoidHit = true;
        // 显示血条
        player1.playerHealth.comp_HealthBar.gameObject.SetActive(true);
        player2.playerHealth.comp_HealthBar.gameObject.SetActive(true);
        // 清理掉落物
        PickupItemManager.Instance.ClearPickUpItem();
        PickupItemManager.Instance.enabled = false;

        //
        ClockParent.gameObject.SetActive(false);
    }
    /// <summary>
    /// 当游戏结束时
    /// </summary>
    private void OnEndGameWinner()
    {
        // 设置位置
        player1.transform.position = init_pos_1;
        player2.transform.position = init_pos_2;

        // 清理掉落物
        PickupItemManager.Instance.ClearPickUpItem();
        PickupItemManager.Instance.enabled = true;
        // 复活
        player1.playerHealth.RebornPlayer();
        player2.playerHealth.RebornPlayer();
        // 锁定血量
        player1.playerHealth.avoidHit = false;
        player2.playerHealth.avoidHit = false;
        // 重置武器
        player1.ClearWeapon();
        player2.ClearWeapon();

        Destroy(player1ItemData);
        Destroy(player2ItemData);
    }

    private void _OnRefreshPlayerWeapon()
    {
        // 重新随机武器
        int num_1 = UnityEngine.Random.Range(1, 7);
        int num_2 = UnityEngine.Random.Range(1, 7);
        player1.SetWeaponNum(num_1);
        player2.SetWeaponNum(num_2);

        for(int i = 0; i < Tou_1.Count; i++)
        {
            // 调整了位置，所以要反过来
            if ((i + 1) == num_1)
            {
                Tou_2[i].SetActive(true);
            }
            else
            {
                Tou_2[i].SetActive(false);
            }
            if ((i + 1) == num_2)
            {
                Tou_1[i].SetActive(true);
            }
            else
            {
                Tou_1[i].SetActive(false);
            }
        }
    }


    IEnumerator Fig()
    {
        LoadingFight();
        yield return InEditor();
        OnRoundIsCombatStart();
        yield return new WaitUntil(() => fightState == FightState.FightOver);
    }
    #region LoadCombat
    private void LoadingFight()
    {
        //combatUI.txt_TestScore.text = $"天使:{player1_Score}分[{player1.playerHealth.health}血] : 恶魔:{player2_Score}分[{player2.playerHealth.health}血]";
        current_Round++;//轮次递增 1 -> 2 -> 3
        Load_Env();
        Load_Player();
        Load_UI();
    }
    private void Load_Env()
    {
        //currentLevel.SetActive(true);
    }
    private void Load_Player()
    {
        //player1Obj.SetActive(true);
        //player2Obj.SetActive(true);
    }
    private void Load_UI()
    {
        ViewManager.GetView(UIViewType.LogIn).Close();
        combatUI.view_Combating.Close();
        combatUI.Show();
    }
    #endregion
    IEnumerator InEditor()
    {
        OnRoundIsEditor();
        yield return new WaitUntil(() => fightState == FightState.InEditor);
        combatUI.view_Editor.SetHelpInfo("Editing phase, Place traps.");
        yield return new WaitForSeconds(1.5f);
        combatUI.view_Editor.SetHelpInfo("");
        combatUI.view_Editor.ShowLastTime();

        currentEditorTime = EDITOR_TIME;
        while (currentEditorTime > 0f) 
        {
            currentEditorTime -= Time.deltaTime;
            combatUI.view_Editor.SetTimeValue((int)currentEditorTime);
            yield return null;
        }
        combatUI.view_Editor.CloseLastTime();
        combatUI.view_Editor.SetHelpInfo("Edit end, Start Fight!");
        yield return new WaitForSeconds(1.5f);
        combatUI.view_Editor.Close();
    }
    public void ShowEditorUI()
    {
        combatUI.view_Editor.Show();
        fightState = FightState.InEditor;
    }
    /// <summary>
    /// 清除编辑器时间
    /// </summary>
    public void ClearEditorTime()
    {
        currentEditorTime = 0f;
    }
    #region CombatOver
    /// <summary>
    /// 当前回合战斗结束
    /// </summary>
    /// <param name="currentRoundWinner">当前回合获胜者</param>
    public void CombatOver(int currentRoundWinner)
    {
        if (fightState != FightState.Fighting)
            return;
        PlayerType _winner = (PlayerType)currentRoundWinner;
        fightState = FightState.FightOver;
        StartCoroutine(Over(_winner));
    }
    private IEnumerator Over(PlayerType currentRoundWinner)
    {
        OnRoundIsCombatOver();

        UpdeatePlayerScore(currentRoundWinner);
        Debug.Log($"当前回合: {current_Round}, 获胜者: {currentRoundWinner}");

        //结束情况1: 有一方玩家已经获得了2分, 不需要进行第三轮
        if (player1_Score == 2 || player2_Score == 2)
        {
            ShowEndWinnerUI(CheckEndingWinner());
            combatUI.view_EndWinner.Show();
            yield return new WaitForSeconds(3f);
            combatUI.view_EndWinner.Close();
            ViewManager.GetView(UIViewType.LogIn).Show();
            combatUI.Close();
            EndGame();
            yield break;
        }

        //结束情况2: 已经结束了最后一轮
        if (current_Round == MAXROUND)
        {
            ShowEndWinnerUI(CheckEndingWinner());

            combatUI.view_EndWinner.Show();
            yield return new WaitForSeconds(3f);
            combatUI.view_EndWinner.Close();
            ViewManager.GetView(UIViewType.LogIn).Show();
            combatUI.Close();
            EndGame();
            yield break;
        }
        //显示正常轮次结束后的结算UI, 如果在此之前游戏结束 不会执行到此处
        combatUI.view_CombatOver.Show();
        yield return new WaitForSeconds(3f);
        combatUI.view_CombatOver.Close();
        Fight();
    }
    /// <summary>
    /// 检查最终获胜玩家
    /// </summary>
    /// <returns>获胜玩家</returns>
    private PlayerType CheckEndingWinner()
    {
        if (player1_Score > player2_Score)
            return PlayerType.Player_1;
        else
            return PlayerType.Player_2;
    }
    private void ShowEndWinnerUI(PlayerType endWinner)
    {
        Debug.Log($"游戏结束, 最终获胜者: {endWinner}, 玩家1得分:{player1_Score}, 玩家2得分: {player2_Score}");
        //执行后续操作 特殊的UI显示或......
        combatUI.view_CombatOver.Close();
        combatUI.view_EndWinner.Show();
        switch (endWinner)
        {
            case PlayerType.Player_1: { combatUI.view_EndWinner.ShowAngelWin(); } break;
            case PlayerType.Player_2: { combatUI.view_EndWinner.ShowDemoWin(); } break;
        }
    }
    /// <summary>
    /// 更新玩家分数
    /// </summary>
    /// <param name="winner">获胜者</param>
    private void UpdeatePlayerScore(PlayerType winner)
    {
        switch (winner)
        {
            case PlayerType.Player_1: player1_Score++; break;
            case PlayerType.Player_2: player2_Score++; break;
        }
        combatUI.view_CombatOver.SetScore(player1_Score, player2_Score);
    }
    /// <summary>
    /// 重置战斗
    /// </summary>
    private void ResetCombat()
    {
        fightState = FightState.UnFight;
        player1_Score = 0;
        player2_Score = 0;
        current_Round = 0;
        currentEditorTime = 0;
    }
    /// <summary>
    /// 结束游戏
    /// </summary>
    private void EndGame()
    {
        OnEndGameWinner();
        ResetCombat();
        StartCoroutine(ClearPlaceItems());
    }
    #endregion

    /// <summary>
    ///添加编辑完成的物品
    /// </summary>
    /// <param name="item"></param>
    public void AddTrapItem(TrapsItem item)
    {
        placeItems.Add(item);
    }
    /// <summary>
    /// 清空编辑物品
    /// </summary>
    private IEnumerator ClearPlaceItems()
    {
        int count = placeItems.Count;
        for (int i = 0; i < count;)
        {
            TrapsItem item = placeItems[i];
            if (item != null)
                Destroy(item.gameObject);
            placeItems.Remove(item);
            count = placeItems.Count;
            yield return null;
        }
        placeItems.Clear();

        yield return new WaitUntil(() => placeItems.Count == 0);
        StopAllCoroutines();
    }

    private void LockPlaceItem()
    {
        for (int i = 0; i < placeItems.Count; i++)
            placeItems[i].PlaceLock();
    }
    private void UnLockPlaceItem()
    {
        for (int i = 0; i < placeItems.Count; i++)
            placeItems[i].UnLock();
    }
}

public enum PlayerType
{
    Player_1 = 1,
    Player_2 = 2,
}

public enum FightState
{
    /// <summary>
    /// 未在战斗中
    /// </summary>
    UnFight,
    ReadyEnterEditor,
    /// <summary>
    /// 编辑器模式
    /// </summary>
    InEditor,
    /// <summary>
    /// 战斗中
    /// </summary>
    Fighting,
    /// <summary>
    /// 战斗结束
    /// </summary>
    FightOver,
}