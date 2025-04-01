using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using UnityEngine.VFX;

public partial class CardVE : VisualElement {

    public CardManager card;
    private UnityEvent<CardVE> response;
    public int index;
    public VisualElement ve;

    public CardVE(CardManager c, VisualTreeAsset t, UnityEvent<CardVE> r, int id) {
        card = c;
        response = r;
        index = id;
        ve = t.CloneTree();

        Label cardNameLabel = ve.Q<Label>("CARD-name");
        cardNameLabel.text = card.baseData.cardName;

        Label effectLabel = ve.Q<Label>("EFFECT");
        string effectDescription = GetEffectDescription(card);

        Dictionary<int, string> attackMapping = new Dictionary<int, string> {
        { 0, "aaT" }, { 1, "acT" }, { 2, "arT" }, { 3, "asT" }
    };

        // Define vulnerability order mapping to UI elements
        Dictionary<int, string> vulnerabilityMapping = new Dictionary<int, string> {
        { 0, "daT" }, { 1, "dcT" }, { 2, "drT" }, { 3, "dsT" }
    };

        // Assign Attack Values
        for (int i = 0; i < card.attackValues.Length; i++) {
            if (i < attackMapping.Count) {
                string labelName = attackMapping[i];

                Label attackLabel = ve.Q<Label>(labelName);
                if (attackLabel != null) {
                    attackLabel.text = card.attackValues[i].ToString();
                }
            }
        }

        for (int i = 0; i < card.vulnerabilityValues.Length; i++) {
            if (i < vulnerabilityMapping.Count) {
                string labelName = vulnerabilityMapping[i];

                Label vulnerabilityLabel = ve.Q<Label>(labelName);
                if (vulnerabilityLabel != null) {
                    vulnerabilityLabel.text = card.vulnerabilityValues[i].ToString();
                }
            }
        }

        VisualElement pictureElement = ve.Q<VisualElement>("Picture");
        if (pictureElement != null && card.baseData.sprite != null) {
            pictureElement.style.backgroundImage = new StyleBackground(card.baseData.sprite.texture);
        }

        Button cudl = ve.Q<Button>("Cudl");
        cudl.clicked += () => response.Invoke(this);
    }

    private string GetEffectDescription(CardManager card) {
        if (card == null) {
            Debug.LogError("Card is null.");
            return string.Empty;
        }

        if (!card.hasEffects) {
            return string.Empty;
        }

        var sb = new System.Text.StringBuilder();

        foreach (var effect in card.effects) {
            sb.AppendLine(effect.description);
        }

        return sb.ToString();

    }
}
