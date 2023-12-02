using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HRL;

public class AudioManager : MonoSingleton<AudioManager>
{
    [SerializeField] AudioSource sfxPlayer_Music;
    [SerializeField] AudioSource sfxPlayer;
    const float LOW_PITCH = 1f;
    const float HIGH_PITCH = 1f;

    public float audioVolume = 1;
    public float musicVolume = 1;

    protected override void Init()
    {
        base.Init();
        audioVolume = PlayerPrefs.GetFloat("AudioVolume", 1f);
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
    }

    public void SetAudioVolume(float _volume)
    {
        audioVolume = _volume;
        sfxPlayer.volume = _volume;
        PlayerPrefs.SetFloat("AudioVolume", audioVolume);
    }

    public void SetMusicVolume(float _volume)
    {
        musicVolume = _volume;
        sfxPlayer_Music.volume = _volume;
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
    }

    //适用于普通音效
    public void PlaySFX(AudioData audioData)
    {
        sfxPlayer.PlayOneShot(audioData.audioClip, audioVolume);
    }
    //适用于重复播放的音效
    public void PlayRandomSFX(AudioData audioData)
    {
        sfxPlayer.pitch = Random.Range(LOW_PITCH, HIGH_PITCH);
        PlaySFX(audioData);
    }
    //适用于播放随机类型的音效
    public void PlayRandomSFX(AudioData[] audioDatas)
    {
        sfxPlayer.pitch = Random.Range(LOW_PITCH, HIGH_PITCH);
        PlaySFX(audioDatas[Random.Range(0, audioDatas.Length)]);
    }

    public void PlayBGM(AudioClip audioClip)
    {
        sfxPlayer_Music.volume = musicVolume;
        sfxPlayer_Music.clip = audioClip;
        sfxPlayer_Music.loop = true;
        sfxPlayer_Music.Play();
    }
}

//使用类来封装字段
[System.Serializable] public class AudioData
{
    public AudioClip audioClip;
    public float volume;
}