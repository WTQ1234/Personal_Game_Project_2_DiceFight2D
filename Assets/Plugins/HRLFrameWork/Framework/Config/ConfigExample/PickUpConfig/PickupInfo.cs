using System.Linq;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.Utilities;
using Sirenix.OdinInspector;
using HRL;

public enum PickupType
{
    Weapon,
    Buff,
}

[SerializeField]
public class PickupInfo : BasicInfo
{
    public string PickupName;

    public string PickupDesc;
    
    public Sprite PickupIcon;

    public PickupType type;

    [Title("ÎäÆ÷")]
    [ShowIf("@type == PickupType.Weapon")]
    public WeaponInfo weaponInfo;
}
