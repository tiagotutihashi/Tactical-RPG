using UnityEngine;

public class WeaponObject : ScriptableObject
{
    [Header("Weapon Info")]
    [SerializeField]
    private string weaponName;
    [SerializeField]
    private string weaponDescription;

    [Header("Base Stats")]
    [SerializeField]
    private int damage;

    public string WeaponName => weaponName;
    public string WeaponDescription => weaponDescription;
    public int Damage => damage;
}
