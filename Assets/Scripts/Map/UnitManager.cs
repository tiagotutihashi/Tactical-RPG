using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour {

    [SerializeField]
    private Transform playerTransform;
    [SerializeField]
    private Transform enemyTransform;

    [SerializeField]
    private List<Unit> unitlist = new List<Unit>();

    public List<Unit> Unitlist => unitlist;

    private void Awake() {

        GelAllUnits();

    }

    private void GelAllUnits() {

        unitlist.Clear();

        foreach (Transform chiled in playerTransform) {
            Unit unit = chiled.GetComponent<Unit>();
            if (unit != null) {
                unitlist.Add(unit);
            }
        }

        foreach (Transform chiled in enemyTransform) {
            Unit unit = chiled.GetComponent<Unit>();
            if (unit != null) {
                unitlist.Add(unit);
            }
        }

    }

}
