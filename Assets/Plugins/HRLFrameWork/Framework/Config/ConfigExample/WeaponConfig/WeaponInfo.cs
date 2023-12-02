using System.Linq;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.Utilities;
using Sirenix.OdinInspector;
using HRL;

public enum WeaponQuality
{
    white,
    green,
    gold,
    red,
}

[SerializeField]
public class WeaponInfo : BasicInfo
{
    public string WeaponName;

    public string WeaponDesc;
    
    public Sprite Icon;

    public Weapon prefab_weapon;

    public WeaponQuality quality;

    public int damage;

    public float cd;

    [Title("ÄÍ¾Ã")]
    public int durable;

    [Title("ÊÇ·ñÉä»÷")]
    public bool shoot;
    [ShowIf("shoot")]
    public Bullet prefab_bullet;
    [ShowIf("shoot")]
    public int bullet_damage;
    [ShowIf("shoot")]
    public float bullet_vol;
}
