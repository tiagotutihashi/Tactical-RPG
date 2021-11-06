using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ShowRangeTiles : MonoBehaviour {

    [SerializeField] Tilemap map;
    [SerializeField] Tilemap rangeTileMap;

    [SerializeField] TileBase redTile;
    [SerializeField] TileBase greenTile;

    private PlayerInput playerInput;

    private void Awake() {
        playerInput = new PlayerInput();
    }

    private void OnEnable() {
        playerInput.Enable();
    }

    private void OnDisable() {
        playerInput.Disable();
    }

    private void Start() {

        playerInput.Camera.MouseClick.performed += _ => GetMoveRange(5);

    }

    private List<Vector3Int> GetMoveRange(int amount) {

        Vector2 mousePosition = playerInput.Camera.MousePositon.ReadValue<Vector2>();

        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector3Int gridPosition = map.WorldToCell(mousePosition);

        TileBase tile = map.GetTile(gridPosition);

        // Return null if clicked outside a tile
        if (tile == null) { return null; }

        List<Vector3Int> range = new List<Vector3Int>();
        range.Add(gridPosition);
        for (int i = 0; i < amount; i++) {
            range.Add(new Vector3Int(gridPosition.x + i + 1, gridPosition.y, gridPosition.z));
            range.Add(new Vector3Int(gridPosition.x + i, gridPosition.y, gridPosition.z));
            range.Add(new Vector3Int(gridPosition.x - i - 1, gridPosition.y, gridPosition.z));
            range.Add(new Vector3Int(gridPosition.x - i, gridPosition.y, gridPosition.z));
            range.Add(new Vector3Int(gridPosition.x, gridPosition.y + i + 1, gridPosition.z));
            range.Add(new Vector3Int(gridPosition.x, gridPosition.y - i - 1, gridPosition.z));
            range.Add(new Vector3Int(gridPosition.x, gridPosition.y + i, gridPosition.z));
            range.Add(new Vector3Int(gridPosition.x, gridPosition.y - i, gridPosition.z));
        }

        TileBase[] reds = new TileBase[range.Count];

        for (int i = 0; i < reds.Length; i++) {
            reds[i] = redTile;
        }

        // Set 
        rangeTileMap.SetTiles(range.ToArray(), reds);

        return range;

    }

}
