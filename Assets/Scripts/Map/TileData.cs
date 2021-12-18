using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "TileObject", menuName = "TileData")]
public class TileData : ScriptableObject {

    [SerializeField]
    private TileBase[] tiles;
    [SerializeField]
    [Range(0, 10)]
    private int walkingSpeed;

    public int WalkingSpeed => walkingSpeed;
    public TileBase[] Tiles => tiles;

}
