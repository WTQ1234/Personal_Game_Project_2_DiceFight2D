using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;

public class LogHelper : MonoBehaviour
{
    [System.Serializable]
    public class LogSetting
    {
        public bool print = true;
        public string type = "Test";  // 放在字典的key里
        public Color color = Color.blue;
    }

    private static LogHelper instance;
    public static LogHelper Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<LogHelper>();
                if (instance == null)
                {
                    instance = new GameObject("_" + typeof(LogHelper).Name).AddComponent<LogHelper>();
                    DontDestroyOnLoad(instance);
                }
            }
            return instance;
        }
        set { }
    }

    [ShowInInspector]
    [Title("自定义打印")]
    public List<LogSetting> List_LogSetting;

    public static void Info(string type="Log", params object[] args)
    {
        if (Instance == null || Instance.List_LogSetting == null)
        {
            return;
        }
        foreach(LogSetting logSetting in Instance.List_LogSetting)
        {
            if (type == logSetting.type)
            {
                if (!logSetting.print)
                {
                    return;
                }
                Debug.Log($"<color=#{ColorUtility.ToHtmlStringRGB(logSetting.color)}>[{type}]</color> {string.Join(",", args)}");
                return;
            }
        }
        Debug.Log(string.Join(",", args));
    }
    
    public static void Warning(string type="Warning", params object[] args)
    {
        foreach (LogSetting logSetting in Instance.List_LogSetting)
        {
            if (type == logSetting.type)
            {
                if (!logSetting.print)
                {
                    return;
                }
                Debug.LogWarning($"<color=#{ColorUtility.ToHtmlStringRGB(logSetting.color)}>[{type}]</color> {string.Join(",", args)}");
                return;
            }
        }
        Debug.LogWarning(string.Join(",", args));
    }

    public static void Error(string type="Error", params object[] args)
    {
        foreach (LogSetting logSetting in Instance.List_LogSetting)
        {
            if (type == logSetting.type)
            {
                if (!logSetting.print)
                {
                    return;
                }
                Debug.LogError($"<color=#{ColorUtility.ToHtmlStringRGB(logSetting.color)}>[{type}]</color> {string.Join(",", args)}");
                return;
            }
        }
        Debug.LogError(string.Join(",", args));
    }
}