using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

[CreateAssetMenu(fileName = "MouseInputManager", menuName = "Scriptable Objects/Managers/MouseInputManager")]
public class MouseInputManager : ScriptableObject {
    private Camera mainCamera;
    private UIBlocker uiBlocker;
    public RaycastHit2D hit;
    public RaycastHit2D clickableHit;

    public void Initialize() {
        if (mainCamera == null) mainCamera = Camera.main;
        if (uiBlocker == null) uiBlocker = FindAnyObjectByType<UIBlocker>();
    }

    public void Update() {
        if (mainCamera == null) return;
        if (uiBlocker != null && uiBlocker.CheckPointerOverUI()) {
            hit = new RaycastHit2D();
            clickableHit = new RaycastHit2D();
            return;
        }

        Vector2 worldPoint = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        hit = Physics2D.Raycast(worldPoint, Vector2.zero, 0f, 1 << 6);
        clickableHit = Physics2D.Raycast(worldPoint,Vector2.zero, 0f, 1 << 7);
    }

    public void OnClick(InputAction.CallbackContext context) {
        if (!context.performed || clickableHit.collider == null) return;

        if (uiBlocker == null) uiBlocker = FindAnyObjectByType<UIBlocker>();
        if (uiBlocker != null && uiBlocker.CheckPointerOverUI()) return;

        IsClickable clickable = clickableHit.collider.gameObject.GetComponent<IsClickable>();
        if (clickable != null) {
            Debug.Log("Click");
            clickable.OnClick(context.control);
        }
        else Debug.Log("No Clickable on Collider");
    }
}
