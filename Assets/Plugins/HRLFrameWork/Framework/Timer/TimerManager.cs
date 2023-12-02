using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace HRL
{
    public class TimerManager : MonoSingleton<TimerManager>
    {
        [ShowInInspector]
        public List<Timer> timerList = new List<Timer>();
        public List<Timer> timerList_ToCancel = new List<Timer>();

        private float waitingTime = 0;
        private int curID = 0;

        public int AddTimer(UnityAction _unityAction, float _duration = 1, int _loopTime = 1)
        {
            int _ID = _GetNewID();
            Timer timer = new Timer();
            timer.Init(_ID, _unityAction, _duration, _loopTime);
            timerList.Add(timer);
            return _ID;
        }

        public int AddRepeatTimer(UnityAction _unityAction, float _duration = 1)
        {
            return AddTimer(_unityAction, _duration, -1);
        }

        public Timer GetTimer(int _ID)
        {
            for (int i = 0; i < timerList.Count; i++)
            {
                if (timerList[i].ID == _ID)
                {
                    return timerList[i];
                }
            }
            return null;
        }

        public void CancelTimer(int timerID)
        {
            Timer timer = GetTimer(timerID);
            if (timer == null)
            {
                return;
            }
            if (!timer.isRunning)
            {
                return;
            }
            timer.isRunning = false;
            timerList_ToCancel.Add(timer);
        }

        private int _GetNewID()
        {
            return curID++;
        }

        void Update()
        {
            float deltaTime = Time.deltaTime;
            waitingTime += deltaTime;
            if (waitingTime > 1)
            {
                waitingTime = 0;
                foreach(Timer timer in timerList_ToCancel)
                {
                    if (timerList.Contains(timer))
                    {
                        timerList.Remove(timer);
                    }
                }
                timerList_ToCancel.Clear();
            }
            foreach (Timer timer in timerList)
            {
                timer.Tick(deltaTime);
            }
        }
    }
}
