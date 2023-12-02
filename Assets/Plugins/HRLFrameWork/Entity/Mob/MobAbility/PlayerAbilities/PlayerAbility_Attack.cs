using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HRL;

// ���ڽ�ս����������һ����ײ�н��м�⣬��ײ�򴥷�����
public class PlayerAbility_Attack : PlayerAbilityBase
{
    [HideInInspector]
    public Weapon weapon;
    public Animator animator;
    public Collider2D collider2d;

    public int damage;

    protected override void _SetName()
    {
        Name = "Attack";
    }

    protected override void OnAbilityEnable()
    {
        base.OnAbilityEnable();
        EndAttackDetect();
    }

    protected override bool _Execute()
    {
        base._Execute();
        animator.SetTrigger("Attack");
        return true;
    }

    public void StartAttackDetect()
    {
        collider2d.enabled = true;
    }

    public void EndAttackDetect()
    {
        collider2d.enabled = false;
        animator.ResetTrigger("Attack");
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collider2d.enabled) return;
        if (collision.tag == "SceneEntity")
        {
            SceneEntity sceneEntity = collision.GetComponent<SceneEntity>();
            // TODO: ��ʵSceneEntityҲ���Կ���ͳ�ϵ�team��
            if (sceneEntity == null) return;
            _CauseDamage(sceneEntity);
        }
        else if (collision.tag == "Player")
        {
            Entity entity = collision.GetComponent<Entity>();
            if (entity == null) return;
            TeamController teamController = entity.teamController;
            int teamId = teamController.teamId;
            if (!Owner.teamController.DetectTeam(teamId))
            {
                _CauseDamage(entity);
            }
        }
    }

    protected void _CauseDamage(Entity target)
    {
        target.Damage(damage, true, transform);
        weapon?.OnCostDurable(1);
    }
}
