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
    private ActionModal actionModal;
    private PathManager pathManager;
    private UnitMoverManager unitMoverManager;

    [SerializeField]
    private List<TileData> tileObjects;

    [SerializeField]
    private Dictionary<TileBase, TileData> dataFromTiles;

    [SerializeField]
    private List<StoredDataTile> storedDataTiles = new List<StoredDataTile>();

    [SerializeField]
    private Vector3Int gridPosition;
    public Vector3Int GridPosition => gridPosition;

    [SerializeField]
    private TileBase selectedTile;

    private void Awake() {

        SetdataFromTiles();
        playerInput = new PlayerInput();
        actionModal = FindObjectOfType<ActionModal>();

        pathManager = FindObjectOfType<PathManager>();
        unitMoverManager = FindObjectOfType<UnitMoverManager>();

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

        GetMoveRange();

    }

    private void GetMoveRange() {

        if (actionModal.IsModalActive()) {
            return;
        }

        rangeTileMap.ClearAllTiles();

        Vector2 mousePosition = playerInput.Camera.MousePositon.ReadValue<Vector2>();

        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        gridPosition = map.WorldToCell(mousePosition);

        selectedTile = map.GetTile(gridPosition);

        // Return null if clicked outside a selectedTile
        if (selectedTile == null) { return; }

        List<CustomGrid> gridList = gridManager.Gridlist;

        CustomGrid unitGrid = gridManager.VerifyIfContains(new Vector2(gridPosition.x, gridPosition.y));

        bool madeMovement = unitMoverManager.MakeMovement(unitGrid, storedDataTiles, gridPosition);

        if (madeMovement) {
            return;
        }

        int amount = unitMoverManager.UnitSelected.Movement;

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

        CreateRhombus(amount);

        pathManager.CalcMovement(gridPosition, storedDataTiles, amount);

        ShowMovementTiles();

    }

    // Create the "Rhombus"
    private void CreateRhombus(int amount) {
        for (int x = -amount; x <= amount; x++) {
            for (int y = -amount; y <= amount; y++) {

                float distanceFromCenter = Mathf.Abs(x) + Mathf.Abs(y);

                if (distanceFromCenter <= amount) {
                    int newX = gridPosition.x + x;
                    int newY = gridPosition.y + y;

                    Vector3Int cellPostion = new Vector3Int(newX, newY, gridPosition.z);
                    TileBase currentCell = map.GetTile(cellPostion);

                    CustomGrid verifyUnitGridCurrentPosition = gridManager.VerifyIfContains(new Vector2(newX, newY));

                    if (currentCell != null) {
                        StoredDataTile storedDataTile = new StoredDataTile();
                        storedDataTile.position = cellPostion;
                        storedDataTile.distance = Mathf.Infinity;
                        storedDataTile.landSpeed = dataFromTiles[currentCell].WalkingSpeed;
                        storedDataTiles.Add(storedDataTile);
                    }

                }

            }
        }
    }

    // 
    private void ShowMovementTiles() {
        List<Vector3Int> range = new List<Vector3Int>();

        foreach (StoredDataTile storedDataTile in storedDataTiles) {
            range.Add(storedDataTile.position);
        }

        range.RemoveAt(0);
        List<Vector3Int> toRemoveTiles = new List<Vector3Int>();
        foreach (var tile in range) {
            if (gridManager.VerifyIfContains(new Vector2(tile.x, tile.y)) != null) {
                toRemoveTiles.Add(tile);
            }
        }
        foreach (var tile in toRemoveTiles) {
            range.Remove(tile);
        }

        TileBase[] rangeTiles = new TileBase[range.Count];

        for (int i = 0; i < rangeTiles.Length; i++) {
            rangeTiles[i] = greenTile;
        }

        // Set the tiles that the unit can make action
        rangeTileMap.SetTiles(range.ToArray(), rangeTiles);
    }

}
