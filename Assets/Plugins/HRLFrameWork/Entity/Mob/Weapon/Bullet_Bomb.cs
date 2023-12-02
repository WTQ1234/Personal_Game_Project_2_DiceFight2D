using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HRL;

public class Bullet_Bomb : Bullet
{
    public PlayerAbility_SummonObj ability_SummonObj;
    protected override void Awake()
    {
        base.Awake();
        timer_auto_destroy = TimerManager.Instance.AddTimer(_OnTimeOut, time_auto_destroy);
    }

    public override void OnSetDamage(int _damage)
    {
        base.OnSetDamage(_damage);
        if (ability_SummonObj != null)
        {
            ability_SummonObj.damage = _damage;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag != "Player") return;
        Entity entity = collision.transform.GetComponent<Entity>();
        if (entity == null) return;
        TeamController teamController = entity.teamController;
        int teamId = teamController.teamId;
        if (!teamController.DetectTeam(teamId))
        {
            _OnTimeOut();
        }
    }

    protected virtual void _OnTimeOut()
    {
        TimerManager.Instance.CancelTimer(timer_auto_destroy);
        //timer_auto_destroy = -1;
        ability_SummonObj?.OnAbilityCall();
        Destroy(gameObject);
    }
}
