using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaitUI : MonoBehaviour
{
    public TMP_InputField input;
    public bool active = false;

    WalkWaitAction action;

    public GameManager gameManager;

    private void Start()
    {
        action = GetComponent<WalkWaitAction>();
    }

    private void Update()
    {
        if(GameManager.gamePaused || gameManager.isPlaying)
        {
            Hide();
        }
    }

    public void Show()
    {
        input.gameObject.SetActive(true);
        active = true;
        if(action == null)
        {
            action = GetComponent<WalkWaitAction>();
        }
        input.text = ((int)action.waitTime).ToString();
    }

    public void Hide()
    {
        input.gameObject.SetActive(false);
        active = false;
    }

    public void Edited()
    {
        try
        {
            int t = int.Parse(input.text);
            if (t > 99) t = 99;
            if (t <= 1) t = 1;
            action.waitTime = t;
            input.text = t.ToString();
        }
        catch
        {
            action.waitTime = 1;
            input.text = "1";
        }
    }
}
