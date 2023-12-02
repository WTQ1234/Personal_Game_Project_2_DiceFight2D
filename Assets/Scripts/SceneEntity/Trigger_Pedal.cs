using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using HRL;
using DG.Tweening;
using Sirenix.OdinInspector;

public delegate bool UnityEventDetect();

public class Trigger_Pedal : Trigger
{
    public float pedal_time = 0.5f;
    public float yscale_up = 0.15f;
    public float yscale_down = 0.05f;

    public UnityEvent unityEvent_OnEnter;

    public UnityEvent unityEvent_OnExit;

    protected override bool _Detect(Collider2D collision)
    {
        if (collision == null)
        {
            return true;
        }
        var entity = collision.GetComponent<Entity>();
        if (entity != null && entity.isPlayer() || entity.getEntityType() == EntityType.PlayerSummon)
        {
            return true;
        }
        return false;
    }

    protected override void _Execute(Collider2D collision)
    {
        if (collision == null)
        {
            transform.DOScaleY(yscale_down, pedal_time);
            return;
        }
        transform.DOScaleY(yscale_down, pedal_time);
        unityEvent_OnEnter?.Invoke();
    }

    protected override void _Reset(Collider2D collision)
    {
        if (collision == null)
        {
            transform.DOScaleY(yscale_up, pedal_time);
            return;
        }
        transform.DOScaleY(yscale_up, pedal_time);
        unityEvent_OnExit?.Invoke();
    }
}
