using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class MouseInputManager : MonoBehaviour {
    private Camera mainCamera;
    public RaycastHit2D hit;

    private void Awake() {
        mainCamera = Camera.main;  // Reference to the main camera
    }

    private void Update() {
        Vector2 worldPoint = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
         hit = Physics2D.Raycast(worldPoint, Vector2.zero);
    }

    public void OnClick(InputAction.CallbackContext context) {
        if (context.performed) {
            // Perform a raycast to check if the click is over the TurnEnder object
            /*Vector2 worldPoint = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);*/

            if (hit.collider != null) {
                IsClickable clickable = hit.collider.gameObject.GetComponent<IsClickable>();
                if (clickable != null) clickable.OnClick(context.control);
            }
        }
    }
}
