using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;


public class WalkWaitAction : PointAction
{
    public float waitTime = 1;
    private GameObject targetObj;
    private bool stopped = true;

    public override void Execute(GameObject target)
    {
        stopped = false;
        target.GetComponent<AILerp>().destination = transform.position;
        target.GetComponent<AIWalker>().timer = 0;
        target.GetComponent<AIWalker>().reached.AddListener(PointReached);

        targetObj = target;
    }

    private void PointReached()
    {
        targetObj.GetComponent<AIWalker>().reached.RemoveListener(PointReached);
        targetObj.GetComponent<AILerp>().destination = targetObj.transform.position;
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(waitTime);
        if (!stopped)
        {
            executed.Invoke();
        }
    }

    public override void StopExecuting(GameObject target)
    {
        stopped = true;
        targetObj.GetComponent<AIWalker>().reached.RemoveListener(PointReached);
        StopCoroutine(Wait());
    }
}
