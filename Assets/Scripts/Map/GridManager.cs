using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour {

    [SerializeField]
    private Tilemap map;
    [SerializeField]
    private List<CustomGrid> gridList = new List<CustomGrid>();

    public List<CustomGrid> Gridlist => gridList;

    [SerializeField]
    private CustomGrid gridPrefab;

    private UnitManager unitManager;

    private void Awake() {
        unitManager = FindObjectOfType<UnitManager>();
    }

    void Start() {
        CreateGrid();
    }

    public CustomGrid VerifyIfContains(Vector2 gridPosition) {

        foreach (CustomGrid grid in gridList) {
            if (
                grid.IndexX == gridPosition.x &&
                grid.IndexY == gridPosition.y
            ) {
                return grid;
            }
        }

        return null;

    }

    public CustomGrid GetTileByUnit(Unit unit)
    {
        foreach (CustomGrid grid in gridList)
        {
            if (grid.Unit == unit)
            {
                return grid;
            }
        }

        return null;
    }

    void CreateGrid() {

        UnitManager unitManager = FindObjectOfType<UnitManager>();
        List<Unit> unitList = unitManager.Unitlist;

        foreach (Unit unit in unitList) {

            Vector3Int gridPosition = map.WorldToCell(unit.transform.position);

            int x = gridPosition.x;
            int y = gridPosition.y;

            CustomGrid newGrid = Instantiate(gridPrefab, transform);
            newGrid.SetUnit(unit);
            newGrid.SetIndex(x, y);
            newGrid.name = "(" + x + ", " + y + ")";
            gridList.Add(newGrid);

        }

    }

    public void ChangeUnitGrid(Unit unitMoved, Vector3Int newPosition) {
        foreach (CustomGrid grid in gridList) {
            if (
                grid.Unit == unitMoved
            ) {
                grid.SetIndex(newPosition.x, newPosition.y);
                grid.name = "(" + newPosition.x + ", " + newPosition.y + ")";
                break;
            }
        }
    }

    public Unit EnemyInGrid(Vector3Int enemyPosition) {
        foreach (CustomGrid grid in gridList) {
            if (
                grid.IndexX == enemyPosition.x &&
                grid.IndexY == enemyPosition.y
            ) {
                if (unitManager.EnemyUnits.Contains(grid.Unit)) {
                    return grid.Unit;
                }
            }
        }
        return null;
    }

    public Unit PlayerInGrid(Vector3Int playerPosition)
    {
        foreach (CustomGrid grid in gridList)
        {
            if (
                grid.IndexX == playerPosition.x &&
                grid.IndexY == playerPosition.y
            )
            {
                if (unitManager.PlayerUnits.Contains(grid.Unit))
                {
                    return grid.Unit;
                }
            }
        }
        return null;
    }

}
