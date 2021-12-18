using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoredDataTile {

    public Vector3Int position;
    public StoredDataTile parent;
    public bool visited = false;
    public int landSpeed;
    public float distance;
    public int remainMovement;

}
