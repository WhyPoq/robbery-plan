using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlacementUI : MonoBehaviour
{
    public PlacementManager placementManager;

    public List<Button> buttons = new();

    public Color normalDeactCol;
    public Color hoverDeactCol;
    public Color normalActCol;
    public Color hoverActCol;
    public Color pressedActCol;



    private void Awake()
    {
        ActivateButton((int)placementManager.curTool);
    }

    private void ActivateButton(int n)
    {
        ColorBlock colors = buttons[n].colors;
        colors.normalColor = normalActCol;
        colors.highlightedColor = hoverActCol;
        colors.pressedColor = pressedActCol;
        buttons[n].colors = colors;
    }

    private void DeactivateButton(int n)
    {
        ColorBlock colors = buttons[n].colors;
        colors.normalColor = normalDeactCol;
        colors.highlightedColor = hoverDeactCol;
        colors.pressedColor = normalActCol;
        buttons[n].colors = colors;
    }

    public void SetTool(int tool)
    {
        if (GameManager.gamePaused) return;

        DeactivateButton((int)placementManager.curTool);
        placementManager.curTool = (Tool)tool;
        ActivateButton(tool);
    }
}
