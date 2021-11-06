using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour {

    [SerializeField]
    private Tilemap map;
    private List<Grid> grids = new List<Grid>();

    public List<Grid> Grids => grids;

    [SerializeField]
    private Grid gridPrefab;

    void Start() {
        CreateGrid();
    }

    void CreateGrid() {

        int width = map.cellBounds.size.x;
        int height = map.cellBounds.size.y;

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                Grid newGrid = Instantiate(gridPrefab, transform);
                newGrid.SetIndex(x, y);
                newGrid.name = "(" + x + ", " + y + ")";
                grids.Add(newGrid);
            }
        }

    }

}
