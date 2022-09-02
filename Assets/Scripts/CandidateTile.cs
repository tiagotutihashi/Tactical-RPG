using UnityEngine;

public class CandidateTile
{
    private StoredDataTile tile;
    private bool hasTarget;
    private int distanceToTarget;
    private bool isInitial;
    private int distanceToInitial;

    public StoredDataTile Tile => tile;
    public bool HasTarget => hasTarget;
    public int DistanceToTarget => distanceToTarget;
    public bool IsInitial => isInitial;
    public int DistanceToInitial => distanceToInitial;

    public CandidateTile(StoredDataTile tile)
    {
        this.tile = tile;
        hasTarget = false;
        distanceToTarget = 0;
        isInitial = false;
    }

    public void SetTarget(int distanceToTarget)
    {
        hasTarget = true;
        this.distanceToTarget = distanceToTarget;
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
