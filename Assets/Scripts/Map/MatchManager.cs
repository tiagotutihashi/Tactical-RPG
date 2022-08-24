using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum TurnState { START, PLAYERTURN, ENEMYTURN, WON, LOST, FLED }

public class MatchManager : MonoBehaviour {

    private UnitManager unitManager;
    private ShowRangeTiles showRangeTiles;

    private ActionModal actionModal;

    [SerializeField]
    private TurnState currentTurnState;
    public TurnState CurrentTurnState => currentTurnState;

    [SerializeField]
    private TextMeshProUGUI turnText;

    private int playerUnitActions = 0;
    private int enemyUnitActions = 0;

    private void Start() {
        unitManager = FindObjectOfType<UnitManager>();
        showRangeTiles = FindObjectOfType<ShowRangeTiles>();
        actionModal = FindObjectOfType<ActionModal>();
    }

    public void StartMatch() {
        // TODO  
        StartPlayerTurn();
    }

    public void StartPlayerTurn() {
        turnText.text = "Seu Turno";
        unitManager.PlayerRemoveMoved();
        playerUnitActions = 0;
    }

    public void PlayerUnitMadeAction(Unit unit) {
        playerUnitActions += 1;
        if (unit.TryGetComponent<UnitMatch>(out UnitMatch unitMatch)) {
            unitMatch.SetMoved(true);
            showRangeTiles.ResetSelectableRangeTiles();
            showRangeTiles.ClearAllTiles();
            actionModal.CloseModal();
        }
        if (playerUnitActions >= unitManager.PlayerUnits.Count) {
            StartEnemyTurn();
        }
    }

    public void StartEnemyTurn() {
        turnText.text = "Inimigo Turno";
        enemyUnitActions = 0;
    }

    public void EnemyUnitMadeAction() {
        enemyUnitActions += 1;
        if (enemyUnitActions >= unitManager.EnemyUnits.Count) {
            StartPlayerTurn();
        }
    }

}
