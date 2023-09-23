using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    private LoadingScreen loadingScreen;

    [Scene] [SerializeField]
    private string sceneLoaderScene;

    [Scene] [SerializeField]
    private string startScene;

    public static SceneLoader instance = null;

    private string sceneToLoad;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("More than one instance of sceneLoader are presented");
            Destroy(gameObject);
            return;
        }

        loadingScreen.started.AddListener(LoadStarted);
        loadingScreen.ended.AddListener(LoadEnded);

        if (SceneManager.sceneCount == 1)
        {
            LoadStartScene();
        }
    }

    private void LoadStartScene()
    {
        SceneManager.LoadScene(startScene, LoadSceneMode.Additive);
    }


    public void LoadScene(string sceneToLoad, Transitions transition = Transitions.fade)
    {
        this.sceneToLoad = sceneToLoad;
        loadingScreen.StartLoading(transition);
    }

    private void LoadStarted()
    {
        int countLoaded = SceneManager.sceneCount;
        List<AsyncOperation> unloadOperations = new();
        for (int i = 0; i < countLoaded; i++)
        {
            Scene curScene = SceneManager.GetSceneAt(i);
            if (curScene.name != sceneLoaderScene)
            {
                unloadOperations.Add(SceneManager.UnloadSceneAsync(curScene));
            }
        }
        StartCoroutine(GetUnloadingProgress(unloadOperations));

        AsyncOperation curLoadingOperation = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
        StartCoroutine(GetLoadingProgress(curLoadingOperation));
    }

    public IEnumerator GetUnloadingProgress(List<AsyncOperation> unloadOperations)
    {
        foreach(AsyncOperation operation in unloadOperations) {
            while (!operation.isDone)
            {
                yield return null;
            }
        }
        loadingScreen.Unloaded();

    }

    public IEnumerator GetLoadingProgress(AsyncOperation curLoadingScene)
    {
        while (!curLoadingScene.isDone)
        {
            yield return null;
        }

        loadingScreen.EndLoading();
    }

    private void LoadEnded()
    {
        
    }

    public void RemoveTransitionCamera()
    {
        loadingScreen.RemoveCamera();
    }
}
