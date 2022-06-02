using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    [SerializeField]
    private WeaponObject weaponObject;

    public WeaponObject WeaponObject => weaponObject;

    public abstract List<Vector2Int> GetAttackedTiles(Vector2Int tile, Vector2Int direction);
}
