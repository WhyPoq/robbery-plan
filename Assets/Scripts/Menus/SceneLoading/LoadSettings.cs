using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class LoadSettings : MonoBehaviour
{
    public AudioMixer audioMixer;

    private void Start()
    {
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            audioMixer.SetFloat("musicVolume", PlayerPrefs.GetFloat("musicVolume"));
        }
        if (PlayerPrefs.HasKey("soundsVolume"))
        {
            audioMixer.SetFloat("soundsVolume", PlayerPrefs.GetFloat("soundsVolume"));
        }

        if (PlayerPrefs.HasKey("isFullscreen"))
        {
            int isFullscreen = PlayerPrefs.GetInt("isFullscreen");
            if (isFullscreen == 1)
            {
                Screen.fullScreen = true;
            }
            else
            {
                Screen.fullScreen = false;
            }
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }
}
