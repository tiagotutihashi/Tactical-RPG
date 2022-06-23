using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    [SerializeField]
    private WeaponObject weaponObject;

    public WeaponObject WeaponObject => weaponObject;

    public abstract List<Vector2Int> GetAttackedTiles(Vector2Int targetTile, Vector2Int targetTileDirection);

    public List<Vector2Int> GetTilesInRange(Vector2Int attackerTile)
    {
        List<Vector2Int> tilesInRange = new List<Vector2Int>();

        for (int range = weaponObject.MaxRange; range > 0; range--)
        {
            tilesInRange.AddRange(GetExternalTilesAtRange(attackerTile, range));
        }

        return tilesInRange;
    }

    private List<Vector2Int> GetExternalTilesAtRange(Vector2Int attackerTile, int range)
    {
        List<Vector2Int> externalTilesAtRange = new List<Vector2Int>();

        if (range == 0)
        {
            return externalTilesAtRange;
        }

        for (int x = -range; x <= range; x++)
        {
            if (Mathf.Abs(x) == range)
            {
                externalTilesAtRange.Add(new Vector2Int(attackerTile.x + x, attackerTile.y));
            }
            else
            {
                int y = range - Mathf.Abs(x);
                externalTilesAtRange.Add(new Vector2Int(attackerTile.x + x, attackerTile.y + y));
                externalTilesAtRange.Add(new Vector2Int(attackerTile.x + x, attackerTile.y - y));
            }
        }

        return externalTilesAtRange;
    }
}
