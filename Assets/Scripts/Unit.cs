using System.Collections;
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

    private bool isDead;

    [SerializeField]
    private JobBase job;
    [SerializeField]
    private WeaponBase weapon;
    public WeaponBase Weapon => weapon;

    [SerializeField]
    private HealthBar healthBar;
    public HealthBar HealthBar => healthBar;

    public int Level => level;
    public int Exp => exp;
    public int MaxHealth => maxHealth;
    public int Health => health;
    public int Attack => attack;
    public int Defense => defense;
    public int Movement => movement;

    private void Awake() {
        isDead = false;

        job = GetComponent<JobBase>();
    }

    private void Start() {
        level = level == 0 ? 1 : level;
        SetUnitByLevel(level);
        healthBar.SetHealthMaxValue(maxHealth);
        healthBar.SetHealthValue(health);
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

    public IEnumerator DealDamage(Unit target) {
        int damage = attack + weapon.Damage;
        yield return StartCoroutine(target.ReceiveDamage(damage));
    }

    public IEnumerator ReceiveDamage(int damage) {
        int finalDamage = Mathf.Clamp(damage - defense, 1, damage);
        health = Mathf.Clamp(health - finalDamage, 0, health);

        yield return StartCoroutine(HealthAnimation(health));
    }

    private IEnumerator HealthAnimation(int damage){

        yield return StartCoroutine(healthBar.DecreaseHealthValue(damage));
        if (health == 0 && !isDead) {
            yield return StartCoroutine(Die());
        }
    }

    private IEnumerator Die() {
        SpriteEffect spriteEffect = GetComponentInChildren<SpriteEffect>();

        isDead = true;

        if (spriteEffect != null) {
            yield return StartCoroutine(spriteEffect.FadeTo(0.0f, 1.0f));
            DestroyUnit();
        } else {
            Debug.LogError("SpriteEffect component not found in Unit child game object");
        }
    }

    private void DestroyUnit() {
        UnitManager unitManager = FindObjectOfType<UnitManager>();
        UnitMatch unitMatch = GetComponent<UnitMatch>();

        if (unitManager != null && unitMatch != null) {
            unitManager.RemoveUnit(this, unitMatch.IsAlly);
        }

        Destroy(gameObject);
    }
}
