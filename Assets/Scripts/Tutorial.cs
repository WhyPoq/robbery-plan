using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tutorial : MonoBehaviour
{
    public List<GameObject> states = new List<GameObject>();

    int curState = 0;

    private void Awake()
    {
        states[curState].SetActive(true);
    }

    public void Next()
    {
        if (curState + 1 >= states.Count)
        {
            gameObject.SetActive(false);
        }
        else
        {
            ChangeState();
        }
    }

    private void ChangeState()
    {
        states[curState].SetActive(false);
        curState++;
        states[curState].SetActive(true);
    }

}
