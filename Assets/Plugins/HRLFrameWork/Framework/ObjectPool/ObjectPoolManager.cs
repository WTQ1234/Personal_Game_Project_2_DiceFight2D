using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HRL
{
    public class ObjectPoolManager : MonoSingleton<ObjectPoolManager>
    {
        private Dictionary<string, ObjectPool> dict_prefab_pools = new Dictionary<string, ObjectPool>();

        public ObjectPool InitObjectPool(GameObject prefab, int poolSize = 16)
        {
            if (prefab == null) return null;
            if (!dict_prefab_pools.ContainsKey(prefab.name))
            {
                GameObject trans_parent = new GameObject();
                trans_parent.name = "ObjectPool_" + prefab.name;
                trans_parent.transform.parent = transform;
                trans_parent.transform.localPosition = Vector3.zero;
                trans_parent.transform.rotation = Quaternion.identity;
                trans_parent.transform.localScale = Vector3.one;
                ObjectPool objectPool = new ObjectPool(prefab, trans_parent.transform, poolSize);
                dict_prefab_pools.Add(prefab.name, objectPool);
                return objectPool;
            }
            else
            {
                return dict_prefab_pools[prefab.name];
            }
        }

        public GameObject GetObject(GameObject prefab)
        {
            ObjectPool objectPool = InitObjectPool(prefab);
            return objectPool.Get();
        }
        public GameObject GetObject(string prefab_name)
        {
            if (dict_prefab_pools.TryGetValue(prefab_name, out ObjectPool objectPool))
            {
                return objectPool.Get();
            }
            Debug.LogError($"Can not Get ObjectPool by name {prefab_name}");
            return null;
        }

        public bool ReturnToPool(GameObject prefab, GameObject obj)
        {
            ObjectPool objectPool = InitObjectPool(prefab);
            objectPool.ReturnToPool(obj);
            return true;
        }
        public bool ReturnToPool(string prefab_name, GameObject obj)
        {
            if (dict_prefab_pools.TryGetValue(prefab_name, out ObjectPool objectPool))
            {
                objectPool.ReturnToPool(obj);
                return true;
            }
            Debug.LogError($"Can not get ObjectPool for ReturnToPool by name {prefab_name}");
            return false;
        }
    }
}