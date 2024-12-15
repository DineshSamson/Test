using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource buttonClick;
    public AudioSource match_Sound;
    public AudioSource missMatch_Sound;


    private void Awake()
    {
        if(instance == null) 
        {
            instance = this;
        }
    }

    public void PlayButtonClickSound()
    {
        buttonClick.Play();
    }

    public void PlayMatchSound()
    {
        match_Sound.Play();
    }

    public void PlayMissMatchSound()
    {
        missMatch_Sound.Play();
    }

    public void AdjustVolume(float Value)
    {
        PlayerPrefs.SetFloat("Volume", Value);
        buttonClick.volume = Value;
        missMatch_Sound.volume = Value;
        match_Sound.volume = Value;
    }
}
