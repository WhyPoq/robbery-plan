using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PointAction : MonoBehaviour
{
    public UnityEvent executed;

    public virtual void Execute(GameObject target)
    {
        executed.Invoke();
    }

    public virtual void StopExecuting(GameObject target)
    {

    }
}
