using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ClickSound : MonoBehaviour
{
    private string[] sounds = { "Mouse1", "Mouse2" };

    public void Awake()
    {
        GetComponent<Button>().onClick.AddListener(Play);
    }

    public void Play()
    {
        int ind = Random.Range(0, sounds.Length);
        AudioManager.instance.Play(sounds[ind]);
    }
}
