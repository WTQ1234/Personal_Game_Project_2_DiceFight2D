using System.Linq;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.Utilities;
using Sirenix.OdinInspector;
using HRL;

[SerializeField]
public class LanguageInfo : BasicInfo
{
    public LanguageType languageType;

    public string languageName;

    public Sprite languageImg;

    public Dictionary<string, string> dic_languages;
}

public enum LanguageType
{
    CH,
    EN,
    JA,
    CH_T,
}
