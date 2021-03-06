using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMover : MonoBehaviour {

    const int unitSpeed = 10;
    private ShowRangeTiles showRangeTiles;

    private void Awake() {
        showRangeTiles = FindObjectOfType<ShowRangeTiles>();
    }

    public IEnumerator MoveUnitTo(List<Vector3Int> path) {

        foreach (Vector3Int newPosition in path) {

            Vector3 startPosition = transform.position;
            Vector3 endPosition = newPosition;
            float travelPersent = 0f;

            while (travelPersent < 1f) {
                travelPersent += Time.deltaTime * unitSpeed;
                transform.position = Vector3.Lerp(startPosition, endPosition, travelPersent);
                yield return new WaitForEndOfFrame();
            }

        }

    }

    public void TeleportUnitTo(Vector3Int destination) {

        transform.position = destination;

    }

}
