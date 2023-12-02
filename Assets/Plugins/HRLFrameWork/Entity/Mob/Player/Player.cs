using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HRL;
using Sirenix.OdinInspector;
using DG.Tweening;

public class Player : Entity
{
    public static Player Instance { get; private set; }

    public PlayerHealth playerHealth;

    public PlayerAbilityController abilityController;

    public PlayerController playerController;

    public Interactive curInteractive;

    protected override void Awake()
    {
        Instance = this;
        InitWeapon();
    }

    public override EntityType getEntityType()
    {
        return EntityType.Player;
    }

    public void OnSetInteractive(Interactive interactive, KeyCode keyCode)
    {
        if (curInteractive == interactive) return;
        curInteractive = interactive;
        InputManager.Instance.RegisterInputAction(new InputData(keyCode, ButtonType.Down, InputOccasion.Update, interactive.OnInteract));
    }

    public void OnRemoveInteractive(Interactive interactive, KeyCode keyCode)
    {
        if (curInteractive == null) return;
        curInteractive = null;
        InputManager.Instance.UnRegisterInputAction(new InputData(keyCode, ButtonType.Down, InputOccasion.Update, interactive.OnInteract));
    }

    public override void Damage(int damage = 1, bool knock_back = false, Transform trans_damage_from = null)
    {
        playerHealth.Damage(damage, knock_back, trans_damage_from);
    }

    // TODO: 临时处理自动瞄准
    public Entity enemyTarget;
    public void SetTarget()
    {
        abilityController.OnSetEntityTarget(enemyTarget);
    }

    public bool CanPickUpItem()
    {
        return weapons.Count < weapon_max_num_current;
    }

    public void PickUpItem(PickupItem item)
    {
        switch (item.info.type)
        {
            case PickupType.Weapon:
                GetWeapon(item.info, item.extra_data_weapon_durable);
                break;
        }
    }

    public int weapon_max_num = 6;
    public int weapon_max_num_current = 6;
    public List<Weapon> weapons;
    public List<Hand> hands_list;
    public Transform weapons_parent;
    public Hand weapon_hand_parent;

    public int maxAngle = 160;
    public int minAngle = 20;
    public float radious = 1.5f;

    public float tween_time = 0.5f;

    private void InitWeapon()
    {
        foreach (var item in weapons)
        {
            item.Owner = this;
        }
    }

    [Button("设置肢体数量")]
    public void SetWeaponNum(int num)
    {
        num = Mathf.Clamp(num, 1, 6);
        weapon_max_num_current = num;

        if (hands_list.Count > weapon_max_num_current)
        {
            for (int i = hands_list.Count - 1; i >= weapon_max_num_current; i--)
            {
                // 丢弃多余的手和武器
                DropHand(i);
            }
        }
        else if (hands_list.Count < weapon_max_num_current)
        {
            int need_num = weapon_max_num_current - hands_list.Count;
            for (int i = 0; i < need_num; i++)
            {
                Hand hand = Instantiate<Hand>(weapon_hand_parent, weapons_parent);
                hand.transform.name = $"hand_{i}";
                hand.OnSetEntityTarget(enemyTarget);
                hands_list.Add(hand);
            }
        }
        hands_list.Sort();
        // 按角度排列半圆
        RefreshWeaponPos(true);
    }

    public void ClearWeapon()
    {
        weapons.Clear();
        for (int i = hands_list.Count - 1; i >= 0; i--)
        {
            // 丢弃多余的手和武器
            var hand = hands_list[i];
            Destroy(hand.gameObject);
        }
        hands_list.Clear();
        weapons.Clear();
    }

    // 销毁手 掉落武器
    private void DropHand(int index)
    {
        print($"Drop Hand {index}, weapon {weapons.Count}");
        Hand hand = hands_list[index];
        // 因为是倒序remove，所以直接removeat，可以不用考虑列表从中间remove的变化
        hands_list.RemoveAt(index);
        if (hand.weapon)
        {
            // 创建一个武器掉落物
            PickupInfo pickupInfo = hand.weapon.pickupInfo;
            PickupItemManager.Instance.CreatePickUpItemDrop(hand.weapon.transform.position, pickupInfo, hand.weapon.durable);
            weapons.RemoveAt(hand.weapon.curIndex);
            for (int i = 0; i < weapons.Count; i++)
            {
                Weapon cur_weapon = weapons[i];
                cur_weapon.curIndex = i;
            }
        }
        Destroy(hand.gameObject);
    }

