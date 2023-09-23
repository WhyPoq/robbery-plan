using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicChanger : MonoBehaviour
{
    [SerializeField]
    private bool offMusic = false;

    [SerializeField]
    private string newMusic;

    private void Start()
    {
        if (offMusic)
        {
            AudioManager.instance.TurnOffMusic();
        }
        else
        {
            AudioManager.instance.PlayMusic(newMusic);
        }
    }


}
