using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class WalkAction : PointAction
{
    private GameObject targetObj;

    public override void Execute(GameObject target)
    {
        target.GetComponent<AILerp>().destination = transform.position;
        target.GetComponent<AIWalker>().timer = 0;
        target.GetComponent<AIWalker>().reached.AddListener(PointReached);

        targetObj = target;
    }

    private void PointReached()
    {
        targetObj.GetComponent<AIWalker>().reached.RemoveListener(PointReached);
        executed.Invoke();
    }

    public override void StopExecuting(GameObject target)
    {
        targetObj.GetComponent<AIWalker>().reached.RemoveListener(PointReached);
        target.GetComponent<AILerp>().destination = target.transform.position;
    }

}
