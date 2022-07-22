using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMoverManager : MonoBehaviour {

    private GridManager gridManager;
    private PathManager pathManager;
    private ActionModal actionModal;

    [SerializeField]
    private Unit unitSelected;

    public Unit UnitSelected => unitSelected;

    private Vector3Int initialPosition;
    public Vector3Int InitialPosition => initialPosition;

    private Vector3Int finalPosition;
    public Vector3Int FinalPosition => finalPosition;

    private void Awake() {
        gridManager = FindObjectOfType<GridManager>();
        pathManager = FindObjectOfType<PathManager>();
        actionModal = FindObjectOfType<ActionModal>();
    }

    public IEnumerator UnitMovement(StoredDataTile storedDataTile, UnitMover unitMover) {

        List<Vector3Int> path = pathManager.ReturnUnitPath(storedDataTile);
        initialPosition = path[0];
        yield return StartCoroutine(
             unitMover.MoveUnitTo(
                 path
             ));
        finalPosition = storedDataTile.position;
        gridManager.ChangeUnitGrid(unitMover.GetComponent<Unit>(), storedDataTile.position);

        if (actionModal) {
            actionModal.ShowModal();
        }

    }

    public bool MakeMovement(CustomGrid unitGrid, List<StoredDataTile> storedDataTiles, Vector3Int gridPosition) {
        if (unitSelected == null) {
            if (unitGrid == null) {
                unitSelected = null;
                return true;
            }
            if (unitGrid.Unit.TryGetComponent<UnitMatch>(out UnitMatch unitMatch)) {
                if (!unitMatch.IsAlly) {
                    return true;
                } else if (unitMatch.Moved) {
                    return true;
                }
            }
            unitSelected = unitGrid.Unit;
        } else if (storedDataTiles.Count > 0) {
            foreach (StoredDataTile storedDataTile in storedDataTiles) {
                if (storedDataTile.position.x == gridPosition.x && storedDataTile.position.y == gridPosition.y) {
                    if (storedDataTile == storedDataTiles[0]
                        || gridManager.VerifyIfContains(new Vector2(gridPosition.x, gridPosition.y)) != null
                    ) {
                        unitSelected = null;
                        return true;
                    }
                    if (unitSelected.TryGetComponent<UnitMover>(out UnitMover unitMover)) {

                        StartCoroutine(UnitMovement(storedDataTile, unitMover));

                        return true;
                    }
                }
            }
            unitSelected = null;
            return true;
        }

        return false;
    }

    public bool MoveUnitBack() {
        if (unitSelected.TryGetComponent<UnitMover>(out UnitMover unitMover)) {
            unitMover.TeleportUnitTo(initialPosition);
            unitSelected = null;
            gridManager.ChangeUnitGrid(unitMover.GetComponent<Unit>(), initialPosition);
            return true;
        }
        return false;
    }

    public void CleanUnitSelected() {
        unitSelected = null;
    }

}
