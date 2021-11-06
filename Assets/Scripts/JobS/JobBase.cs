using UnityEngine;

public abstract class JobBase : MonoBehaviour
{
    [SerializeField]
    private JobObject jobObject;

    public JobObject JobObject => jobObject;
}
