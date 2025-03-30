using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class IsClickable : MonoBehaviour
{
    public event Action<InputControl> OnClicked;
    public void OnClick(InputControl button) {
        OnClicked?.Invoke(button);
    }
}
