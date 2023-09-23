using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Pathfinding;

public class CloseDorAction : PointAction
{
    private float delay = 1.5f;
    GameObject targetObj;

    public override void Execute(GameObject target)
    {
        targetObj = target;
        Vector3Int pos = TileInformation.instance.WallTiles.WorldToCell(transform.position);
        TileBase t = TileInformation.instance.WallTiles.GetTile(pos);
        executed.Invoke();
        ClosenDoor(t, pos);
    }

    private void ClosenDoor(TileBase t, Vector3Int pos)
    {
        foreach (DoorInfo info in TileInformation.instance.doorInfos)
        {
            if (t == info.opened)
            {
                StartCoroutine(LongWait(t, pos, info));
                return;
            }
        }
    }

    private IEnumerator LongWait(TileBase t, Vector3Int pos, DoorInfo info)
    {
        yield return new WaitForSeconds(delay);
        TileInformation.instance.WallTiles.SetTile(pos, info.closed);
        if (targetObj.GetComponent<Robot>() != null)
        {
            AudioManager.instance.Play("DoorClosed");
        }

        Vector3 center = TileInformation.instance.WallTiles.transform.
            TransformPoint(TileInformation.instance.WallTiles.GetBoundsLocal(pos).max);
        Vector3 size = TileInformation.instance.WallTiles.transform.
            TransformPoint(TileInformation.instance.WallTiles.GetBoundsLocal(pos).size);
        GraphUpdateObject guo = new GraphUpdateObject(new Bounds(center, size));

        AstarPath.active.UpdateGraphs(guo, 0.03f);
    }

    public override void StopExecuting(GameObject target)
    {
        StopCoroutine(LongWait(null, Vector3Int.zero, new DoorInfo()));
    }
}
