using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChangeByThisEffect", menuName = "Scriptable Objects/Effects/ChangeByThisEffect")]
public class ChangeByThisEffect : ChangeValuesEffect {
    [SerializeField] private bool isNegative;
    protected override void EffectAction(CardManager card, List<CardManager> allies, List<CardManager> enemies, int idx) {
        IncreaseValueByMe(card, allies[idx]);
    }

    private void IncreaseValueByMe(CardManager card, CardManager source) {
        if (typeTo < 4) {
            value = source.attackValues[typeTo];
        }
        else {
            typeTo -= 4;
            value = source.vulnerabilityValues[typeTo];
        }
        if (isNegative) value = -value;

        ChangeValue(card);
    }
}
