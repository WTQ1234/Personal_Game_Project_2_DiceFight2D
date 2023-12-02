using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HRL;
using DG.Tweening;
using Sirenix.OdinInspector;


public class SceneEntity_Clock : SceneEntity
{
    public GameObject Clock_Sting;

    public GameObject Clock_Show;

    public UIViewInScene weapon_tips;
    public override void Damage(int damage = 1, bool knock_back = false, Transform trans_damage_from = null)
    {
        if (isInDamageCD)
        {
            return;
        }
        CombatManager.Instance.OnDamageClock();
        isInDamageCD = true;
        current_cd = 0;
        Clock_Show.transform.DOShakePosition(0.2f, 0.2f);

        UIInfo screen_ui_info = new UIInfo();
        screen_ui_info.RegisterParam("content", "计时加速");
        UIManager.Instance.CreateSceneUI(weapon_tips, trans_damage_from.position, true, screen_ui_info);
    }

    public float max_cd = 0.5f;
    public float current_cd;
    public bool isInDamageCD;
    public float current_angle;

    protected override void Awake()
    {
        base.Awake();
        isInDamageCD = false;
    }

    protected override void Update()
    {
        base.Update();
        if (isInDamageCD)
        {
            current_cd += Time.deltaTime;
            if (current_cd > max_cd)
            {
                current_cd = 0;
                isInDamageCD = false;
            }
        }
        float rate = CombatManager.Instance.currentWaitTime / CombatManager.Instance.MaxWaitTime;
        current_angle = Mathf.Lerp(current_angle, 360 * rate, 0.5f);
        Clock_Sting.transform.localRotation = Quaternion.AngleAxis(current_angle
            , Vector3.left);
    }
}
