using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomProperites
{
    public GameObject Prefab;
    public Vector2 Scale;
    public Vector2[] entryPoints = new Vector2[0];
    public int generateAfter = 0;
    public RoomProperites(Vector2 Scale, Vector2[] entryPoints, GameObject prefab)
    {
        this.Prefab = prefab;
        this.Scale = Scale;
        this.entryPoints = entryPoints;
    }
}
