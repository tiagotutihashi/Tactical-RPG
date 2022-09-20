using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyMoverManager : MonoBehaviour
{
    [SerializeField]
    private int visionRange; // vision range of an enemy that it will use to approach distant targets

    private UnitManager unitManager;
    private GridManager gridManager;
    private PathManager pathManager;
    private ShowRangeTiles showRangeTiles;

    private List<StoredDataTile> storedDataTiles;

    private void Start()
    {
        unitManager = FindObjectOfType<UnitManager>();
        gridManager = FindObjectOfType<GridManager>();
        pathManager = FindObjectOfType<PathManager>();
        showRangeTiles = FindObjectOfType<ShowRangeTiles>();

        storedDataTiles = new List<StoredDataTile>();
    }

    public IEnumerator MakeEnemiesMove()
    {
        foreach (Unit enemy in unitManager.EnemyUnits)
        {
            yield return StartCoroutine(MakeEnemyMove(enemy));
        }
    }

    private IEnumerator MakeEnemyMove(Unit enemy)
    {
        List<StoredDataTile> moveOptions = GetEnemyMoveOptions(enemy);
        UnitMover unitMover = enemy.GetComponent<UnitMover>();

        if (moveOptions == null || unitMover == null)
        {
            yield return null;
        }

        StoredDataTile finalTile = ChooseTileToMove(enemy, moveOptions);
        List<Vector3Int> path = pathManager.ReturnUnitPath(finalTile);

        gridManager.ChangeUnitGrid(enemy, finalTile.position);

        yield return unitMover.MoveUnitTo(path);
    }

    private StoredDataTile ChooseTileToMove(Unit enemy, List<StoredDataTile> moveOptions)
    {
        CandidateTile choosenCandidate = new CandidateTile(moveOptions[0]);
        List<CandidateTile> candidateTiles = new List<CandidateTile>();

        foreach (StoredDataTile tile in moveOptions)
        {
            CandidateTile candidate = new CandidateTile(tile);
            Vector2Int tilePosition = new Vector2Int(tile.position.x, tile.position.y);
            CustomGrid thisTile = gridManager.VerifyIfContains(tilePosition);
            int distanceToInitial = Mathf.Abs(moveOptions[0].position.x - tilePosition.x) + Mathf.Abs(moveOptions[0].position.y - tilePosition.y);

            if (tile == moveOptions[0])
            {
                candidate.SetInitial();
            }
            else if (thisTile != null)
            {
                continue;
            }

            foreach (Vector2Int tileInRange in enemy.Weapon.GetTilesInRange(tilePosition))
            {
                Vector2Int direction = tileInRange - tilePosition;

                foreach (Vector2Int attackedTile in enemy.Weapon.GetAttackedTiles(tileInRange, direction))
                {
                    CustomGrid attackedTileGrid = gridManager.VerifyIfContains(attackedTile);
                    if (attackedTileGrid != null && attackedTileGrid.Unit.GetComponent<UnitMatch>().IsAlly)
                    {
                        int distanceToTarget = Mathf.Abs(attackedTile.x - tilePosition.x) + Mathf.Abs(attackedTile.y - tilePosition.y);

                        if (distanceToInitial <= enemy.Movement)
                        {
                            candidate.SetAttackableTarget(distanceToTarget);
                        }
                        else
                        {
                            StoredDataTile middleTile = candidate.Tile;

                            while (middleTile.distance > enemy.Movement)
                            {
                                middleTile = middleTile.parent;
                                distanceToTarget++;
                            }

                            candidate = new CandidateTile(middleTile);
                            candidate.SetVisibleTarget(distanceToTarget);
                        }
                    }
                }
            }

            candidate.SetDistanceToInitial(distanceToInitial);
            candidateTiles.Add(candidate);
        }

        List<CandidateTile> attackableTargets = candidateTiles.FindAll(x => x.HasAttackableTarget);
        if (attackableTargets.Count > 0)
        {
            choosenCandidate = attackableTargets[0];
            foreach (CandidateTile attackableTarget in attackableTargets)
            {
                if (attackableTarget.DistanceToAttackableTarget > choosenCandidate.DistanceToAttackableTarget)
                {
                    choosenCandidate = attackableTarget;
                }
            }
        }
        else
        {
            List<CandidateTile> visibleTargets = candidateTiles.FindAll(x => x.HasVisibleTarget);
            if (visibleTargets.Count > 0)
            {
                choosenCandidate = visibleTargets[0];
                foreach (CandidateTile visibleTarget in visibleTargets)
                {
                    if (visibleTarget.DistanceToVisibleTarget > choosenCandidate.DistanceToVisibleTarget)
                    {
                        choosenCandidate = visibleTarget;
                    }
                }
            }
        }

        return choosenCandidate.Tile;
    }

    private List<StoredDataTile> GetEnemyMoveOptions(Unit enemy)
    {
        CustomGrid enemyTile = gridManager.GetTileByUnit(enemy);

        if (enemyTile == null)
        {
            Debug.LogError("Enemy not found in the grid.");
            return null;
        }

        Vector3Int enemyPosition = new Vector3Int(enemyTile.IndexX, enemyTile.IndexY, 0);

        storedDataTiles.Clear();

        // Set Initial position 
        StoredDataTile initialStoredDataTile = new StoredDataTile();
        initialStoredDataTile.position = enemyPosition;
        initialStoredDataTile.distance = 0;
        initialStoredDataTile.visited = true;
        initialStoredDataTile.landSpeed = 0;
        initialStoredDataTile.remainMovement = enemy.Movement + visionRange;
        storedDataTiles.Add(initialStoredDataTile);

        CreateEnemyRhombus(enemy, enemyPosition);

        return pathManager.CalcMovement(enemyPosition, storedDataTiles, enemy.Movement + visionRange);
    }

    // Create the "Rhombus"
    private void CreateEnemyRhombus(Unit enemy, Vector3Int enemyPosition)
    {
        int amount = enemy.Movement + visionRange;

        for (int x = -amount; x <= amount; x++)
        {
            for (int y = -amount; y <= amount; y++)
            {
                float distanceFromCenter = Mathf.Abs(x) + Mathf.Abs(y);

                if (distanceFromCenter <= amount)
                {
                    int newX = enemyPosition.x + x;
                    int newY = enemyPosition.y + y;

                    Vector3Int cellPosition = new Vector3Int(newX, newY, enemyPosition.z);
                    TileBase currentCell = showRangeTiles.Map.GetTile(cellPosition);

                    CustomGrid verifyUnitGridCurrentPosition = gridManager.VerifyIfContains(new Vector2(newX, newY));

                    if (currentCell != null)
                    {
                        StoredDataTile storedDataTile = new StoredDataTile();
                        storedDataTile.position = cellPosition;
                        storedDataTile.distance = Mathf.Infinity;
                        storedDataTile.landSpeed = showRangeTiles.DataFromTiles[currentCell].WalkingSpeed;
                        storedDataTiles.Add(storedDataTile);
                    }
                }
            }
        }
    }
}
