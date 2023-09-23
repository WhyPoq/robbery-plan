using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Pathfinding;

public class WinWalkAction : PointAction
{
    private GameObject targetObj;
    private bool stopped = true;
    private bool ended = true;

    public override void Execute(GameObject target)
    {
        stopped = false;
        ended = false;
        SetWalkable();
        targetObj = target;
        targetObj.GetComponent<AILerp>().destination = transform.position;
        targetObj.GetComponent<AIWalker>().timer = 0;
        targetObj.GetComponent<AIWalker>().reached.AddListener(PointReached);
    }


    private void PointReached()
    {
        targetObj.GetComponent<AILerp>().destination = targetObj.transform.position;
        targetObj.GetComponent<AIWalker>().reached.RemoveListener(PointReached);
        Vector3Int pos = TileInformation.instance.WallTiles.WorldToCell(transform.position);
        TileBase t = TileInformation.instance.WallTiles.GetTile(pos);
        if (!stopped)
        {
            OpenDoor(t, pos);
        }
    }

    private void UpdateArea()
    {
        Vector3Int pos = TileInformation.instance.WallTiles.WorldToCell(transform.position);
        Vector3 center = TileInformation.instance.WallTiles.transform.
                TransformPoint(TileInformation.instance.WallTiles.GetBoundsLocal(pos).max);
        Vector3 size = TileInformation.instance.WallTiles.transform.
            TransformPoint(TileInformation.instance.WallTiles.GetBoundsLocal(pos).size);
        GraphUpdateObject guo = new GraphUpdateObject(new Bounds(center, size));

        AstarPath.active.UpdateGraphs(guo);
    }

    private void SetWalkable()
    {
        Vector3Int pos = TileInformation.instance.WallTiles.WorldToCell(transform.position);
        Vector3 center = TileInformation.instance.WallTiles.transform.
                TransformPoint(TileInformation.instance.WallTiles.GetBoundsLocal(pos).max);
        Vector3 size = TileInformation.instance.WallTiles.transform.
            TransformPoint(TileInformation.instance.WallTiles.GetBoundsLocal(pos).size);
        GraphUpdateObject guo = new GraphUpdateObject(new Bounds(center, size));

        guo.modifyWalkability = true;
        guo.setWalkability = true;
        guo.updatePhysics = false;
        AstarPath.active.UpdateGraphs(guo);
    }

    private void OpenDoor(TileBase t, Vector3Int pos)
    {
        foreach (DoorInfo info in TileInformation.instance.windowInfos)
        {
            if (t == info.closed)
            {
                AudioManager.instance.Play("WindowStart");
                StartCoroutine(WaitToOpen(t, pos, info));
                return;
            }
        }
        UpdateArea();
        ended = true;
        executed.Invoke();
    }

    private IEnumerator WaitToOpen(TileBase t, Vector3Int pos, DoorInfo info)
    {
        yield return new WaitForSeconds(3f);
        if (!stopped)
        {
            AudioManager.instance.Play("WindowEnd");
            TileInformation.instance.WallTiles.SetTile(pos, info.opened);
            MapMemory.instance.Modified(pos, info.closed);

            Vector3 center = TileInformation.instance.WallTiles.transform.
                TransformPoint(TileInformation.instance.WallTiles.GetBoundsLocal(pos).max);
            Vector3 size = TileInformation.instance.WallTiles.transform.
                TransformPoint(TileInformation.instance.WallTiles.GetBoundsLocal(pos).size);
            GraphUpdateObject guo = new GraphUpdateObject(new Bounds(center, size));

            AstarPath.active.UpdateGraphs(guo, 0.03f);
            ended = true;
            StartCoroutine(Wait());
        }
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.03f);
        executed.Invoke();
    }

    public override void StopExecuting(GameObject target)
    {
        stopped = true;
        targetObj.GetComponent<AIWalker>().reached.RemoveListener(PointReached);
        StopCoroutine(Wait());
        StopCoroutine(WaitToOpen(null, Vector3Int.zero, new DoorInfo()));
        if (!ended)
        {
            UpdateArea();
            ended = true;
        }
    }
}
