using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGrid : MonoBehaviour {

    [SerializeField]
    private int indexX;
    [SerializeField]
    private int indexY;

    [SerializeField]
    private Unit unit;
    public Unit Unit => unit;

    public int IndexX => indexX;
    public int IndexY => indexY;

    public void SetIndex(int x, int y) {

        indexX = x;
        indexY = y;

    }

    public void SetUnit(Unit unit) {
        this.unit = unit;
    }

}
