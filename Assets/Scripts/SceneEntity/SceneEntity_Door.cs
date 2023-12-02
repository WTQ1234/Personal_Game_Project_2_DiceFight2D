using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HRL;
using DG.Tweening;
using Sirenix.OdinInspector;

public class SceneEntity_Door : SceneEntity
{
    public float wait_to_down = 1f;

    public float tween_time = 0.5f;
    public float ypos_up = 0.7f;
    public float ypos_down = 0f;

    public float cur_wait_to_down = 0f;
    public bool is_count_down = false;

    private Tween cur_tween;

    [Title("触发引用计数")]
    public bool useTriggerCount = true;
    [ShowInInspector, ReadOnly]
    private int cur_trigger_count = 0;

    public void Up()
    {
        if (useTriggerCount)
        {
            cur_trigger_count++;
            if (cur_trigger_count > 1)
            {
                return;
            }
        }
        if (cur_tween != null)
        {
            cur_tween.Kill();
        }
        cur_tween = transform.DOLocalMoveY(ypos_up, Mathf.Lerp(0, tween_time, Mathf.Abs(transform.localPosition.y - ypos_up) / Mathf.Abs(ypos_down - ypos_up)));
        is_count_down = false;
    }

    public void Down()
    {
        if (useTriggerCount)
        {
            cur_trigger_count--;
            if (cur_trigger_count > 0)
            {
                return;
            }
        }
        is_count_down = true;
        cur_wait_to_down = 0f;
    }

    private void Update()
    {
        if (is_count_down)
        {
            cur_wait_to_down += Time.deltaTime;
            if (cur_wait_to_down >= wait_to_down)
            {
                is_count_down = false;
                if (cur_tween != null)
                {
                    cur_tween.Kill();
                }
                cur_tween = transform.DOLocalMoveY(ypos_down, Mathf.Lerp(0, tween_time, Mathf.Abs(transform.localPosition.y - ypos_down) / Mathf.Abs(ypos_down - ypos_up)));
            }
        }
    }
}
