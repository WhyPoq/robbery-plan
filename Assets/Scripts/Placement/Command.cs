using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Command : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI textMeshPro;

    Vector3 startSize;
    bool pulsating = false;

    bool canPulsate = true;

    private void Awake()
    {
        startSize = transform.localScale;
        if(GetComponent<CarUI>() != null)
        {
            canPulsate = false;
        }
    }

    public void SetNumber(int number)
    {
        textMeshPro.text = number.ToString();
    }

    public void StartPulsate()
    {
        pulsating = true;
    }

    public void EndPulsate()
    {
        pulsating = false;
        transform.localScale = startSize;
    }

    private void Update()
    {
        if (pulsating && canPulsate)
        {
            float curSize = 1f + Mathf.Sin(Time.time * 7) / 15f;
            transform.localScale = new Vector3(curSize, curSize, curSize);
        }
    }
}
