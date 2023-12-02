using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HRL
{
    public class Language_Text : Text
    {
        public override string text
        {
            get
            {
                return m_Text;
            }
            set
            {
                original_text = value;
                if (System.String.IsNullOrEmpty(original_text))
                {
                    if (System.String.IsNullOrEmpty(m_Text))
                        return;
                    m_Text = "";
                    SetVerticesDirty();
                }
                else if (m_Text != original_text)
                {
                    m_Text = OnSetLanguage();
                    SetVerticesDirty();
                    SetLayoutDirty();
                }
            }
        }

        public string original_text = "";

        public void OnLanguageChange()
        {
            m_Text = OnSetLanguage();
            SetVerticesDirty();
            SetLayoutDirty();
        }

        private string OnSetLanguage()
        {
            if (string.IsNullOrEmpty(original_text))
            {
                original_text = m_Text;
            }
            return LanguageManager.Instance.OnGetLanguage(original_text);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            if (Application.isPlaying)
            {
                LanguageManager.Instance.RegisterLanguageText(this);
            }
        }

        protected override void OnDisable()
        {
            if (Application.isPlaying && LanguageManager.Instance)
            {
                LanguageManager.Instance.UnregisterLanguageText(this);
            }
            base.OnDisable();
        }
    }
}
