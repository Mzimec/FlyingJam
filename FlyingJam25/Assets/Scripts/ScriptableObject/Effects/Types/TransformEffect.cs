using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TransformEffect", menuName = "Scriptable Objects/Effects/TransformEffect")]
public class TransformEffect : Effect {
    [SerializeField] private bool transformToEnemy;
    protected override void EffectAction(CardManager card, List<CardManager> allies, List<CardManager> enemies, int idx) {
        if (transformToEnemy) TransformCard(card, enemies, idx);
        else TransformCard(card, allies, idx);
    }

    private void TransformCard(CardManager card, List<CardManager> cards, int idx) {
        card.attackValues = new int[cards[idx].attackValues.Length];
        Array.Copy(cards[idx].attackValues, card.attackValues, cards[idx].attackValues.Length);
        card.vulnerabilityValues = new int[cards[idx].vulnerabilityValues.Length];
        Array.Copy(cards[idx].vulnerabilityValues, card.vulnerabilityValues, cards[idx].vulnerabilityValues.Length);
        card.effects = new List<Effect>(cards[idx].effects);
    }
}
