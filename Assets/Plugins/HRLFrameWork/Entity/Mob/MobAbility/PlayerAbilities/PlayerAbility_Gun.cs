using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HRL;

public class PlayerAbility_Gun : PlayerAbilityBase
{
    [HideInInspector]
    public Weapon weapon;
    public Animator animator;

    public Transform shoot_point;

    public int damage = 1;
    public float bullet_volicity = 1;

    public int each_bullet_num = 1;
    public float aim_angle = 0f;

    public Bullet prefab_bullet;

    protected override void _SetName()
    {
        Name = "Gun";
    }

    protected override bool _Execute()
    {
        base._Execute();
        animator.SetBool("Attack", true);
        if (invokeByLongPress)
        {
            isCurrentLongPress = true;
        }
        return true;
    }

    protected override void _Cancel()
    {
        base._Cancel();
        animator.SetBool("Attack", false);
        if (invokeByLongPress)
        {
            isCurrentLongPress = false;
        }
    }

    public void OnShoot()
    {
        if (!this.enabled)
        {
            return;
        }
        if (currentTarget == null)
        {
            return;
        }
        for (int i = 0; i < each_bullet_num; i++)
        {
            Bullet bullet = Instantiate<Bullet>(prefab_bullet);
            bullet.transform.position = shoot_point.position;
            bullet.OnSetDamage(damage);
            bullet.teamController.SetTeam(Owner.teamController);

            if (aim_angle > 0)
            {
                float cur_angle = Random.Range(-aim_angle, aim_angle);
                bullet.transform.rotation = shoot_point.rotation;
                bullet.transform.Rotate(Vector3.forward * cur_angle);
                Vector3 dir = currentTarget.transform.position - transform.position;
                Matrix4x4 rotate = Matrix4x4.Rotate(Quaternion.Euler(0, 0, cur_angle));
                bullet.OnSetVelocity(rotate.MultiplyVector(dir), bullet_volicity);
            }
            else
            {
                bullet.transform.rotation = shoot_point.rotation;
                bullet.OnSetVelocity(currentTarget.transform.position - transform.position, bullet_volicity);
            }
        }

        weapon?.OnCostDurable(1);
    }
}
