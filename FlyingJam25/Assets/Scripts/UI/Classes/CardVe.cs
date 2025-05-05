using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.VFX;

public partial class CardVE : VisualElement {

    public CardManager card;
    private UnityEvent<CardVE> response;
    private UnityEvent<CardVE> responseR;
    public int index;
    public VisualElement ve;
    public float scale;

    public CardVE(CardManager c, VisualTreeAsset t, float f) {
        this.card = c;
        this.response = new UnityEvent<CardVE>();
        this.responseR = new UnityEvent<CardVE>();
        this.index = 0;
        this.ve = t.CloneTree();
        this.scale = f;

        InitializeVE();
    }

    public CardVE(CardManager c, VisualTreeAsset t, UnityEvent<CardVE> r, UnityEvent<CardVE> rr, float f) {
        this.card = c;
        this.response = r;
        this.responseR = rr;
        this.index = 0;
        this.ve = t.CloneTree();
        this.scale = f;

        InitializeVE();
    }

    public CardVE(CardManager c, VisualTreeAsset t, UnityEvent<CardVE> r, int id, float f) {
        card = c;
        response = r;
        responseR = new UnityEvent<CardVE>();
        index = id;
        ve = t.CloneTree();
        scale = f;

        InitializeVE();
    }

    private void InitializeVE() {
        Label cardNameLabel = ve.Q<Label>("CARD-name");
        cardNameLabel.text = card.baseData.cardName;

        Label effectLabel = ve.Q<Label>("EFFECT");
        //string effectDescription = GetEffectDescription(card);
        if (card.hasEffects) effectLabel.text = card.baseData.effectDescription;
        else effectLabel.text = string.Empty;

        Dictionary<int, Tuple<string,string>> attackMapping = new Dictionary<int, Tuple<string,string>> {
            { 0, new Tuple<string,string>("AA","aaT")}, { 1, new Tuple<string,string>("AC", "acT") },
            { 2,  new Tuple<string,string>("AR","arT") }, { 3,  new Tuple<string,string>("AS","asT") }
        };

        // Define vulnerability order mapping to UI elements
        Dictionary<int, Tuple<string,string>> vulnerabilityMapping = new Dictionary<int, Tuple<string,string>> {
            { 0,  new Tuple<string,string>("DA","daT") }, { 1,  new Tuple<string,string>("DC","dcT") },
            { 2,  new Tuple<string,string>("DR", "drT") }, { 3,  new Tuple<string,string>("DS","dsT") }
        };

        // Assign Attack Values
        for (int i = 0; i < card.attackValues.Length; i++) {
            if (i < attackMapping.Count) {
                string labelName = attackMapping[i].Item2;

                Label attackLabel = ve.Q<Label>(labelName);
                if (attackLabel != null) {
                    if (card.attackValues[i] > 0) {
                        attackLabel.text = card.attackValues[i].ToString();
                        //attackLabel.style.color = new StyleColor(Color.red);
                    }
                    else {
                        attackLabel.text = string.Empty;
                        var a = ve.Q<VisualElement>(attackMapping[i].Item1);
                        a.style.unityBackgroundImageTintColor = new StyleColor(new Color(1,1,1,0.5f));
                    }
                }
            }
        }

        for (int i = 0; i < card.vulnerabilityValues.Length; i++) {
            if (i < vulnerabilityMapping.Count) {
                string labelName = vulnerabilityMapping[i].Item2;

                Label vulnerabilityLabel = ve.Q<Label>(labelName);
                if (vulnerabilityLabel != null) {
                    if (card.vulnerabilityValues[i] > 0) {
                        vulnerabilityLabel.text = card.vulnerabilityValues[i].ToString();
                        //vulnerabilityLabel.style.color = new StyleColor(Color.red);
                    }
                    else {
                        vulnerabilityLabel.text = string.Empty;
                        var a = ve.Q<VisualElement>(vulnerabilityMapping[i].Item1);
                        a.style.unityBackgroundImageTintColor = new StyleColor(new Color(1, 1, 1, 0.5f));
                    }
                }
            }
        }

        VisualElement pictureElement = ve.Q<VisualElement>("Picture");
        if (pictureElement != null && card.baseData.sprite != null) {
            pictureElement.style.backgroundImage = new StyleBackground(card.baseData.sprite.texture);
        }

        Button cudl = ve.Q<Button>("Cudl");
        cudl.clicked += () => response.Invoke(this);
        cudl.RegisterCallback<PointerUpEvent>(e =>
        {
            if (e.button == (int)MouseButton.RightMouse) {
                responseR.Invoke(this);
            }
        });

        //ve.style.transformOrigin = new StyleTransformOrigin(new TransformOrigin(0, 0));
        //ve.transform.scale = new Vector3(scale, scale, 0.0f);
        //ve.style.width = scale * ConstantValues.cardWidth;
        //ve.style.height = scale * ConstantValues.cardHeight;
        ve.style.marginBottom = ConstantValues.cardMargin;
        ve.style.marginRight = ConstantValues.cardMargin;
    }



    /*private string GetEffectDescription(CardManager card) {
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

    }*/
}
