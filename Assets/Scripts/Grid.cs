using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

    [SerializeField]
    private int indexX;
    [SerializeField]
    private int indexY;

    private GameObject unit;

    public int IndexX => indexX;
    public int IndexY => indexY;

    public void SetIndex(int x, int y) {

        indexX = x;
        indexY = y;

    }

}
