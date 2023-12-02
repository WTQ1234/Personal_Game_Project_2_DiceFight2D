using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace HRL
{
    public class LanguageManager : MonoSingleton<LanguageManager>
    {
        public LanguageType languageType;

        [ShowInInspector, ReadOnly]
        public Dictionary<LanguageType, LanguageInfo> dict_languageInfo;

        public List<Language_Text> languages = new List<Language_Text>();

        protected override void Init()
        {
            base.Init();
            var dict_language = ConfigManager.Instance.GetAllInfo<LanguageInfo>();
            dict_languageInfo = new Dictionary<LanguageType, LanguageInfo>();
            foreach (var pair in dict_language)
            {
                dict_languageInfo.Add(pair.Value.languageType, pair.Value);
            }
            _ChangeLangeuage();
        }

        public void ChangeLanguage(LanguageType _languageType)
        {
            languageType = _languageType;
            _ChangeLangeuage();
        }

        private void _ChangeLangeuage()
        {
            foreach (var language in languages)
            {
                language.OnLanguageChange();
            }
        }

        public string OnGetLanguage(string original_text)
        {
            if (dict_languageInfo.TryGetValue(languageType, out var language))
            {
                if (language.dic_languages.TryGetValue(original_text, out string value))
                {
                    return value;
                }
            }
            //LogHelper.Warning("Language", "多语言文本缺失: " + original_text);
            if (languageType == LanguageType.CH)
            {
                return original_text;
            }
            return "多语言文本缺失: " + original_text;
        }

        public void RegisterLanguageText(Language_Text language_text)
        {
            if (!languages.Contains(language_text))
            {
                languages.Add(language_text);
                language_text.OnLanguageChange();
            }
        }

        public void UnregisterLanguageText(Language_Text language_text)
        {
            if (languages.Contains(language_text))
            {
                languages.Remove(language_text);
            }
        }
    }
}