using UnityEngine;
using UnityEngine.InputSystem;

public class CameraEdgeScrollingInput : MonoBehaviour {
    public float moveSpeed = 5f; // How fast the camera moves
    public float edgeThreshold = 10f; // How close to the edge the cursor should be

    private Vector2 mousePosition;

    private void Update() {
        Vector3 moveDirection = Vector3.zero;

        // Check if the mouse is near the edge of the screen
        if (mousePosition.x <= edgeThreshold) moveDirection.x = -1; // Move left
        if (mousePosition.x >= Screen.width - edgeThreshold) moveDirection.x = 1; // Move right
        if (mousePosition.y <= edgeThreshold) moveDirection.y = -1; // Move down
        if (mousePosition.y >= Screen.height - edgeThreshold) moveDirection.y = 1; // Move up

        // Move the camera
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    // Called when the Input System updates mouse position
    public void OnCursorMove(InputAction.CallbackContext context) {
        mousePosition = context.ReadValue<Vector2>(); // Get the mouse position
    }
}
