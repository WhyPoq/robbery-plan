using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Transition : MonoBehaviour
{
    [HideInInspector]
    public UnityEvent stared = new();
    [HideInInspector]
    public UnityEvent ended = new();

    public virtual void StartLoading()
    {
        Debug.Log("Start loading screen");
        stared.Invoke();
    }

    public virtual void Unloaded()
    {
        Debug.Log("Previous scene was unloaded");
    }

    public virtual void EndLoading()
    {
        Debug.Log("End loading screen");
        ended.Invoke();
    }

    public virtual void RemoveCamera()
    {
        Debug.Log("Camera was removed");
    }
}
