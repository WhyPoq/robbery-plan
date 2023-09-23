using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;

    public Slider musicVolumeSlider;
    public Slider soundsVolumeSlider;

    public Toggle fullscreenToggle;


    public void Awake()
    {
        LoadVisuals();
    }

    public void OnEnable()
    {
        LoadVisuals();
    }

    private void LoadVisuals()
    {
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            musicVolumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
        }
        if (PlayerPrefs.HasKey("soundsVolume"))
        {
            soundsVolumeSlider.value = PlayerPrefs.GetFloat("soundsVolume");
        }

        fullscreenToggle.isOn = Screen.fullScreen;
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("musicVolume", volume);
        PlayerPrefs.SetFloat("musicVolume", volume);

        if (Random.Range(0, 2) == 0)
        {
            AudioManager.instance.Play("Mouse1");
        }
        else
        {
            AudioManager.instance.Play("Mouse2");
        }
    }

    public void SetSoundsVolume(float volume)
    {
        audioMixer.SetFloat("soundsVolume", volume);
        PlayerPrefs.SetFloat("soundsVolume", volume);

        if (Random.Range(0, 2) == 0)
        {
            AudioManager.instance.Play("Mouse1");
        }
        else
        {
            AudioManager.instance.Play("Mouse2");
        }
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        if(isFullscreen)
            PlayerPrefs.SetInt("isFullscreen", 1);
        else
            PlayerPrefs.SetInt("isFullscreen", 0);
    }

    private void OnDisable()
    {
        PlayerPrefs.Save();
    }
}
