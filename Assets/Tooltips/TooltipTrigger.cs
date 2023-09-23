using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string header;
    public string content;

    Coroutine lastRoutine = null;

    public void OnPointerEnter(PointerEventData eventData)
    {
        lastRoutine = StartCoroutine(Wait());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipSystem.Hide();
        StopCoroutine(lastRoutine);
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(TooltipSystem.instance.tooltipDelay);
        TooltipSystem.Show(content, header);
    }
}
