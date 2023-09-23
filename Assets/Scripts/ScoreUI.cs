using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    public GameManager gameManager;
    public TextMeshProUGUI myMoney;
    public TextMeshProUGUI needMoney;
    public Robot robot;

    private void Awake()
    {
        needMoney.text = "/" + gameManager.targetMoney.ToString();
        UpdateMoney();
        robot.moneyChanged.AddListener(UpdateMoney);
    }

    private void UpdateMoney()
    {
        myMoney.text = robot.money.ToString();
    }
}
