using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private List<Executor> executors;

    [SerializeField]
    private List<CameraMovement> cameras;

    [SerializeField]
    private Robot robot;

    public GameObject hideInPlay;

    public bool isPlaying = false;

    public int targetMoney = 500;
    public GameObject loseScreen;
    public TextMeshProUGUI loseScore;
    public GameObject winScreen;
    public TextMeshProUGUI winScore;
    public GameObject caughtScreen;
    public TextMeshProUGUI caughtScore;

    public PlayButtons playButtons;

    public static bool gamePaused = false;

    public float gameSpeed = 1;

    public string loadName = "level";

    [Scene]
    public string nextLevelScene;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!gamePaused)
                PauseGame();
            else
                UnpauseGame();
        }
    }

    public void Play()
    {
        if (gamePaused) return;

        isPlaying = true;
        for (int i = 0; i < executors.Count; i++)
        {
            executors[i].StartPath();
        }

        for (int i = 0; i < cameras.Count; i++)
        {
            cameras[i].Play();
        }

        hideInPlay.SetActive(false);
    }

    public void Stop()
    {
        if (gamePaused) return;

        isPlaying = false;
        for (int i = 0; i < executors.Count; i++)
        {
            executors[i].Stop();
        }

        for (int i = 0; i < cameras.Count; i++)
        {
            cameras[i].Stop();
        }
        MapMemory.instance.LoadTiles();
        MapMemory.instance.ResetValuables();
        robot.ResetMoney();
        hideInPlay.SetActive(true);
    }

    public void Escaped()
    {
        if (gamePaused) return;

        PauseGame();
        if (robot.money >= targetMoney)
        {
            winScreen.SetActive(true);
            winScore.text = robot.money.ToString();
        }
        else
        {
            loseScreen.SetActive(true);
            loseScore.text = robot.money.ToString();
        }
    }

    public void GotCaught()
    {
        if (gamePaused) return;

        AudioManager.instance.Play("GotCaught");
        PauseGame();
        caughtScreen.SetActive(true);
        caughtScore.text = robot.money.ToString();
    }

    public void PauseGame()
    {
        gamePaused = true;
        Time.timeScale = 0;
    }

    public void UnpauseGame()
    {
        gamePaused = false;
        Time.timeScale = gameSpeed;
    }

    public void NextLevel()
    {
        PlayerPrefs.SetString("curLevel", loadName);
        UnpauseGame();
        SceneLoader.instance.LoadScene(nextLevelScene, Transitions.fadeText);
    }

    public void Again()
    {
        UnpauseGame();
        playButtons.ActionPressed();
        loseScreen.SetActive(false);
        caughtScreen.SetActive(false);
    }
}
