using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyMoverManager : MonoBehaviour
{
    private UnitManager unitManager;
    private UnitMoverManager unitMoverManager;
    private GridManager gridManager;
    private PathManager pathManager;
    private ShowRangeTiles showRangeTiles;

    private List<StoredDataTile> storedDataTiles;

    private void Start()
    {
        unitManager = FindObjectOfType<UnitManager>();
        unitMoverManager = FindObjectOfType<UnitMoverManager>();
        gridManager = FindObjectOfType<GridManager>();
        pathManager = FindObjectOfType<PathManager>();
        showRangeTiles = FindObjectOfType<ShowRangeTiles>();

        storedDataTiles = new List<StoredDataTile>();
    }

    float time = 0;
    private void Update()
    {
        time += Time.deltaTime;

        if (time > 3 && time < 4)
        {
            Debug.Log("Entered");
            time = 4;
            StartCoroutine(MakeEnemiesMove());
        }
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
                        candidate.SetTarget(Mathf.Abs(attackedTile.x - tilePosition.x) + Mathf.Abs(attackedTile.y - tilePosition.y));
                    }
                }
            }

            candidate.SetDistanceToInitial(Mathf.Abs(moveOptions[0].position.x - tilePosition.x) + Mathf.Abs(moveOptions[0].position.y - tilePosition.y));

            candidateTiles.Add(candidate);
        }

        var temp = candidateTiles.FindAll(x => x.HasTarget);

        foreach (CandidateTile candidate in candidateTiles)
        {
            if (candidate.HasTarget)
            {
                if (candidate.DistanceToTarget > choosenCandidate.DistanceToTarget)
                {
                    choosenCandidate = candidate;
                }
                else if (candidate.DistanceToTarget == choosenCandidate.DistanceToTarget
                    && candidate.DistanceToInitial < choosenCandidate.DistanceToInitial)
                {
                    choosenCandidate = candidate;
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
        initialStoredDataTile.remainMovement = enemy.Movement;
        storedDataTiles.Add(initialStoredDataTile);

        CreateEnemyRhombus(enemy, enemyPosition);

        return pathManager.CalcMovement(enemyPosition, storedDataTiles, enemy.Movement);
    }

    // Create the "Rhombus"
    private void CreateEnemyRhombus(Unit enemy, Vector3Int enemyPosition)
    {
        int amount = enemy.Movement;

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
