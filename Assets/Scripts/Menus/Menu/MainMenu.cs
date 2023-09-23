using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    [Scene] [SerializeField]
    private string gameScene1;

    [Scene]
    [SerializeField]
    private string gameScene2;

    private void Awake()
    {
        Time.timeScale = 1;
    }

    public void PlayGame()
    {
        /*if (PlayerPrefs.HasKey("curLevel"))
        {
            string c = PlayerPrefs.GetString("curLevel");
            if(c == "level1")
            {
                SceneLoader.instance.LoadScene(gameScene1, Transitions.fadeText);
            }
            else if(c == "level1")
            {
                SceneLoader.instance.LoadScene(gameScene2, Transitions.fadeText);
            }
            else
            {
                SceneLoader.instance.LoadScene(gameScene2, Transitions.fadeText);
            }
        }
        else{
            SceneLoader.instance.LoadScene(gameScene1, Transitions.fadeText);
        }*/
        SceneLoader.instance.LoadScene(gameScene1, Transitions.fadeText);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }
}
