using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class CardVE : VisualElement {
    [System.Obsolete]
    public new class UxmlFactory : UxmlFactory<CardVE, UxmlTraits> { } // Enables usage in UXML

    public VisualTreeAsset someUXML;

    public CardVE() {
        if (someUXML != null) {
            VisualElement content = someUXML.Instantiate();
            Add(content);
        }

    }
}