    public UIViewInScene weapon_tips;
    public UIViewInScene weapon_tips_Get;
    // 销毁武器
    public void ExhaustWeapon(int index)
    {
        print($"Remove{index} - {weapons.Count}");
        if (index >= weapons.Count)
        {
            return;
        }
        Weapon weapon = weapons[index];
        weapons.RemoveAt(index);
        for (int i = 0; i < weapons.Count; i++)
        {
            Weapon cur_weapon = weapons[i];
            cur_weapon.curIndex = i;
        }

        UIInfo screen_ui_info = new UIInfo();
        screen_ui_info.RegisterParam("content", weapon.weaponInfo.shoot ? "No Ammo" : "No Durability");
        UIManager.Instance.CreateSceneUI(weapon_tips, weapon.transform.position, true, screen_ui_info);

        var tween = weapon.transform.DOScale(0.1f, tween_time);
        tween.onComplete = () =>
        {
            Destroy(weapon.gameObject);
        };
        Hand hand = hands_list[index];
        hand.OnDropWeapon();
        hands_list.Sort();
        RefreshWeaponPos(true);
    }

    [Button("获取武器")]
    private void GetWeapon(PickupInfo pickupInfo, int extra_data_weapon_durable)
    {
        WeaponInfo weaponInfo = pickupInfo.weaponInfo;

        if (hands_list.Count <= weapons.Count)
        {
            return;
        }
        Hand hand = hands_list[weapons.Count];
        Weapon weapon = Instantiate<Weapon>(weaponInfo.prefab_weapon, hand.transform);
        weapon.curIndex = weapons.Count;
        weapons.Add(weapon);
        weapon.transform.localScale = Vector3.one * 0.1f;
        weapon.transform.DOScale(1f, tween_time);
        weapon.teamController.SetTeam(teamController);
        weapon.Owner = this;
        weapon.SetWeaponInfo(weaponInfo);
        weapon.OnSetEntityTarget(enemyTarget);
        weapon.pickupInfo = pickupInfo;

        UIInfo screen_ui_info = new UIInfo();
        screen_ui_info.RegisterParam("content", pickupInfo.weaponInfo.WeaponName);
        screen_ui_info.RegisterParam("quality", pickupInfo.weaponInfo.quality);
        UIManager.Instance.CreateSceneUI(weapon_tips_Get, weapon.transform.position, true, screen_ui_info);

        if (extra_data_weapon_durable > 0)
        {
            weapon.OnSetDurable(extra_data_weapon_durable);
        }

        hand.SetWeapon(weapon);
        hands_list.Sort();
        RefreshWeaponPos(false);
    }

    private List<Tween> tweens = new List<Tween>();

    [Button("刷新位置")]
    private void RefreshWeaponPos(bool use_tween = true)
    {
        _KillAllMovingTweens();
        int num = hands_list.Count;
        if (num == 1)
        {
            var tmpX = radious * Mathf.Cos(minAngle * 3.14f / 180);
            var tmpY = radious * Mathf.Sin(minAngle * 3.14f / 180);
            if (use_tween)
            {
                Tween tween = hands_list[0].transform.DOLocalMove(new Vector3(tmpX, tmpY), tween_time);
                tweens.Add(tween);
            }
            else
            {
                hands_list[0].transform.localPosition = new Vector3(tmpX, tmpY);
            }
        }
        else
        {
            int each_angle = (maxAngle - minAngle) / (num - 1);
            for(int i = 0; i < num; i++)
            {
                var angle = minAngle + each_angle * i;//角度
                Hand weapon_hand = hands_list[i];
                var tmpX = radious * Mathf.Cos(angle * 3.14f / 180);
                var tmpY = radious * Mathf.Sin(angle * 3.14f / 180);
                if (use_tween)
                {
                    Tween tween = weapon_hand.transform.DOLocalMove(new Vector3(tmpX, tmpY), tween_time);
                    tweens.Add(tween);
                }
                else
                {
                    weapon_hand.transform.localPosition = new Vector3(tmpX, tmpY);
                }
            }
        }
        foreach (var hand in hands_list)
        {
            if (hand.weapon == null)
            {
                hand.sprite_hand.enabled = true;
            }
            else
            {
                hand.sprite_hand.enabled = false;
            }
        }
    }

    private void _KillAllMovingTweens()
    {
        foreach (var tween in tweens)
        {
            tween.Kill();
        }
        tweens.Clear();
    }
}
