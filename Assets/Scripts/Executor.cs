using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Executor : MonoBehaviour
{
    [SerializeField]
    private List<Path> paths = new();

    public int curPath = 0;
    public int curPoint = 0;

    Vector3 startPos;
    Quaternion startRotation;

    public void StartPath()
    {
        startPos = transform.position;
        startRotation = transform.rotation;

        if (paths.Count > 0 && paths[curPath].actions.Count > 0)
        {
            RunPoint();
        }
    }

    public void Stop()
    {
        GetComponent<AIStopLerp>().destination = startPos;
        GetComponent<AIStopLerp>().Teleport(startPos);
        GetComponent<AIStopLerp>().rotation = startRotation;

        if (curPath < paths.Count && curPoint < paths[curPath].actions.Count)
        {
            paths[curPath].actions[curPoint].StopExecuting(gameObject);
            paths[curPath].actions[curPoint].executed.RemoveListener(ActionExecuteed);
        }

        curPath = 0;
        curPoint = 0;
    }

    private void ActionExecuteed()
    {
        paths[curPath].actions[curPoint].executed.RemoveListener(ActionExecuteed);
        curPoint++;
        if (curPoint >= paths[curPath].actions.Count)
        {
            curPoint = 0;
            if (paths[curPath].loop)
            {
                RunPoint();
            }
            else
            {
                curPath++;
                if (curPath < paths.Count)
                {
                    RunPoint();
                }
            }
        }
        else
        {
            RunPoint();
        }
    }

    private void RunPoint()
    {
        paths[curPath].actions[curPoint].executed.AddListener(ActionExecuteed);
        paths[curPath].actions[curPoint].Execute(gameObject);
    }
}
