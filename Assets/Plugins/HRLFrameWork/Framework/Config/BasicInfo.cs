using System;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using Sirenix.OdinInspector;

namespace HRL
{
    // todo 将存储xml的路径存在temp文件夹里，并实现一键存储xml
    public class BasicInfo : SerializedScriptableObject
    {
        //  [GUIColor(.5f, 1, 0, 1)]//颜色
        //  [Searchable]//使列表可搜索
        //  [TypeFilter("GetFilteredTypeList")]//通过基类显示子类
        //[Title("按钮-暂时无效")]
        //[Button("转换为XML", ButtonSizes.Medium)]
        public void CreateAttrAsset()
        {
            return;
            //1.得到存储路径
            string path = Application.persistentDataPath + "/" + FileName + ".xml";
            Debug.Log(path);
            //2.存储文件
            using (StreamWriter writer = new StreamWriter(path))
            {
                //3.序列化
                XmlSerializer s = new XmlSerializer(this.GetType());
                s.Serialize(writer, this);
            }
        }

        //[Button("从XML读取", ButtonSizes.Medium)]
        public void LoadFromXML()
        {
            return;
            Type type = this.GetType();
            //1。首先要判断文件是否存在
            string path = Application.persistentDataPath + "/" + FileName + ".xml";
            if (!File.Exists(path))
            {
                path = Application.streamingAssetsPath + "/" + FileName + ".xml";
                if (!File.Exists(path))
                {
                    //如果根本不存在文件 两个路径都找过了
                    Debug.Log("读取失败");
                }
            }
            //2.存在就读取
            using (StreamReader reader = new StreamReader(path))
            {
                //3.反序列化 取出数据
                XmlSerializer s = new XmlSerializer(type);
                System.Object xmlInfo = s.Deserialize(reader);
                // ScriptableObject mInfo = ScriptableObject.CreateInstance(type.ToString());

                System.Reflection.FieldInfo[] pA = xmlInfo.GetType().GetFields();
                System.Reflection.FieldInfo[] pB = this.GetType().GetFields();
                for (int i = 0; i < pA.Length; i++)
                {
                    // if(pB[i].CanWrite)
                    pB[i].SetValue(this, pA[i].GetValue(xmlInfo));
                }
            }
        }

        [Title("基础配置")]
        [ReadOnly]
        public int Id;
        [ReadOnly]
        public string FileName;

        public string Name;

        public virtual void InitAfterCreateFile()
        {

        }

        public override string ToString()
        {
            return Name?.ToString();
        }
    }
}