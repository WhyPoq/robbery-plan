using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;


public class WaitAction : PointAction
{
    public float waitTime = 1;
    private bool stopped = true;

    public override void Execute(GameObject target)
    {
        stopped = false;
        target.GetComponent<AILerp>().destination = target.transform.position;
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
        StopCoroutine(Wait());

    }
}
