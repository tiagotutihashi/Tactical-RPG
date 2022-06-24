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

    [SerializeField]
    private List<Unit> playerUnits = new List<Unit>();
    public List<Unit> PlayerUnits => playerUnits;

    [SerializeField]
    private List<Unit> enemyUnits = new List<Unit>();
    public List<Unit> EnemyUnits => enemyUnits;

    private void Awake() {

        GelAllUnits();

    }

    private void GelAllUnits() {

        unitlist.Clear();

        foreach (Transform chiled in playerTransform) {
            Unit unit = chiled.GetComponent<Unit>();
            if (unit != null) {
                playerUnits.Add(unit);
                unitlist.Add(unit);
            }
        }

        foreach (Transform chiled in enemyTransform) {
            Unit unit = chiled.GetComponent<Unit>();
            if (unit != null) {
                enemyUnits.Add(unit);
                unitlist.Add(unit);
            }
        }

    }

    public void PlayerRemoveMoved(){
        playerUnits.ForEach(item => {
            if(item.TryGetComponent<UnitMatch>(out UnitMatch unitMatch)){
                unitMatch.SetMoved(false);
            }
        });
    }

}
