using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public struct DoorInfo{
    public Tile closed;
    public Tile opened;
    public int orientation;
}

public class TileInformation : MonoBehaviour
{
    public static TileInformation instance;

    public List<DoorInfo> doorInfos;
    public List<DoorInfo> windowInfos;
    public Tilemap WallTiles;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

    }
}
