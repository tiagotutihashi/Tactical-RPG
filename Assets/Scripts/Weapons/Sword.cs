using System.Collections.Generic;
using UnityEngine;

public class Sword : WeaponBase
{
    public override List<Vector2Int> GetAttackedTiles(Vector2Int tile, Vector2Int direction)
    {
        Vector2Int orthogonalDirection;

        if (direction.x == 0)
        {
            orthogonalDirection = Vector2Int.right;
        }
        else
        {
            orthogonalDirection = Vector2Int.up;
        }

        return new List<Vector2Int>
        {
            tile - orthogonalDirection,
            tile,
            tile + orthogonalDirection
        };
    }
}
