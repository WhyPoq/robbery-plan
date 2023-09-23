using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class TriggerButtonPress : MonoBehaviour
{
    public Button button;

    public UnityEvent buttonPressed;

    private void OnEnable()
    {
        button.onClick.AddListener(ButtonPressed);
    }


    private void OnDisable()
    {
        button.onClick.RemoveListener(ButtonPressed);
    }

    private void ButtonPressed()
    {
        buttonPressed.Invoke();
    }
}
