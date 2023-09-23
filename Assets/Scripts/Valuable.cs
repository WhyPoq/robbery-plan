using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Valuable : MonoBehaviour
{
    public Robot robot;
    public int value = 100;

    public void Pick()
    {
        robot.AddMoney(value);
        gameObject.SetActive(false);
    }

    public void ResetVal()
    {
        gameObject.SetActive(true);
    }
}
