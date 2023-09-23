using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Pathfinding;

public struct SavedTile
{
    public Vector3Int pos;
    public Tile tile;

    public SavedTile(Vector3Int pos, Tile tile)
    {
        this.pos = pos;
        this.tile = tile;
    }
}

public class MapMemory : MonoBehaviour
{
    public static MapMemory instance;

    List<SavedTile> savedTiles = new List<SavedTile>();
    HashSet<Vector3Int> modifiedPoses = new HashSet<Vector3Int>();

    List<Valuable> valsToReset = new();

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            if(instance != this)
            {
                Destroy(instance.gameObject);
                instance = this;
            }
        }
    }

    public void Modified(Vector3Int pos, Tile tile)
    {
        if (!modifiedPoses.Contains(pos))
        {
            modifiedPoses.Add(pos);
            savedTiles.Add(new SavedTile(pos, tile));
        }
    }

    public void LoadTiles()
    {
        foreach (SavedTile savedTile in savedTiles)
        {
            TileInformation.instance.WallTiles.SetTile(savedTile.pos, savedTile.tile);

            Vector3 center = TileInformation.instance.WallTiles.transform.
                TransformPoint(TileInformation.instance.WallTiles.GetBoundsLocal(savedTile.pos).max);
            Vector3 size = TileInformation.instance.WallTiles.transform.
                TransformPoint(TileInformation.instance.WallTiles.GetBoundsLocal(savedTile.pos).max + TileInformation.instance.WallTiles.GetBoundsLocal(savedTile.pos).size);
            GraphUpdateObject guo = new GraphUpdateObject(new Bounds(center, size));

            AstarPath.active.UpdateGraphs(guo, 0.03f);
        }
        savedTiles.Clear();
        modifiedPoses.Clear();
    }

    public void SaveValuable(Valuable val)
    {
        valsToReset.Add(val);
    }

    public void ResetValuables()
    {
        foreach(Valuable val in valsToReset)
        {
            val.ResetVal();
        }
        valsToReset.Clear();
    }
}
