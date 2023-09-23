using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public enum Transitions
{
    fade,
    fadeText
}

public class LoadingScreen : MonoBehaviour
{
    public UnityEvent started = new();
    public UnityEvent ended = new();

    [SerializeField]
    List<GameObject> transitionsObjects = new();

    Transition curTransition = null;

    public void StartLoading(Transitions transition)
    {
        if (curTransition != null)
        {
            Destroy(curTransition.gameObject);
        }

        GameObject transitionObj = Instantiate(transitionsObjects[(int)transition], transform);
        curTransition = transitionObj.GetComponent<Transition>();

        curTransition.stared.AddListener(TransitionStarted);
        curTransition.ended.AddListener(TransitionEnded);
        curTransition.StartLoading();
    }

    public void Unloaded()
    {
        if (curTransition != null)
        {
            curTransition.Unloaded();
        }
    }

    public void EndLoading()
    {
        if (curTransition != null)
        {
            curTransition.EndLoading();
        }
    }

    private void TransitionStarted()
    {
        started.Invoke();
    }

    private void TransitionEnded()
    {
        if (curTransition != null)
        {
            Destroy(curTransition.gameObject);
        }
        ended.Invoke();
    }

    public void RemoveCamera()
    {
        if (curTransition != null)
        {
            curTransition.RemoveCamera();
        }
    }
}
