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
/// ս������
/// </summary>
public class CombatManager : MonoSingleton<CombatManager>
{
    /// <summary>
    /// ���غ���
    /// </summary>
    public readonly int MAXROUND = 3;
    /// <summary>
    /// ��ǰ�غ�
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
    /// ��ʼ��ս������
    /// </summary>
    private void Init_Combat()
    {
        OnRoundInit();

        combatOver = new CombatOver(CombatOver);
        Messenger.Instance.AddListener("CombatOver", combatOver);

        startFight = new StartFight(Fight);
        Messenger.Instance.AddListener("StartFight", startFight);

        // ����һ������
        //GameObject levelPrefab = Resources.Load<GameObject>("Prefabs/Level/Env");
        //currentLevel = Instantiate(levelPrefab);
        //currentLevel.SetActive(false);
        init_pos_1 = player1.transform.position;
        init_pos_2 = player2.transform.position;
        //��ȡ���GameObject
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
    /// ����ս��
    /// </summary>
    private void Fight()
    {
        StartCoroutine(Fig());
    }
    private void OnRoundInit()
    {
        Debug.Log("OnRoundInit");
        // ��������
        InputManager.Instance.mLockPlayerInputNum = 1;
        // ����Ѫ��
        player1.playerHealth.comp_HealthBar.gameObject.SetActive(false);
        player2.playerHealth.comp_HealthBar.gameObject.SetActive(false);
        // ���������
        PickupItemManager.Instance.ClearPickUpItem();
        PickupItemManager.Instance.enabled = false;

        //
        ClockParent.gameObject.SetActive(false);
    }

    /// <summary>
    /// ���غ��Ǳ༭��ʱ
    /// </summary>
    private void OnRoundIsEditor()
    {
        Debug.Log("OnRoundIsEditor");

        currentWaitTime = 0;
        currentFightTime = 0;
        fightState = FightState.ReadyEnterEditor;
        if (isFirstShowRule)//����ǵ�һ�ν�����Ϸ, �ɹر���Ϸ˵����, ��ʾ�༭��UI
        {
            combatUI.view_ESC.ShowGameRule();
        }
        else
            ShowEditorUI();//���򳣹���ʾ

        LockPlaceItem();//�༭��ģʽ��, ���Ѿ��༭�ĵ��߽�������

        // ����
        player1.playerHealth.RebornPlayer();
        player2.playerHealth.RebornPlayer();
        // ����Ѫ��
        player1.playerHealth.avoidHit = true;
        player2.playerHealth.avoidHit = true;
        // ����Ѫ��
        player1.playerHealth.comp_HealthBar.gameObject.SetActive(false);
        player2.playerHealth.comp_HealthBar.gameObject.SetActive(false);
        // ���������
        PickupItemManager.Instance.ClearPickUpItem();
        PickupItemManager.Instance.enabled = false;
        // ��������
        player1.ClearWeapon();
        player2.ClearWeapon();
        //
        ClockParent.gameObject.SetActive(true);

        // ��������
        InputManager.Instance.mLockPlayerInputNum = 0;

        // ����λ��
        player1.transform.position = init_pos_1;
        player2.transform.position = init_pos_2;

        GameObject itemDataPrefab = Resources.Load<GameObject>("Prefabs/Items/ItemData");
        player1ItemData = Instantiate(itemDataPrefab, player1.transform);
        player1ItemData.GetComponent<TrapItemEditor>().OpenPlayerInput(KeyCode.DownArrow, KeyCode.KeypadEnter);
        player2ItemData = Instantiate(itemDataPrefab, player2.transform);
        player2ItemData.GetComponent<TrapItemEditor>().OpenPlayerInput(KeyCode.S, KeyCode.G);
    }
    /// <summary>
    /// ���غ���ս����ʼʱ
    /// </summary>
    private void OnRoundIsCombatStart()
    {
        currentWaitTime = 0;
        currentFightTime = 0;

        Debug.Log("OnRoundIsCombatStart");
        fightState = FightState.Fighting;//�༭��ģʽ����, ����ս��
        UnLockPlaceItem();//ս��ģʽ�½�����ѱ༭��Ʒ������

        // ��ʾѪ��
        player1.playerHealth.comp_HealthBar.gameObject.SetActive(true);
        player2.playerHealth.comp_HealthBar.gameObject.SetActive(true);
        // ����
        player1.playerHealth.RebornPlayer();
        player2.playerHealth.RebornPlayer();
        // ����Ѫ��
        player1.playerHealth.avoidHit = false;
        player2.playerHealth.avoidHit = false;
        // ���������
        PickupItemManager.Instance.ClearPickUpItem();
        PickupItemManager.Instance.enabled = true;
        // ��������
        player1.ClearWeapon();
        player2.ClearWeapon();

        // �����������
        _OnRefreshPlayerWeapon();
        //
        ClockParent.gameObject.SetActive(true);

        // ����λ��
        player1.transform.position = init_pos_1;
        player2.transform.position = init_pos_2;
        // ��������
        player1.playerController.LockAttack = false;
        player2.playerController.LockAttack = false;
        combatUI.view_Combating.Show();
    }
    /// <summary>
    /// ���غ���ս������ʱ
    /// </summary>
    private void OnRoundIsCombatOver()
    {
        currentWaitTime = 0;
        currentFightTime = 0;

        Debug.Log("OnRoundIsCombatOver");
        // ��������
        InputManager.Instance.mLockPlayerInputNum = 1;
        // ��������
        player1.playerController.LockAttack = true;
        player1.playerController.Attack_Release();
        player1.playerController.Attack_Release_Far();
        player2.playerController.LockAttack = true;
        player2.playerController.Attack_Release();
        player2.playerController.Attack_Release_Far();
        // ����Ѫ��
        player1.playerHealth.avoidHit = true;
        player2.playerHealth.avoidHit = true;
        // ��ʾѪ��
        player1.playerHealth.comp_HealthBar.gameObject.SetActive(true);
        player2.playerHealth.comp_HealthBar.gameObject.SetActive(true);
        // ���������
        PickupItemManager.Instance.ClearPickUpItem();
        PickupItemManager.Instance.enabled = false;

        //
        ClockParent.gameObject.SetActive(false);
    }
    /// <summary>
    /// ����Ϸ����ʱ
    /// </summary>
    private void OnEndGameWinner()
    {
        // ����λ��
        player1.transform.position = init_pos_1;
        player2.transform.position = init_pos_2;

        // ���������
        PickupItemManager.Instance.ClearPickUpItem();
        PickupItemManager.Instance.enabled = true;
        // ����
        player1.playerHealth.RebornPlayer();
        player2.playerHealth.RebornPlayer();
        // ����Ѫ��
        player1.playerHealth.avoidHit = false;
        player2.playerHealth.avoidHit = false;
        // ��������
        player1.ClearWeapon();
        player2.ClearWeapon();

        Destroy(player1ItemData);
        Destroy(player2ItemData);
    }

    private void _OnRefreshPlayerWeapon()
    {
        // �����������
        int num_1 = UnityEngine.Random.Range(1, 7);
        int num_2 = UnityEngine.Random.Range(1, 7);
        player1.SetWeaponNum(num_1);
        player2.SetWeaponNum(num_2);

        for(int i = 0; i < Tou_1.Count; i++)
        {
            // ������λ�ã�����Ҫ������
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
        //combatUI.txt_TestScore.text = $"��ʹ:{player1_Score}��[{player1.playerHealth.health}Ѫ] : ��ħ:{player2_Score}��[{player2.playerHealth.health}Ѫ]";
        current_Round++;//�ִε��� 1 -> 2 -> 3
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
    /// ����༭��ʱ��
    /// </summary>
    public void ClearEditorTime()
    {
        currentEditorTime = 0f;
    }
    #region CombatOver
    /// <summary>
    /// ��ǰ�غ�ս������
    /// </summary>
    /// <param name="currentRoundWinner">��ǰ�غϻ�ʤ��</param>
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
        Debug.Log($"��ǰ�غ�: {current_Round}, ��ʤ��: {currentRoundWinner}");

        //�������1: ��һ������Ѿ������2��, ����Ҫ���е�����
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

        //�������2: �Ѿ����������һ��
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
        //��ʾ�����ִν�����Ľ���UI, ����ڴ�֮ǰ��Ϸ���� ����ִ�е��˴�
        combatUI.view_CombatOver.Show();
        yield return new WaitForSeconds(3f);
        combatUI.view_CombatOver.Close();
        Fight();
    }
    /// <summary>
    /// ������ջ�ʤ���
    /// </summary>
    /// <returns>��ʤ���</returns>
    private PlayerType CheckEndingWinner()
    {
        if (player1_Score > player2_Score)
            return PlayerType.Player_1;
        else
            return PlayerType.Player_2;
    }
    private void ShowEndWinnerUI(PlayerType endWinner)
    {
        Debug.Log($"��Ϸ����, ���ջ�ʤ��: {endWinner}, ���1�÷�:{player1_Score}, ���2�÷�: {player2_Score}");
        //ִ�к������� �����UI��ʾ��......
        combatUI.view_CombatOver.Close();
        combatUI.view_EndWinner.Show();
        switch (endWinner)
        {
            case PlayerType.Player_1: { combatUI.view_EndWinner.ShowAngelWin(); } break;
            case PlayerType.Player_2: { combatUI.view_EndWinner.ShowDemoWin(); } break;
        }
    }
    /// <summary>
    /// ������ҷ���
    /// </summary>
    /// <param name="winner">��ʤ��</param>
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
    /// ����ս��
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
    /// ������Ϸ
    /// </summary>
    private void EndGame()
    {
        OnEndGameWinner();
        ResetCombat();
        StartCoroutine(ClearPlaceItems());
    }
    #endregion

    /// <summary>
    ///��ӱ༭��ɵ���Ʒ
    /// </summary>
    /// <param name="item"></param>
    public void AddTrapItem(TrapsItem item)
    {
        placeItems.Add(item);
    }
    /// <summary>
    /// ��ձ༭��Ʒ
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
    /// δ��ս����
    /// </summary>
    UnFight,
    ReadyEnterEditor,
    /// <summary>
    /// �༭��ģʽ
    /// </summary>
    InEditor,
    /// <summary>
    /// ս����
    /// </summary>
    Fighting,
    /// <summary>
    /// ս������
    /// </summary>
    FightOver,
}