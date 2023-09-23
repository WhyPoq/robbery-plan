using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Pathfinding;

public class DoorAction : PointAction
{
    GameObject targetObj;

    public override void Execute(GameObject target)
    {
        targetObj = target;
        Vector3Int pos = TileInformation.instance.WallTiles.WorldToCell(transform.position);
        TileBase t = TileInformation.instance.WallTiles.GetTile(pos);
        OpenDoor(t, pos);
    }

    private void OpenDoor(TileBase t, Vector3Int pos)
    {
        foreach(DoorInfo info in TileInformation.instance.doorInfos)
        {
            if(t == info.closed)
            {
                TileInformation.instance.WallTiles.SetTile(pos, info.opened);
                if (targetObj.GetComponent<Robot>() != null)
                {
                    AudioManager.instance.Play("DoorOpened");
                }
                MapMemory.instance.Modified(pos, info.closed);

                Vector3 center = TileInformation.instance.WallTiles.transform.
                    TransformPoint(TileInformation.instance.WallTiles.GetBoundsLocal(pos).max);
                Vector3 size = TileInformation.instance.WallTiles.transform.
                    TransformPoint(TileInformation.instance.WallTiles.GetBoundsLocal(pos).size);
                GraphUpdateObject guo = new GraphUpdateObject(new Bounds(center, size));

                AstarPath.active.UpdateGraphs(guo, 0.03f);
                StartCoroutine(Wait());
                return;
            }
        }
        executed.Invoke();
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.03f);
        executed.Invoke();
    }
}
