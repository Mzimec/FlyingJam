using UnityEngine;
using UnityEngine.InputSystem;

public class TurnEnder : MonoBehaviour {
    private IsClickable clickable;
    [SerializeField] EmptyEvent onTurnEnd;
    [SerializeField] GameManager gameManager;

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

    private void HandleClick(InputControl button) {
        if (button == Mouse.current.leftButton) {
            Debug.Log("EndTurn");
            OnTurnEnd();
        }
        else if (button == Mouse.current.rightButton) {
            Debug.Log("Cancel or Show Options (Right Click)");
        }
    }

    private void OnTurnEnd() {
        gameManager.OnEndTurn();
        onTurnEnd.Raise(new Empty());
    }
}
