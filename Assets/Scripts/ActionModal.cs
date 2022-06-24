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

    private void Awake() {
        unitManager = FindObjectOfType<UnitManager>();
        unitMoverManager = FindObjectOfType<UnitMoverManager>();
        matchManager = FindObjectOfType<MatchManager>();
    }

    public bool IsModalActive() {
        return buttonContainer.activeInHierarchy;
    }

    public void ShowModal() {
        buttonContainer.SetActive(true);
    }

    public void CloseModal() {
        buttonContainer.SetActive(false);
    }

    public void AttackButtonAction() {
        //TODO
        Debug.Log("Acao do botao de attack");
    }

    public void WaitButtonAction() {
        // TODO
        Debug.Log("Acao do botao de wait");
        matchManager.PlayerUnitMadeAction(unitMoverManager.UnitSelected);
        unitMoverManager.CleanUnitSelected();
        CloseModal();
    }

    public void BackButtonAction() {
        // TODO
        Debug.Log("Acao do botao de back");
        bool movedBack = unitMoverManager.MoveUnitBack();
        if (movedBack) {
            CloseModal();
        }
    }

}
