using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using HRL;

public class SceneEntity_Elevator : MonoBehaviour
{
    public DOTweenPath path;

    public bool isPlay = false;
    public bool isMoving = false;

    public List<Trigger_Pedal> trigger_Pedals = new List<Trigger_Pedal>();

    public void Up(Trigger_Pedal cur_pedal)
    {
        if (isMoving) return;
        isPlay = !isPlay;
        if (isPlay)
        {
            isMoving = true;
            path.DORestart();   // 奇怪的坑，用DoPlay会导致第3次开始失效
        }
        else
        {
            isMoving = true;
            path.DOPlayBackwards();
        }
        foreach (var pedal in trigger_Pedals)
        {
            if (cur_pedal != pedal)
            {
                pedal.AssumeTriggerEnter();
            }
            pedal.SetCanInteract(false);
        }
    }

    public void onComplete()
    {
        // 1秒延迟，防止一些奇怪的bug
        TimerManager.Instance.AddTimer(() =>
        {
            isMoving = false;
            LeavePedal(null);
        }, 1, 1);
    }

    public void LeavePedal(Trigger_Pedal cur_pedal)
    {
        if (isMoving) return;
        foreach (var pedal in trigger_Pedals)
        {
            pedal.SetCanInteract(true);
            if (cur_pedal != pedal)
            {
                pedal.AssumeTriggerExit();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.GetType().ToString() == "UnityEngine.BoxCollider2D")
        {
            other.gameObject.transform.parent = gameObject.transform;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.GetType().ToString() == "UnityEngine.BoxCollider2D")
        {
            other.gameObject.transform.parent = null;
        }
    }
}
