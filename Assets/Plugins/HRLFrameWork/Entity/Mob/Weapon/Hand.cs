using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HRL;
using System;
using Sirenix.OdinInspector;

public class Hand : Entity, IComparable<Hand>
{
    public Weapon weapon;

    public Entity owner;

    public SpriteRenderer sprite_hand;

    public bool autoLookAtTarget = true;
    [ShowIf("autoLookAtTarget")]
    public float currentAngle;
    [ShowIf("autoLookAtTarget")]
    public Entity target;
    protected override void Update()
    {
        base.Update();
        if (autoLookAtTarget && target != null)
        {
            Vector2 direction = target.transform.position - transform.position;
            currentAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(currentAngle
                , Vector3.forward);
            //Vector3 v = (target.transform.position - transform.position).normalized;
            //transform.right = v;
        }
    }

    public void OnSetEntityTarget(Entity enemyTarget)
    {
        target = enemyTarget;
    }

    public int CompareTo(Hand other)
    {
        if (weapon == null)
        {
            return 1;
        }
        if (other.weapon == null)
        {
            return -1;
        }
        return weapon.CompareTo(other.weapon);
    }

    public void SetWeapon(Weapon _weapon)
    {
        weapon = _weapon;
        sprite_hand.enabled = false;
    }

    public void OnDropWeapon()
    {
        weapon = null;
        sprite_hand.enabled = true;
    }
}
