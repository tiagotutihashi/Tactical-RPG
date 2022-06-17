using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionModal : MonoBehaviour {

    public GameObject buttonContainer;

    public void ShowModal() {
        buttonContainer.SetActive(true);
    }

    public void CloseModal() {
        buttonContainer.SetActive(false);
    }

}
