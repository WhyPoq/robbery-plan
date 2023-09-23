using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using TMPro;

public enum Tool{
    move,
    interact,
    wait,
    erase,
    edit
}

public class PlacementManager : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    [SerializeField]
    private GameObject pathObj;

    private Path curPath;

    [SerializeField]
    private GameObject moveCommandObj;
    [SerializeField]
    private GameObject timeCommandObj;
    [SerializeField]
    private GameObject doorCommandObjH;
    [SerializeField]
    private GameObject doorCommandObjV;
    [SerializeField]
    private GameObject windowCommandObjH;
    [SerializeField]
    private GameObject windowCommandObjV;
    [SerializeField]
    private GameObject pickCommandObj;
    [SerializeField]
    private GameObject selectCarCommandObj;

    public Tool curTool = Tool.interact;

    GameObject selectedObj = null;
    WaitUI selectedTime = null;

    [SerializeField]
    private LayerMask valLayer;
    [SerializeField]
    private LayerMask iconLayer;

    public GameManager gameManager;

    public BoxCollider2D placeableArea;

    public int movesLimit;
    public TextMeshProUGUI movesLeftText;

    Command pulsatingCommand;

    void Awake()
    {
        curPath = pathObj.GetComponent<Path>();
        movesLeftText.text = movesLimit.ToString();
    }


    void Update()
    {
        if((curTool != Tool.wait && curTool != Tool.edit && curTool != Tool.erase) || gameManager.isPlaying)
        {
            if(pulsatingCommand != null)
            {
                pulsatingCommand.EndPulsate();
                pulsatingCommand = null;
            }
        }

        if (curTool != Tool.wait && selectedTime != null)
        {
            selectedTime.Hide();
            selectedTime = null;
        }

        if (gameManager.isPlaying) return;
        if (GameManager.gamePaused) return;

        movesLeftText.text = (movesLimit - curPath.actions.Count).ToString();

        if (curTool != Tool.edit || Input.GetMouseButtonUp(0))
        {
            selectedObj = null;
        }


        if (selectedObj != null)
        {
            Vector3 pos = cam.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;
            selectedObj.transform.position = pos;
        }

        if(curTool == Tool.wait)
        {
            RaycastHit2D hit = Physics2D.Raycast(cam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 10, iconLayer);
            if ((hit.collider != null && hit.collider.GetComponent<WaitUI>() != null))
            {
                Command hitCommand = hit.collider.GetComponent<Command>();
                if(selectedTime != null && hitCommand.gameObject == selectedTime.gameObject)
                {
                    if (pulsatingCommand != null)
                    {
                        pulsatingCommand.EndPulsate();
                        //selectedTime = null;
                    }
                }
                else if (pulsatingCommand == null || hitCommand != pulsatingCommand)
                {
                    if (pulsatingCommand != null)
                    {
                        pulsatingCommand.EndPulsate();
                    }
                    pulsatingCommand = hitCommand;
                    pulsatingCommand.StartPulsate();
                }

            }
            else
            {
                if (pulsatingCommand != null)
                {
                    pulsatingCommand.EndPulsate();
                    pulsatingCommand = null;
                }
            }
        }

        if (curTool == Tool.edit)
        {
            RaycastHit2D hit = Physics2D.Raycast(cam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 10, iconLayer);
            if ((hit.collider != null && hit.collider.GetComponent<Command>() != null) && selectedObj == null)
            {
                Command hitCommand = hit.collider.GetComponent<Command>();
                if (pulsatingCommand == null || hitCommand != pulsatingCommand)
                {
                    if (pulsatingCommand != null)
                    {
                        pulsatingCommand.EndPulsate();
                    }
                    pulsatingCommand = hitCommand;
                    pulsatingCommand.StartPulsate();
                }
            }
            else
            {
                if (pulsatingCommand != null)
                {
                    pulsatingCommand.EndPulsate();
                    pulsatingCommand = null;
                }
            }
        }

        if (curTool == Tool.erase)
        {
            RaycastHit2D hit = Physics2D.Raycast(cam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 10, iconLayer);
            if (hit.collider != null && hit.collider.GetComponent<Command>() != null)
            {
                Command hitCommand = hit.collider.GetComponent<Command>();
                if (pulsatingCommand == null || hitCommand != pulsatingCommand)
                {
                    if (pulsatingCommand != null)
                    {
                        pulsatingCommand.EndPulsate();
                    }
                    pulsatingCommand = hitCommand;
                    pulsatingCommand.StartPulsate();
                }
            }
            else
            {
                if (pulsatingCommand != null)
                {
                    pulsatingCommand.EndPulsate();
                    pulsatingCommand = null;
                }
            }
        }

        if (Input.GetMouseButtonDown(0)) //im returning out of this btw
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                Vector3 clickPos = cam.ScreenToWorldPoint(Input.mousePosition);
                clickPos.z = 0;
                if (!placeableArea.bounds.Contains(clickPos))
                {
                    return;
                }

                if (curTool == Tool.move)
                {
                    if (curPath.actions.Count < movesLimit)
                    {
                        GameObject g = Instantiate(moveCommandObj, pathObj.transform);
                        AudioManager.instance.Play("Placed");
                        g.transform.position = clickPos;
                        curPath.actions.Add(g.GetComponent<PointAction>());
                        g.GetComponent<Command>().SetNumber(curPath.actions.Count);
                    }
                }
                else if (curTool == Tool.interact)
                {
                    if (curPath.actions.Count >= movesLimit) return;

                    Vector3Int pos = TileInformation.instance.WallTiles.WorldToCell(clickPos);
                    TileBase t = TileInformation.instance.WallTiles.GetTile(pos);

                    RaycastHit2D hit = Physics2D.Raycast(cam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 10, valLayer);
                    if (t == null || (hit.collider != null && (hit.collider.GetComponent<Valuable>() || hit.collider.tag == "Car")))
                    {
                        if (hit.collider != null && hit.collider.GetComponent<Valuable>() != null)
                        {
                            GameObject g = Instantiate(pickCommandObj, pathObj.transform);
                            AudioManager.instance.Play("Placed");
                            g.GetComponent<WalkPickAction>().thing = hit.collider.GetComponent<Valuable>();
                            g.transform.position = hit.collider.transform.position;
                            curPath.actions.Add(g.GetComponent<PointAction>());
                            g.GetComponent<Command>().SetNumber(curPath.actions.Count);
                        }
                        else if (hit.collider != null && hit.collider.tag == "Car")
                        {
                            GameObject g = Instantiate(selectCarCommandObj, pathObj.transform);
                            AudioManager.instance.Play("Placed");
                            g.transform.position = hit.collider.transform.position;
                            g.transform.rotation = hit.collider.transform.rotation;
                            g.GetComponent<CarUI>().Placed(hit.collider.transform.eulerAngles.z);
                            g.GetComponent<EscapeAction>().gameManager = gameManager;
                            curPath.actions.Add(g.GetComponent<PointAction>());
                            g.GetComponent<Command>().SetNumber(curPath.actions.Count);
                        }
                    }
                    else
                    {
                        Vector3 tilePos = TileInformation.instance.WallTiles.CellToWorld(pos) + TileInformation.instance.WallTiles.cellSize / 2f;
                        foreach (DoorInfo info in TileInformation.instance.doorInfos)
                        {
                            if (t == info.closed || t == info.opened)
                            {
                                GameObject g;
                                if (info.orientation == 0 || info.orientation == 2)
                                {
                                    g = Instantiate(doorCommandObjH, pathObj.transform);
                                }
                                else
                                {
                                    g = Instantiate(doorCommandObjV, pathObj.transform);
                                }
                                AudioManager.instance.Play("Placed");
                                g.transform.position = tilePos;
                                curPath.actions.Add(g.GetComponent<PointAction>());
                                g.GetComponent<Command>().SetNumber(curPath.actions.Count);
                                return;
                            }
                        }
                        foreach (DoorInfo info in TileInformation.instance.windowInfos)
                        {
                            if (t == info.closed || t == info.opened)
                            {
                                GameObject g;
                                if (info.orientation == 0 || info.orientation == 2)
                                {
                                    g = Instantiate(windowCommandObjH, pathObj.transform);
                                }
                                else
                                {
                                    g = Instantiate(windowCommandObjV, pathObj.transform);
                                }
                                AudioManager.instance.Play("Placed");
                                g.transform.position = tilePos;
                                curPath.actions.Add(g.GetComponent<PointAction>());
                                g.GetComponent<Command>().SetNumber(curPath.actions.Count);
                                return;
                            }
                        }
                    }
                }
                else if (curTool == Tool.wait)
                {
                    RaycastHit2D hit = Physics2D.Raycast(cam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 10, iconLayer);
                    if (selectedTime != null)
                    {
                        selectedTime.Hide();
                    }
                    if (hit.collider != null && hit.collider.GetComponent<WaitUI>() != null)
                    {
                        selectedTime = hit.collider.GetComponent<WaitUI>();
                        selectedTime.Show();
                        AudioManager.instance.Play("Mouse1");
                    }
                    else
                    {
                        if (curPath.actions.Count < movesLimit)
                        {
                            GameObject g = Instantiate(timeCommandObj, pathObj.transform);
                            AudioManager.instance.Play("Placed");
                            g.transform.position = clickPos;
                            curPath.actions.Add(g.GetComponent<PointAction>());
                            g.GetComponent<Command>().SetNumber(curPath.actions.Count);
                            selectedTime = g.GetComponent<WaitUI>();
                            selectedTime.gameManager = gameManager;
                            selectedTime.Show();
                        }
                    }
                }
                else if (curTool == Tool.erase)
                {
                    RaycastHit2D hit = Physics2D.Raycast(cam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 10, iconLayer);
                    if (hit.collider != null && hit.collider.GetComponent<Command>() != null)
                    {
                        curPath.actions.Remove(hit.collider.GetComponent<PointAction>());
                        RenumberActions();
                        AudioManager.instance.Play("Erased");
                        Destroy(hit.collider.gameObject);
                    }
                }
                else if (curTool == Tool.edit)
                {
                    RaycastHit2D hit = Physics2D.Raycast(cam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 10, iconLayer);
                    if (hit.collider != null && hit.collider.GetComponent<Command>() != null)
                    {
                        if(hit.collider.GetComponent<WalkAction>() != null || hit.collider.GetComponent<WalkWaitAction>() != null)
                        selectedObj = hit.collider.gameObject;
                        AudioManager.instance.Play("Mouse2");
                    }
                }
            }
        }
    }

    public void RenumberActions()
    {
        for(int i = 0; i < curPath.actions.Count; i++)
        {
            curPath.actions[i].GetComponent<Command>().SetNumber(i + 1);
        }
    }
}
