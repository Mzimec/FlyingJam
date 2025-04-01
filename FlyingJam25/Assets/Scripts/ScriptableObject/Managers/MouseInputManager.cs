using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

[CreateAssetMenu(fileName = "MouseInputManager", menuName = "Scriptable Objects/Managers/MouseInputManager")]
public class MouseInputManager : ScriptableObject {
    private Camera mainCamera;
    private UIBlocker uiBlocker;
    public RaycastHit2D hit;

    public void Initialize() {
        if (mainCamera == null) mainCamera = Camera.main;
        if (uiBlocker == null) uiBlocker = FindAnyObjectByType<UIBlocker>();
    }

    public void Update() {
        if (mainCamera == null) return;
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;

        Vector2 worldPoint = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        hit = Physics2D.Raycast(worldPoint, Vector2.zero);
    }

    public void OnClick(InputAction.CallbackContext context) {
        if (!context.performed || hit.collider == null) return;

        if (uiBlocker == null) uiBlocker = FindAnyObjectByType<UIBlocker>();
        if (uiBlocker != null && uiBlocker.CheckPointerOverUI()) return;

        IsClickable clickable = hit.collider.gameObject.GetComponent<IsClickable>();
        if (clickable != null) {
            clickable.OnClick(context.control);
        }
    }
}
