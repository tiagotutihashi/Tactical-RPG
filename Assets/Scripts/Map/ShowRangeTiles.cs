using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ShowRangeTiles : MonoBehaviour {

    [SerializeField] Tilemap map;
    [SerializeField] Tilemap rangeTileMap;

    [SerializeField] TileBase redTile;
    [SerializeField] TileBase greenTile;
    [SerializeField] TileBase selectTile;

    private PlayerInput playerInput;
    [SerializeField]
    private GridManager gridManager;

    [SerializeField]
    private List<TileData> tileObjects;

    [SerializeField]
    private Dictionary<TileBase, TileData> dataFromTiles;

    [SerializeField]
    private Unit unitSelected;

    [SerializeField]
    private List<Vector3Int> range = new List<Vector3Int>();

    [SerializeField]
    private List<StoredDataTile> storedDataTiles = new List<StoredDataTile>();

    [SerializeField]
    private Vector3Int gridPosition;

    [SerializeField]
    private TileBase selectedTile;

    private void Awake() {

        SetdataFromTiles();
        playerInput = new PlayerInput();

    }

    private void OnEnable() {
        playerInput.Enable();
    }

    private void OnDisable() {
        playerInput.Disable();
    }

    private void Start() {

        playerInput.Camera.MouseClick.performed += _ => onClickMap();
        gridManager = FindObjectOfType<GridManager>();

    }

    private void SetdataFromTiles() {
        dataFromTiles = new Dictionary<TileBase, TileData>();

        foreach (var tileData in tileObjects) {
            foreach (var selectedTile in tileData.Tiles) {
                dataFromTiles.Add(selectedTile, tileData);
            }
        }
    }

    private void onClickMap() {

        GetMoveRange(6);

    }

    private void GetMoveRange(int amount) {

        rangeTileMap.ClearAllTiles();

        Vector2 mousePosition = playerInput.Camera.MousePositon.ReadValue<Vector2>();

        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        gridPosition = map.WorldToCell(mousePosition);

        selectedTile = map.GetTile(gridPosition);

        // Return null if clicked outside a selectedTile
        if (selectedTile == null) { return; }

        List<CustomGrid> gridList = gridManager.Gridlist;

        CustomGrid unitGrid = gridManager.VerifyIfContains(new Vector2(gridPosition.x, gridPosition.y));

        if (unitGrid == null) {
            unitSelected = null;
            return;
        }

        if (unitSelected == null) {
            unitSelected = unitGrid.Unit;
        }

        // Making the map
        storedDataTiles.Clear();

        rangeTileMap.SetTile(gridPosition, selectTile);

        // Set Initial position 
        StoredDataTile initialStoredDataTile = new StoredDataTile();
        initialStoredDataTile.position = new Vector3Int(gridPosition.x, gridPosition.y, gridPosition.z);
        initialStoredDataTile.distance = 0;
        initialStoredDataTile.visited = true;
        initialStoredDataTile.landSpeed = dataFromTiles[selectedTile].WalkingSpeed;
        initialStoredDataTile.remainMovement = amount;
        storedDataTiles.Add(initialStoredDataTile);

        // Create the "Rhombus"
        for (int x = -amount; x <= amount; x++) {
            for (int y = -amount; y <= amount; y++) {

                float distanceFromCenter = Mathf.Abs(x) + Mathf.Abs(y);

                if (distanceFromCenter <= amount) {
                    int newX = gridPosition.x + x;
                    int newY = gridPosition.y + y;

                    Vector3Int cellPostion = new Vector3Int(newX, newY, gridPosition.z);
                    TileBase currentCell = map.GetTile(cellPostion);

                    if (currentCell != null)
                    {
                        StoredDataTile storedDataTile = new StoredDataTile();
                        storedDataTile.position = cellPostion;
                        storedDataTile.distance = Mathf.Infinity;
                        storedDataTile.landSpeed = dataFromTiles[currentCell].WalkingSpeed;
                        storedDataTiles.Add(storedDataTile);
                    }

                }

            }
        }

        CalcMovement(gridPosition, storedDataTiles, amount);

        range.Clear();

        foreach (StoredDataTile storedDataTile in storedDataTiles) {
            range.Add(storedDataTile.position);
        }

        range.RemoveAt(0);
        List<Vector3Int> toRemoveTiles = new List<Vector3Int>();
        foreach (var tile in range)
        {
            if (gridManager.VerifyIfContains(new Vector2(tile.x, tile.y)) != null)
            {
                toRemoveTiles.Add(tile);
            }
        }
        foreach (var tile in toRemoveTiles)
        {
            range.Remove(tile);
        }

        TileBase[] rangeTiles = new TileBase[range.Count];

        for (int i = 0; i < rangeTiles.Length; i++) {
            rangeTiles[i] = greenTile;
        }

        // Set the tiles that the unit can make action
        rangeTileMap.SetTiles(range.ToArray(), rangeTiles);

        if (unitSelected != null) {
            if (unitSelected.TryGetComponent<UnitMover>(out UnitMover unitMover)) {
                // StartCoroutine(unitMover.MoveUnitTo());
            }
        }

    }

    // Dijkstra Algorithm
    private void CalcMovement(Vector3Int initialPosition, List<StoredDataTile> storedDataTiles, int amountMove) {

        StoredDataTile startTile = storedDataTiles[0];
        startTile.visited = true;
        startTile.remainMovement = amountMove;

        MinHeap<StoredDataTile> heapTile = new MinHeap<StoredDataTile>(storedDataTiles.Count, x => x.distance);

        heapTile.Add(startTile);

        while (heapTile.Count() > 0) {

            StoredDataTile selectedTile = heapTile.RemoveMin();

            if (selectedTile.remainMovement > 0) {
                List<StoredDataTile> neighbor = storedDataTiles.Where(x => Mathf.Abs(selectedTile.position.x - x.position.x) + Mathf.Abs(selectedTile.position.y - x.position.y) == 1).ToList();

                foreach (StoredDataTile storedDataTile in neighbor) {
                    if (storedDataTile.distance > selectedTile.distance + storedDataTile.landSpeed && selectedTile.remainMovement > storedDataTile.landSpeed - 1) {
                        storedDataTile.parent = selectedTile;
                        storedDataTile.visited = true;
                        storedDataTile.remainMovement = selectedTile.remainMovement - storedDataTile.landSpeed;
                        storedDataTile.distance = selectedTile.distance + storedDataTile.landSpeed;
                        heapTile.Add(storedDataTile);
                    }

                }

                heapTile.BuildHeap();
            }

        }

        storedDataTiles.RemoveAll(x => !x.visited);

    }

}
