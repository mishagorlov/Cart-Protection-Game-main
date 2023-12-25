using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionsScript : MonoBehaviour
{

    public Slider musicSlider, sfxSlider;
    public AudioMixer audioMixerMusic, sfxMixer;

    private void OnEnable()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");
    }

    public void SetMusicLevel(float volumeValue)
    {
        audioMixerMusic.SetFloat("MusicVolume", Mathf.Log10(volumeValue) * 20);
        PlayerPrefs.SetFloat("musicVolume", volumeValue);
        Debug.Log(volumeValue);
    }

    public void SetSFXLevel(float volumeValue)
    {
        sfxMixer.SetFloat("SFXVolume", Mathf.Log10(volumeValue) * 20);
        sfxSlider.value = volumeValue;
        PlayerPrefs.SetFloat("sfxVolume", volumeValue);
    }
}
