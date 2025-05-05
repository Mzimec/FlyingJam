using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TransformValuesEffect", menuName = "Scriptable Objects/Effects/TransformValuesEffect")]
public class TransformValuesEffect : ChangeValuesEffect {
    [SerializeField] private int typeFrom;
    protected override void EffectAction(CardManager card, List<CardManager> allies, List<CardManager> enemies, int idx) {
        TransformValues(card);
    }

    public void TransformValues(CardManager card) {
        int value = 0;
        if (typeFrom < 4) {
            if (card.attackValues.Length > typeFrom) {
                value = card.attackValues[typeFrom];
                card.attackValues[typeFrom] = 0;
            }
        }
        else {
            typeFrom -= 4;
            if (card.vulnerabilityValues.Length > typeFrom) {
                value = card.vulnerabilityValues[typeFrom];
                card.vulnerabilityValues[typeFrom] = 0;
            }
        }

        ChangeValue(card);
    }
}
