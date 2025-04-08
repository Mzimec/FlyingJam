using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class IsClickable : MonoBehaviour
{
    public event Action<InputControl> OnClicked;
    [SerializeField] MouseInputManager hitManager;
    public bool isEnabled = true;

    public void OnClick(InputControl button) {
        if (!isEnabled) return;
        if (hitManager.clickableHit.collider == this.GetComponent<Collider2D>()) {
            OnClicked?.Invoke(button);
        }
        else Debug.Log("No Collider On Clicked Object");        
    }
}
