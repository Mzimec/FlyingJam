using UnityEngine;
using UnityEngine.InputSystem;

public class MoveBackgroundInput : MonoBehaviour {
    public float moveSpeed = 5f;

    private Vector2 moveDirection = Vector2.zero;

    private void Update() {
        // Move the camera based on input
        transform.position += (Vector3)(moveDirection * moveSpeed * Time.deltaTime);
    }

    public void OnMoveLeft(InputAction.CallbackContext context) {
        moveDirection.x = context.ReadValue<float>() > 0 ? -1 : 0;
    }

    public void OnMoveRight(InputAction.CallbackContext context) {
        moveDirection.x = context.ReadValue<float>() > 0 ? 1 : 0;
    }
}
