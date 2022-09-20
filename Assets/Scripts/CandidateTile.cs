using UnityEngine;

public class CandidateTile
{
    private StoredDataTile tile;
    private bool hasAttackableTarget;
    private int distanceToAttackableTarget;
    private bool hasVisibleTarget;
    private int distanceToVisibleTarget;
    private bool isInitial;
    private int distanceToInitial;

    public StoredDataTile Tile => tile;
    public bool HasAttackableTarget => hasAttackableTarget;
    public int DistanceToAttackableTarget => distanceToAttackableTarget;
    public bool HasVisibleTarget => hasVisibleTarget;
    public int DistanceToVisibleTarget => distanceToVisibleTarget;
    public bool IsInitial => isInitial;
    public int DistanceToInitial => distanceToInitial;

    public CandidateTile(StoredDataTile tile)
    {
        this.tile = tile;
        hasAttackableTarget = false;
        distanceToAttackableTarget = 0;
        isInitial = false;
    }

    public void SetAttackableTarget(int distanceToTarget)
    {
        hasAttackableTarget = true;
        distanceToAttackableTarget = distanceToTarget;
    }

    public void SetVisibleTarget(int distanceToTarget)
    {
        hasVisibleTarget = true;
        distanceToVisibleTarget = distanceToTarget;
    }

    public void SetInitial()
    {
        isInitial = true;
    }

    public void SetDistanceToInitial(int distanceToInitial)
    {
        this.distanceToInitial = distanceToInitial;
    }
}
