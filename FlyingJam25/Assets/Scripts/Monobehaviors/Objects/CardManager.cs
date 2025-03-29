
using System;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour {
    [SerializeField] private CardSO baseData;
    public bool hasEffects = false;
    public bool hasAttack = false;
    public int[] attackValues;
    public int[] vulnerabilityValues;
    private List<IEffect> effects;

    private void Awake() {
        if (baseData == null) return;
        if (baseData.attackValues != null) hasAttack = true;
        if (baseData.effects != null) hasEffects = true;
        ResetCard();
    }

    public void AddEffect(IEffect effect) { 
        effects.Add(effect);
    }

    public void RemoveEffect(IEffect effect) {
        if (effects.Contains(effect)) effects.Remove(effect);
    }

    public void ClearEffects() { effects.Clear(); }

    public void ResetCard() {
        vulnerabilityValues = baseData.vulnerabilityValues;

        if(hasAttack) {
            attackValues = new int[baseData.attackValues.Length];
            Array.Copy(baseData.attackValues, attackValues, attackValues.Length);
        }
        else attackValues = null;

        if(hasEffects) effects = new List<IEffect>(baseData.effects);
        else effects = null;
    }

    public void ApplyEffects(List<CardManager> cards) {
        foreach (var effect in effects) {
            foreach (var card in cards) effect.Execute(card);
        }
    }
}
