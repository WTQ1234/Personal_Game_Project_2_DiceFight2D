using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HRL;

public class UIScreen_Setting : UIScreen
{
    [SerializeField] Text text_Music;
    [SerializeField] Slider slider_Music;
    [SerializeField] Text text_Effect;
    [SerializeField] Slider slider_Effect;
    [SerializeField] Button btn_Apply;

    protected override void Init()
    {
        base.Init();

        text_Effect.text = $"{(int)(AudioManager.Instance.audioVolume * 100)}%";
        slider_Effect.value = AudioManager.Instance.audioVolume;
        text_Music.text = $"{(int)(AudioManager.Instance.musicVolume * 100)}%";
        slider_Music.value = AudioManager.Instance.musicVolume;

        slider_Effect.onValueChanged.AddListener((float value) =>
        {
            text_Effect.text = $"{(int)(value * 100)}%";
            AudioManager.Instance.SetAudioVolume(value);
        });
        slider_Music.onValueChanged.AddListener((float value) =>
        {
            text_Music.text = $"{(int)(value * 100)}%";
            AudioManager.Instance.SetMusicVolume(value);
        });

        btn_Apply.onClick.AddListener(OnClick_Exit);
    }

    public override void OnShown()
    {
        base.OnShown();
        //InputManager.Instance.canClick = false;
    }

    public override void OnHide()
    {
        base.OnHide();
        //InputManager.Instance.canClick = true;
    }

    private void OnClick_Continue()
    {
        Remove();
    }

    private void OnClick_Exit()
    {
        Remove();
    }
}
