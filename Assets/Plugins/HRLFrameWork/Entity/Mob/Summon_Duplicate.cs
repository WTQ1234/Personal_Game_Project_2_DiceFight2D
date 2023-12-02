using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HRL;

public class Summon_Duplicate : SceneEntity
{
    public override EntityType getEntityType()
    {
        return EntityType.PlayerSummon;
    }
}
