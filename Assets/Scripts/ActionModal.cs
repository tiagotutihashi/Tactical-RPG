using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionModal : MonoBehaviour {

    [SerializeField]
    private GameObject buttonContainer;

    public bool IsModalActive(){
        return buttonContainer.activeInHierarchy;
    }

    public void ShowModal() {
        buttonContainer.SetActive(true);
    }

    public void CloseModal() {
        buttonContainer.SetActive(false);
    }

    public void AttackButtonAction(){
        //TODO
        Debug.Log("Acao do botao de attack");
    }

    public void WaitButtonAction(){
        // TODO
        Debug.Log("Acao do botao de wait");
    }

    public void BackButtonAction(){
        // TODO
        Debug.Log("Acao do botao de back");
    }

}
