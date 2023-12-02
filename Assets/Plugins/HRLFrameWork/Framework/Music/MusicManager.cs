using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HRL;

public class MusicManager : MonoSingleton<MusicManager>
{
    public AudioClip BGM_Main;
    public AudioClip BGM_Combat;

    public AudioSource audioSource;

    public void PlayBGM_Main()
    {
        return;
        audioSource.clip = BGM_Main;
        audioSource.Play();
    }

    public void PlayBGM_Combat()
    {
        return;
        audioSource.clip = BGM_Combat;
        audioSource.Play();
    }
}
