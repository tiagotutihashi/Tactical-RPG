using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour {

    private GridManager gridManager;

    private void Awake() {
        gridManager = FindObjectOfType<GridManager>();
    }

    private bool CheckEnemyInRange(List<Vector3Int> range) {
        foreach (Vector3Int grid in range) {
            if (gridManager.EnemyInGrid(grid)) {
                return true;
            }
        }

        return false;
    }

    private List<Vector3Int> CalcDirection(
        Vector2Int unitPosition,
        WeaponBase weapon,
        Vector2Int direction
    ) {
        List<Vector3Int> range = new List<Vector3Int>();

        weapon.GetAttackedTiles(unitPosition + direction, direction).ForEach(item => {
            Vector3Int newRangeTile = new Vector3Int(item.x, item.y, 0);
            if (!range.Contains(newRangeTile)) {
                range.Add(newRangeTile);
            }
        });

        bool enemyInsideRange = CheckEnemyInRange(range);

        if (enemyInsideRange) {
            return range;
        } else {
            return new List<Vector3Int>();
        }
    }

    private Vector2Int AttackDirection(Vector2Int unitPosition, Vector2Int attackPosition) {

        Vector2Int direction = attackPosition - unitPosition;

        return direction;
    }

    public (List<Vector3Int>, List<Vector3Int>) EnemyInRange(Vector2Int unitPosition, WeaponBase weapon) {

        List<Vector3Int> rangeBase = new List<Vector3Int>();
        List<Vector3Int> range = new List<Vector3Int>();

        weapon.GetTilesInRange(unitPosition).ForEach(item => {
            Vector3Int newRangeTile = new Vector3Int(item.x, item.y, 0);
            Vector2Int direction = AttackDirection(unitPosition, item);
            List<Vector3Int> newRange = CalcDirection(unitPosition, weapon, direction);
            if (newRange.Count > 0) {
                rangeBase.Add(newRangeTile);
                range.AddRange(newRange);
            }
        });

        range.RemoveRange(0, rangeBase.Count);

        return (rangeBase, range);
    }

}
