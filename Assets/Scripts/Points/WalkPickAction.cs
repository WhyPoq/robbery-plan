using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class WalkPickAction : PointAction
{
    public Valuable thing;
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
        if (thing.isActiveAndEnabled)
        {
            StartCoroutine(Wait());
        }
        else
        {
            executed.Invoke();
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.8f);
        if (!stopped)
        {
            if (Random.Range(0, 2) == 0)
            {
                AudioManager.instance.Play("PickCash1");
            }
            else
            {
                AudioManager.instance.Play("PickCash2");
            }
            thing.Pick();
            MapMemory.instance.SaveValuable(thing);
            executed.Invoke();
        }
    }

    public override void StopExecuting(GameObject target)
    {
        targetObj.GetComponent<AIWalker>().reached.RemoveListener(PointReached);
        stopped = true;
        StopCoroutine(Wait());
    }

}
