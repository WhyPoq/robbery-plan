using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EscapeAction : PointAction
{
    private GameObject targetObj;
    public GameManager gameManager;

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
        AudioManager.instance.Play("CarDriving");
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f);
        if (!stopped)
        {
            gameManager.Escaped();
            executed.Invoke();
        }
    }

    public override void StopExecuting(GameObject target)
    {
        targetObj.GetComponent<AIWalker>().reached.RemoveListener(PointReached);
        StopCoroutine(Wait());
        stopped = true;
    }

}
