using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HRL;

public class PlayerAbility_Duplicate : PlayerAbilityBase
{
    public Summon_Duplicate summon_Duplicate;

    protected override void _SetName()
    {
        Name = "Duplicate";
    }

    protected override bool _Execute()
    {
        base._Execute();
        var summon = GameObject.Instantiate<Summon_Duplicate>(summon_Duplicate);
        summon.transform.position = Owner.transform.position;
        summon.transform.rotation = Owner.transform.rotation;
        return true;
    }
}
