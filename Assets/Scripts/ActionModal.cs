using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionModal : MonoBehaviour {

    [SerializeField]
    private GameObject buttonContainer;

    private UnitManager unitManager;

    private UnitMoverManager unitMoverManager;
    private MatchManager matchManager;
    private ShowRangeTiles showRangeTiles;

    private void Awake() {
        unitManager = FindObjectOfType<UnitManager>();
        unitMoverManager = FindObjectOfType<UnitMoverManager>();
        matchManager = FindObjectOfType<MatchManager>();
        showRangeTiles = FindObjectOfType<ShowRangeTiles>();
    }

    public bool IsModalActive() {
        return buttonContainer.activeInHierarchy;
    }

    public void ShowModal() {
        showRangeTiles.ShowSelectedAttackRange();
        buttonContainer.SetActive(true);
    }

    public void CloseModal() {
        buttonContainer.SetActive(false);
    }

    public void AttackButtonAction() {

        showRangeTiles.ClearAllTiles();
        Debug.Log("Acao do botao de attack");

    }

    public void WaitButtonAction() {

        showRangeTiles.ClearAllTiles();
        matchManager.PlayerUnitMadeAction(unitMoverManager.UnitSelected);
        unitMoverManager.CleanUnitSelected();
        CloseModal();

    }

    public void BackButtonAction() {

        showRangeTiles.ClearAllTiles();
        bool movedBack = unitMoverManager.MoveUnitBack();
        if (movedBack) {
            CloseModal();
        }

    }

}
