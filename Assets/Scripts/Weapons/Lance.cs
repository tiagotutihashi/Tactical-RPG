using System.Collections.Generic;
using UnityEngine;

public class Lance : WeaponBase
{
    public override List<Vector2Int> GetAttackedTiles(Vector2Int targetTile, Vector2Int targetTileDirection)
    {
        return new List<Vector2Int>
        {
            targetTile,
            targetTile + targetTileDirection
        };
    }
}
