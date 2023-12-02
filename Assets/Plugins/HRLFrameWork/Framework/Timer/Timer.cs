using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HRL
{
    public class Timer
    {
        public int ID;
        public UnityAction unityAction;
        public float duration;
        public int loopTime;

        public bool isRunning;
        public int _curLoopTime;
        public float _curDuration;

        public void Init(int _ID, UnityAction _unityAction, float _duration = 1, int _loopTime = -1)
        {
            ID = _ID;
            unityAction = _unityAction;
            duration = _duration;
            loopTime = _loopTime;
            Reset();
        }

        public void Tick(float deltaTime)
        {
            if (!isRunning) return;
            _curDuration -= deltaTime;
            if (_curDuration < 0)
            {
                _Excute();
                _curDuration = duration;
            }
        }

        private void Reset()
        {
            isRunning = true;
            _curLoopTime = loopTime;
            _curDuration = duration;
        }

        private void _Excute()
        {
            if (loopTime < 0)
            {
                try
                {
                    unityAction?.Invoke();
                }
                catch(Exception e)
                {
                    Debug.LogError(e);
                }
            }
            else
            {
                _curLoopTime -= 1;
                try
                {
                    unityAction?.Invoke();
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
                finally
                {
                    _DetectLoop();
                }
            }
        }

        private void _DetectLoop()
        {
            if (loopTime <= 0)
            {
                return;
            }
            if (_curLoopTime <= 0)
            {
                TimerManager.Instance.CancelTimer(ID);
            }
        }
    }
}
