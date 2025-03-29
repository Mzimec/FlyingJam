
using System;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour {
    [SerializeField] private CardSO baseData;
    public bool hasEffects = false;
    public bool hasAttack = false;
    public int[] attackValues;
    public int[] vulnerabilityValues;
    private List<Effect> effects;

    private void Awake() {
        if (baseData == null) return;
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

    public void ApplyEffects(CardManager[] allies, CardManager[] enemies) {
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
