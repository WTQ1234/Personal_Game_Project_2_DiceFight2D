using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using System;
using System.Linq;

namespace HRL
{
    public class ConfigManager : MonoSingleton<ConfigManager>
    {
        [ShowInInspector]
        private Dictionary<System.Type, Dictionary<int, BasicInfo>> scriptableObjects = new Dictionary<System.Type, Dictionary<int, BasicInfo>>();

        [ReadOnly, ShowInInspector]
        private static string configPath = "ScriptableObject";

        protected override void Init()
        {
            base.Init();
            LoadScriptableObjects(configPath);
        }

        public Dictionary<int, T> GetAllInfo<T>() where T : BasicInfo
        {
            // 获取指定类型的 BasicInfo 实例列表
            Dictionary<int, BasicInfo> dict = scriptableObjects[typeof(T)];
            Dictionary<int, T> dict_T = new Dictionary<int, T>();
            foreach(var pair in dict)
            {
                dict_T.Add(pair.Key, (T)pair.Value);
            }
            return dict_T;
        }

        // 获取指定类型的 BasicInfo 配置
        public T GetBasicInfoById<T>(int index) where T : BasicInfo
        {
            // 获取指定类型的 BasicInfo 实例列表
            Dictionary<int, BasicInfo> dict = scriptableObjects[typeof(T)];
            // 返回指定序号的 BasicInfo 实例
            if (dict.ContainsKey(index))
            {
                return (T)dict[index];
            }
            return null;
        }

        // 获取满足指定条件的 BasicInfo 配置
        public T GetBasicInfosByLambda<T>(Func<BasicInfo, bool> lambda) where T : BasicInfo
        {
            // 获取指定类型的 BasicInfo 实例列表
            Dictionary<int, BasicInfo> dict = scriptableObjects[typeof(T)];
            var dic = dict.Values.Where(lambda).ToList();
            if (dic.Count <= 0)
            {
                return null;
            }
            return (T)dic[0];
        }

        // 读取指定路径下的所有 BasicInfo 配置，并按类型分组
        public void LoadScriptableObjects(string path)
        {
            // 获取指定路径下的所有文件名
            BasicInfo[] names = Resources.LoadAll<BasicInfo>(path);
            // 遍历每个文件名，并将文件加载为 ScriptableObject 实例
            foreach (BasicInfo scriptableObject in names)
            {
                // 获取 BasicInfo 实例的类型
                System.Type type = scriptableObject.GetType();
                // 如果指定类型的 BasicInfo 实例列表尚未创建，则新建一个列表
                if (!scriptableObjects.ContainsKey(type))
                {
                    scriptableObjects[type] = new Dictionary<int, BasicInfo>();
                }

                // 将 ScriptableObject 实例添加到指定类型的列表中
                scriptableObjects[type].Add(scriptableObject.Id, scriptableObject);
            }
        }
    }
}