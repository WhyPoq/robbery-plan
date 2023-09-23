using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayButtons : MonoBehaviour
{
    public GameManager gameManager;
    public Button playButton;

    public Sprite playSprite;
    public Sprite stopSprite;

    public TextMeshProUGUI speedText;


    private bool toPlaying = true;

    private bool speeded = false;

    private void Awake()
    {
        Time.timeScale = 1;
        speedText.text = "x1";
    }

    public void ActionPressed()
    {
        if (GameManager.gamePaused) return;

        if (toPlaying) { 
            gameManager.Play();
            playButton.GetComponent<Image>().sprite = stopSprite;
        }
        else
        {
            gameManager.Stop();
            playButton.GetComponent<Image>().sprite = playSprite;
        }

        toPlaying = !toPlaying;
    }

    public void SpeedPressed()
    {
        if (GameManager.gamePaused) return;
        if (gameManager.gameSpeed == 1)
        {
            speedText.text = "x3";
            gameManager.gameSpeed = 3;
        }
        else
        {
            speedText.text = "x1";
            gameManager.gameSpeed = 1;
        }
        Time.timeScale = gameManager.gameSpeed;
    }
}
