using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PopUpUIManager : MonoBehaviour {
    UIDocument ui;
    [SerializeField] EmptyEvent onStartTurn;
    [SerializeField] PopUp revolution;

    Button backB;
    Label titleL, descriptionL;
    VisualElement imageVE;
    PopUp popUp;

    private void OnEnable() {
        ui = GetComponent<UIDocument>();
        var root = ui.rootVisualElement;
        backB = root.Q<Button>("BackB");
        titleL = root.Q<Label>("Title");
        descriptionL = root.Q<Label>("Description");
        imageVE = root.Q<VisualElement>("Image");

        StartCoroutine(ConstantValues.DisableButtonsTemporarily(new List<Button> { backB }, ConstantValues.waitTimeOnMenu));

        backB.clicked += OnBack;
    }

    private void OnDisable() {
        backB.clicked -= OnBack;
    }

    private void OnBack() {
        gameObject.SetActive(false);
        popUp.Execute();
        onStartTurn.Raise(new Empty());
    }

    public void StartRevolution() {
        popUp = revolution;
        SetPopUp();
    }

    private void SetPopUp() {
        descriptionL.text = popUp.description;
        titleL.text = popUp.title;
        imageVE.style.backgroundImage = popUp.image.texture;
    }
}
