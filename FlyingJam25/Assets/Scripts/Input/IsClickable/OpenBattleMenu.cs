using UnityEngine;
using UnityEngine.InputSystem;

public class OpenBattleMenu : MonoBehaviour
{
    IsClickable clickable;
    BattleManager manager;
    private void Awake() {
        clickable = GetComponent<IsClickable>();
        if (clickable != null) {
            clickable.OnClicked += HandleClick; // Subscribe to the event
        }
    }

    private void OnDestroy() {
        if (clickable != null) {
            clickable.OnClicked -= HandleClick; // Unsubscribe when destroyed
        }
    }

    public void HandleClick(InputControl control) {
        if (control == Mouse.current.leftButton) {
            manager.OpenBattle();
        }
    }
}
