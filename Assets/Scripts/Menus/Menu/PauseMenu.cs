using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;

    [Scene] [SerializeField]
    private string mainMenuScene;

    [SerializeField]
    private GameObject menuObj;

    public void LoadMainMenu()
    {
        SceneLoader.instance.LoadScene(mainMenuScene, Transitions.fadeText);
    }

    public void Open()
    {
        gameManager.PauseGame();
        menuObj.SetActive(true);
    }

    public void Close()
    {
        gameManager.UnpauseGame();
        menuObj.SetActive(false);
    }
}
