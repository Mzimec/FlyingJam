using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ControlsMenuManager : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    UIDocument ui;
    Button backB;

    private void Awake() {
        ui = GetComponent<UIDocument>();
    }

    private void OnEnable() {
        var root = ui.rootVisualElement;
        backB = root.Q<Button>("BackB");
        StartCoroutine(ConstantValues.DisableButtonsTemporarily(new List<Button> { backB }, ConstantValues.waitTimeOnMenu));
        backB.clicked += OnBack;
    }

    private void OnDisable() {
        backB.clicked -= OnBack;
    }

    private void OnBack() {
        mainMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}
