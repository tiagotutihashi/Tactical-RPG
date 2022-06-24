using System.Collections.Generic;
using UnityEngine;

public class Sword : WeaponBase
{
    public override List<Vector2Int> GetAttackedTiles(Vector2Int targetTile, Vector2Int targetTileDirection)
    {
        Vector2Int orthogonalDirection;

        if (targetTileDirection.x == 0)
        {
            orthogonalDirection = Vector2Int.right;
        }
        else
        {
            orthogonalDirection = Vector2Int.up;
        }

        return new List<Vector2Int>
        {
            targetTile - orthogonalDirection,
            targetTile,
            targetTile + orthogonalDirection
        };
    }
}
