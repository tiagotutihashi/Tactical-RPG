using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TurnState { START, PLAYERTURN, ENEMYTURN, WON, LOST, FLED }

public class MatchManager : MonoBehaviour {

    [SerializeField]
    private TurnState turnState;

    public void StartMatch() {
        // TODO  
    }



}
