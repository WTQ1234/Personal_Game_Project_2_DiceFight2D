using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    protected static bool IsInstanceIsInitialized => Instance != null;
    protected bool IsInited = false;

    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                if (instance == null)
                {
                    print(typeof(T).Name);
                    instance = new GameObject("_" + typeof(T).Name).AddComponent<T>();
                    if (Application.isPlaying)
                    {
                        DontDestroyOnLoad(instance);
                    }
                }
                if (!instance.IsInited)
                {
                    instance.Init();
                }
            }
            return instance;
        }
        set { }
    }

    protected virtual void Awake()
    {
        if (!IsInited)
        {
            Init();
        }    
    }

    protected virtual void Init()
    {
        IsInited = true;
    }

    protected virtual void OnDestroy()
    {
        // if (Instance == this) TODO: 此处会导致未被加载的Instance被创建出来，导致报错
        Instance = null;
    }
}
