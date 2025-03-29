using UnityEngine;
using UnityEngine.InputSystem;

public class CursorInput : MonoBehaviour {
    public float cursorSpeed = 10f; // How fast the cursor follows the mouse
    private Vector2 mousePosition;

    void Start() {
        // Hide the default system cursor
        Cursor.visible = false;
    }

    private void Update() {
        // Convert the mouse position to world space
        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane));

        // Ensure the cursor stays on the same Z position as your cursor sprite
        targetPosition.z = 0;

        // Smoothly move the cursor to the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, cursorSpeed * Time.deltaTime);
    }

    public void OnCursorMove(InputAction.CallbackContext context) { 
        mousePosition = context.ReadValue<Vector2>();
    }
}
