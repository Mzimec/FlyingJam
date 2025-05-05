using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChangeValuesEffect", menuName = "Scriptable Objects/Effects/ChangeValuesEffect")]
public class ChangeValuesEffect : Effect {
    [SerializeField] protected int value;
    [SerializeField] protected int typeTo;
    [SerializeField] private EPredicateType p;
    [SerializeField] private PredicateSelector predicateSelector;
    protected override void EffectAction(CardManager card, List<CardManager> allies, List<CardManager> enemies, int idx) {
        ChangeValue(card);
    }

    protected void ChangeValue(CardManager card) {
        if (typeTo < 4) {
            if (card.attackValues.Length > typeTo) {
                if (!predicateSelector.Predicate(p, card.attackValues[typeTo])) return;
                card.attackValues[typeTo] += value;
                if (card.attackValues[typeTo] < 0) {
                    card.attackValues[typeTo] = 0;
                }
            }
        }
        else {
            typeTo -= 4;
            if (card.vulnerabilityValues.Length > typeTo) {
                if (!predicateSelector.Predicate(p, card.vulnerabilityValues[typeTo])) return;
                card.vulnerabilityValues[typeTo] += value;
                if (card.vulnerabilityValues[typeTo] < 0) {
                    card.vulnerabilityValues[typeTo] = 0;
                }
            }
        }
    }
}
