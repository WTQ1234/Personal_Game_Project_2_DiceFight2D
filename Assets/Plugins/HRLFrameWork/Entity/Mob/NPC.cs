using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HRL;

public class NPC : Entity
{
    public override EntityType getEntityType()
    {
        return EntityType.NPC;
    }
}
