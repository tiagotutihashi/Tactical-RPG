using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathManager : MonoBehaviour {

    [SerializeField]
    private List<StoredDataTile> storedDataTiles = new List<StoredDataTile>();

    public List<Vector3Int> ReturnUnitPath(StoredDataTile finalTile) {

        List<Vector3Int> path = new List<Vector3Int>();
        StoredDataTile parentTile = finalTile;
        bool isFinal = true;
        if (parentTile == null) {
            return path;
        }
        do {
            path.Add(parentTile.position);
            parentTile = parentTile.parent;
            if (parentTile == null) {
                isFinal = false;
            }
        } while (isFinal);

        path.Reverse();

        return path;
    }

    public List<StoredDataTile> CalcMovement(Vector3Int initialPosition, List<StoredDataTile> storedDataTiles, int amountMove) {

        StoredDataTile startTile = storedDataTiles[0];
        startTile.visited = true;
        startTile.remainMovement = amountMove;

        MinHeap<StoredDataTile> heapTile = new MinHeap<StoredDataTile>(storedDataTiles.Count, x => x.distance);

        heapTile.Add(startTile);

        while (heapTile.Count() > 0) {

            StoredDataTile selectedTile = heapTile.RemoveMin();

            if (selectedTile.remainMovement > 0) {
                List<StoredDataTile> neighbor = storedDataTiles.Where(x => Mathf.Abs(selectedTile.position.x - x.position.x) + Mathf.Abs(selectedTile.position.y - x.position.y) == 1).ToList();

                foreach (StoredDataTile storedDataTile in neighbor) {
                    if (storedDataTile.distance > selectedTile.distance + storedDataTile.landSpeed && selectedTile.remainMovement > storedDataTile.landSpeed - 1) {
                        storedDataTile.parent = selectedTile;
                        storedDataTile.visited = true;
                        storedDataTile.remainMovement = selectedTile.remainMovement - storedDataTile.landSpeed;
                        storedDataTile.distance = selectedTile.distance + storedDataTile.landSpeed;
                        heapTile.Add(storedDataTile);
                    }

                }

                heapTile.BuildHeap();
            }

        }

        storedDataTiles.RemoveAll(x => !x.visited);

        return storedDataTiles;

    }

}
