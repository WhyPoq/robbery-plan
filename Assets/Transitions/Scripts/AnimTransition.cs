using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class AnimTransition : Transition
{
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private Camera cam;

    public float transitionTime = 1f;

    private int doneState = -1;
    private int curState = 0;
    private int allowedState = -1;

    public override void StartLoading()
    {
        allowedState = 0;
        if (curState <= allowedState && curState > doneState)
        {
            ExecuteState(curState);
        }
    }

    public override void Unloaded()
    {
        if (allowedState < 2)
        {
            if (cam != null)
                cam.gameObject.SetActive(true);
        }
    }

    public override void EndLoading()
    {
        allowedState = 2;
        if (curState <= allowedState && curState > doneState)
        {
            ExecuteState(curState);
        }
    }

    private void ExecuteState(int state)
    {
        if(state == 0) //start "start" animation
        {
            doneState++;
            animator.gameObject.SetActive(true);
            animator.SetTrigger("Start");
            allowedState = 1;
            StartCoroutine(ChangeState(transitionTime));
        }
        else if (state == 1) //end "start" animation
        {
            doneState++;
            stared.Invoke();
            StartCoroutine(ChangeState(transitionTime));
        }
        else if(state == 2) //start "end" animation
        {
            doneState++;
            animator.SetTrigger("End");
            allowedState = 3;
            StartCoroutine(ChangeState(transitionTime));
        }
        else if(state == 3) //end "end" animation
        {
            doneState++;
            ended.Invoke();
            animator.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if(curState <= allowedState && curState > doneState)
        {
            ExecuteState(curState);
        }
    }

    private IEnumerator ChangeState(float waitTime)
    {
        yield return new WaitForSecondsRealtime(waitTime);
        curState++;
    }

    public override void RemoveCamera()
    {
        if (cam != null)
        {
            Destroy(cam.gameObject);
        }
    }

}
