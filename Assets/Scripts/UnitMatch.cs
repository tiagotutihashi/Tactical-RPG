using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMatch : MonoBehaviour {

    [SerializeField]
    private bool isAlly;
    private bool moved ;

    public bool IsAlly => isAlly;

    public bool Moved => moved;
   
    public void SetMoved(bool newValue){
        moved = newValue;
    }

}
