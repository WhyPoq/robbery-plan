using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Robot : MonoBehaviour
{
    public UnityEvent moneyChanged = new();

    public int money = 0;

    public void AddMoney(int amount)
    {
        money += amount;
        moneyChanged.Invoke();
    }

    public void ResetMoney()
    {
        money = 0;
        moneyChanged.Invoke();
    }
}
