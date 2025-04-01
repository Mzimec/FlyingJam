using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class IsClickable : MonoBehaviour
{
    public event Action<InputControl> OnClicked;
    [SerializeField] MouseInputManager hitManager;

    public void OnClick(InputControl button) {
        if (hitManager.hit.collider == this.GetComponent<Collider2D>()) {
            OnClicked?.Invoke(button);
            Debug.Log("Hit Confirmed");
        }
        else Debug.Log("No Collider On Clicked Object");        
    }
}
