using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraController : MonoBehaviour {

    public Tilemap map;

    private Vector3 bottomLeftLimite;
    private Vector3 topRightLimite;

    private float halfHeight;
    private float halfWidth;

    private PlayerInput playerInput;

    [SerializeField]
    private float cameraSpeed = 10f;

    private void Awake() {
        playerInput = new PlayerInput();
    }

    private void OnEnable() {
        playerInput.Enable();
    }

    private void OnDisable() {
        playerInput.Disable();
    }

    private void Start() {

        if (map != null) {

            //Pegando metado do tamanho da câmera
            halfHeight = Camera.main.orthographicSize;
            halfWidth = halfHeight * Camera.main.aspect;
            //Pegas os limite do tilemap da cena limitando com o tamanho da câmera
            bottomLeftLimite = map.localBounds.min + new Vector3(halfWidth, halfHeight, 0f);
            topRightLimite = map.localBounds.max + new Vector3(-halfWidth, -halfHeight, 0f);

        }

    }

    void LateUpdate() {

        Move(playerInput.Camera.Move.ReadValue<Vector2>());

    }

    private void Move(Vector2 value) {

        if (map != null && (value.x != 0 || value.y != 0)) {
            transform.position = new Vector3(
                transform.position.x + value.x * Time.deltaTime * cameraSpeed,
                transform.position.y + value.y * Time.deltaTime * cameraSpeed,
                transform.position.z
            );
            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, bottomLeftLimite.x, topRightLimite.x),
                Mathf.Clamp(transform.position.y, bottomLeftLimite.y, topRightLimite.y),
                transform.position.z
            );
        }

    }

}
