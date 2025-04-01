using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class UIBlocker : MonoBehaviour
{
    [SerializeField] private List<UIDocument> uiDocuments;

    public bool CheckPointerOverUI() {
        Vector2 mousePosition = Mouse.current.position.ReadValue();

        foreach (var uiDocument in uiDocuments) {
            if (uiDocument == null || !uiDocument.gameObject.activeInHierarchy) continue; // Skip inactive UI Documents

            var root = uiDocument.rootVisualElement;
            if (root.panel == null) continue; // Skip if panel is null

            Vector2 localPosition = root.WorldToLocal(mousePosition);

            if (root.panel.Pick(localPosition) != null) {
                return true;
            }
        }
        return false;
    }
}
