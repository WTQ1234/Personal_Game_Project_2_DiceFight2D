using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HRL;

public class PlayerAbility_SummonObj : PlayerAbilityBase
{
    public bool destroyOnSummon = true;

    public GameObject prefab_summon;

    public int damage;

    protected override void _SetName()
    {
        Name = "SummonObj";
    }

    protected override bool _Execute()
    {
        base._Execute();
        GameObject obj = GameObject.Instantiate(prefab_summon);
        obj.transform.position = transform.position;
        TeamController teamController = obj.GetComponent<TeamController>();
        if (teamController != null)
        {
            teamController.SetTeam(Owner.teamController);
        }
        PlayerAbility_TouchToHurt playerAbility_TouchToHurt = obj.GetComponent<PlayerAbility_TouchToHurt>();
        if (playerAbility_TouchToHurt != null)
        {
            playerAbility_TouchToHurt.damage = damage;
        }
        if (destroyOnSummon)
        {
            Destroy(gameObject);
        }
        return true;
    }
}
