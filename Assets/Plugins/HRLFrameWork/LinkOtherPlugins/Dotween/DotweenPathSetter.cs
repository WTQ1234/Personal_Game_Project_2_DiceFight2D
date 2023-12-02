using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Plugins.Core.PathCore;    // Path
using Sirenix.OdinInspector;

namespace HRL
{
    // use for set path, in need of dotween.
    public class DotweenPathSetter : MonoBehaviour
    {
        public List<Transform> movingPath = new List<Transform>();
        public float duration = 3;

        Vector3[] vector3_array;
        bool isBack = false;
        private void Start()
        {
            vector3_array = new Vector3[movingPath.Count];
            for (int i = 0; i < movingPath.Count; i++)
            {
                vector3_array[i] = movingPath[i].position;
            }
            __DoPath();
        }

        private void __DoPath()
        {
            vector3_array = new Vector3[movingPath.Count];
            if (!isBack)
            {
                for (int i = 0; i < movingPath.Count; i++)
                {
                    vector3_array[i] = movingPath[i].localPosition;
                }
            }
            else
            {
                int j = 0;
                for (int i = movingPath.Count - 1; i >= 0; i--)
                {
                    vector3_array[j] = movingPath[i].localPosition;
                    j++;
                }
            }
            isBack = !isBack;

            var tween = transform.DOLocalPath(vector3_array, duration, PathType.Linear, PathMode.TopDown2D, 10, null);
            tween.onComplete += __DoPath;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Transform last_t = transform;
            for(int i = 0; i < movingPath.Count; i++)
            {
                Transform t = movingPath[i];
                if (last_t != null)
                {
                    Gizmos.DrawLine(t.position, last_t.position);
                }
                last_t = t;
            }
        }
    }

}
