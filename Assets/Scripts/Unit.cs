using UnityEngine;

public class Unit : MonoBehaviour {
    [Header("Stats")]
    [SerializeField]
    private int level;
    [SerializeField]
    private int exp;
    [SerializeField]
    private int maxHealth;
    [SerializeField]
    private int health;
    [SerializeField]
    private int attack;
    [SerializeField]
    private int defense;
    [SerializeField]
    private int movement;

    private JobBase job;
    private WeaponBase weapon;

    public int Level => level;
    public int Exp => exp;
    public int MaxHealth => maxHealth;
    public int Health => health;
    public int Attack => attack;
    public int Defense => defense;
    public int Movement => movement;

    private void Awake() {
        job = GetComponent<JobBase>();
        weapon = GetComponent<WeaponBase>();
    }

    private void Start() {
        level = level == 0 ? 1 : level;
        SetUnitByLevel(level);
    }

    public void SetUnitByLevel(int level) {
        this.level = level;
        exp = 0;
        maxHealth = job.BaseHealth + (level - 1) * job.IncrementHealth;
        health = maxHealth;
        attack = job.BaseAttack + (level - 1) * job.IncrementAttack;
        defense = job.BaseDefense + (level - 1) * job.IncrementDefense;
        movement = job.Movement;
    }

    public void LevelUp() {
        level++;
        maxHealth += job.IncrementHealth;
        attack += job.IncrementAttack;
        defense += job.IncrementDefense;
    }

    public void EarnExp(bool isKill) {
        // Base exp gain values for normal action and kill
        int expGain = isKill ? 40 : 15;

        // Reduction of exp gain by level
        expGain = Mathf.FloorToInt(expGain * (101 - level) / 100);

        if (exp + expGain >= 100) {
            exp = exp + expGain - 100;
            LevelUp();
        } else {
            exp += expGain;
        }
    }

    public void DealDamage(Unit target)
    {
        int damage = attack + weapon.Damage;
        target.ReceiveDamage(damage);
    }

    public void ReceiveDamage(int damage)
    {
        int finalDamage = damage - defense;
        health = Mathf.Clamp(health - finalDamage, 0, health);
        // Será necessário fazer alguma verificação de morte?
    }
}
