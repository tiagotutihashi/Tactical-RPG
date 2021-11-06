using UnityEngine;

[CreateAssetMenu(fileName = "Job", menuName = "Job")]
public class JobObject : ScriptableObject
{
    [Header("Job Info")]
    [SerializeField]
    private string jobName;
    [SerializeField]
    private string jobDescription;

    [Header("Base Stats")]
    [SerializeField]
    private int baseHealth;
    [SerializeField]
    private int baseAttack;
    [SerializeField]
    private int baseDefense;
    [SerializeField]
    private int baseAgility;
    [SerializeField]
    private int movement;

    [Header("Level Increment Stats")]
    [SerializeField]
    private int incrementHealth;
    [SerializeField]
    private int incrementAttack;
    [SerializeField]
    private int incrementDefense;
    [SerializeField]
    private int incrementAgility;

    public string JobName => jobName;
    public string JobDescription => jobDescription;
    public int BaseHealth => baseHealth;
    public int BaseAttack => baseAttack;
    public int BaseDefense => baseDefense;
    public int BaseAgility => baseAgility;
    public int Movement => movement;
    public int IncrementHealth => incrementHealth;
    public int IncrementAttack => incrementAttack;
    public int IncrementDefense => incrementDefense;
    public int IncrementAgility => incrementAgility;
}
