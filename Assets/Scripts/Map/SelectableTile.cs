using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectableTile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {

    private ShowRangeTiles showRangeTiles;

    private List<Vector3Int> tilesPosition = new List<Vector3Int>();
    private Vector3Int baseTile;

    public void SetBaseTile(Vector3Int newValue) {
        baseTile = newValue;
    }

    public List<Vector3Int> GetTilesPosition() {
        return tilesPosition;
    }

    private void Awake() {
        showRangeTiles = FindObjectOfType<ShowRangeTiles>();
    }

    public void SetTilePosition(List<Vector3Int> newList) {
        tilesPosition = newList;
    }

    private List<Vector3Int> BaseTileRemoved() {
        List<Vector3Int> tilesToShow = new List<Vector3Int>(tilesPosition);
        tilesToShow.Remove(baseTile);
        return tilesToShow;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        showRangeTiles.ShowAttackSelectableTile(BaseTileRemoved());
    }

    public void OnPointerExit(PointerEventData eventData) {
        showRangeTiles.RemoveAttackSelectableTile(BaseTileRemoved());
    }

    public void OnPointerClick(PointerEventData pointerEventData) {
        BattleManager battleManager = FindObjectOfType<BattleManager>();
        UnitMoverManager unitMoverManager = FindObjectOfType<UnitMoverManager>();
        StartCoroutine(battleManager.DealDamageInEnemies(tilesPosition, unitMoverManager.UnitSelected));
    }

}
