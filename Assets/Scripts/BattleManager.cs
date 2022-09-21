using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour {

    private GridManager gridManager;
    private MatchManager matchManager;

    private void Awake() {
        gridManager = FindObjectOfType<GridManager>();
        matchManager = FindObjectOfType<MatchManager>();
    }

    private bool CheckEnemyInRange(List<Vector3Int> range) {
        foreach (Vector3Int grid in range) {
            Unit enemy = gridManager.EnemyInGrid(grid);
            if (enemy) {
                enemy.HealthBar.ShowHealthbar();
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

    public (List<Vector3Int>, List<List<Vector3Int>>) EnemyInRange(Vector2Int unitPosition, WeaponBase weapon) {

        List<Vector3Int> rangeBase = new List<Vector3Int>();
        List<List<Vector3Int>> range = new List<List<Vector3Int>>();

        weapon.GetTilesInRange(unitPosition).ForEach(item => {
            Vector3Int newRangeTile = new Vector3Int(item.x, item.y, 0);
            Vector2Int direction = AttackDirection(unitPosition, item);
            List<Vector3Int> newRange = CalcDirection(unitPosition, weapon, direction);
            if (newRange.Count > 0) {
                rangeBase.Add(newRangeTile);
                range.Add(newRange);
            }
        });

        return (rangeBase, range);
    }

    public IEnumerator DealDamageInEnemies(List<Vector3Int> positionToDealDamage, Unit attackingUnit)
    {
        List<Unit> targetsToDealDamage = new List<Unit>();
        for (int i = 0; i < positionToDealDamage.Count; i++)
        {
            if ((attackingUnit.GetComponent<UnitMatch>().IsAlly && gridManager.EnemyInGrid(positionToDealDamage[i]))
                || (!attackingUnit.GetComponent<UnitMatch>().IsAlly && gridManager.PlayerInGrid(positionToDealDamage[i])))
            {
                CustomGrid customGrid = gridManager.VerifyIfContains(new Vector2(positionToDealDamage[i].x, positionToDealDamage[i].y));
                if (customGrid)
                {
                    targetsToDealDamage.Add(customGrid.Unit);
                }
            }
        }

        foreach (Unit target in targetsToDealDamage)
        {
            yield return StartCoroutine(attackingUnit.DealDamage(target));
        }

        foreach (Unit target in targetsToDealDamage)
        {
            target.HealthBar.HideHealthbar();
        }

        // TODO colocar para remover o unidade selecionada e desaparecer com o modal de ação
        if (attackingUnit.GetComponent<UnitMatch>().IsAlly)
        {
            matchManager.PlayerUnitMadeAction(attackingUnit);
        }
    }

}
