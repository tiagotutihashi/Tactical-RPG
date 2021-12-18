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

}
