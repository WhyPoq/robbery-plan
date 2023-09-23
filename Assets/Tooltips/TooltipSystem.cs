using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipSystem : MonoBehaviour
{
    public static TooltipSystem instance;

    public Tooltip tooltip;

    public float tooltipDelay = 0.5f;

    private void Awake()
    {
        if(instance == null || instance != this)
        {
            instance = this;
        }
        else
        {
            if(instance != null)
            {
                Destroy(gameObject);
                return;
            }
        }
    }

    public static void Show(string content, string header = "")
    {
        instance.tooltip.SetText(content, header);
        instance.tooltip.gameObject.SetActive(true);
    }

    public static void Hide()
    {
        instance.tooltip.gameObject.SetActive(false);

    }
}
