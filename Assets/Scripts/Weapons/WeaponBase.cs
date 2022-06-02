using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    [SerializeField]
    private WeaponObject weaponObject;

    public WeaponObject WeaponObject => weaponObject;
}
