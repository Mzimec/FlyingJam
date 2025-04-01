
using System;
using System.Collections.Generic;
using UnityEngine;

public class CardManager {
    public CardSO baseData;
    public bool hasEffects = false;
    public bool hasAttack = false;
    public int[] attackValues;
    public int[] vulnerabilityValues;
    public List<Effect> effects;


    public CardManager(CardData card) {
        baseData = card.baseData;
        hasEffects = card.hasEffects;
        hasAttack = card.hasAttack;
        attackValues = new int[card.attackValues.Length];
        vulnerabilityValues = new int[card.vulnerabilityValues.Length];
        card.attackValues.CopyTo(attackValues, 0);
        card.vulnerabilityValues.CopyTo(vulnerabilityValues, 0);
        effects = new List<Effect>(card.effects);
    }
    public CardManager(CardSO baseCard) {
        baseData = baseCard;
        if (baseData.attackValues != null) hasAttack = true;
        if (baseData.effects != null) hasEffects = true;
        ResetCard();
    }

    public void AddEffect(Effect effect) { 
        effects.Add(effect);
    }

    public void RemoveEffect(Effect effect) {
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

        if(hasEffects) effects = new List<Effect>(baseData.effects);
        else effects = null;
    }

    public void ApplyEffects(List<CardManager> allies, List<CardManager> enemies) {
        foreach (var effect in effects) {
            if (effect.side == ESide.ALLY || effect.side == ESide.BOTH) {
                foreach (var card in allies) effect.Execute(card);
            }
            if (effect.side == ESide.ENEMY || effect.side == ESide.BOTH) {
                foreach(var card in enemies) effect.Execute(card);
            }
        }
    }
}
