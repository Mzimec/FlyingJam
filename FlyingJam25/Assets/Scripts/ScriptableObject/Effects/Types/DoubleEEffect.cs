using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DoubleEEffect", menuName = "Scriptable Objects/Effects/DoubleEEffect")]
public class DoubleEEffect : Effect {
    protected override void EffectAction(CardManager card, List<CardManager> allies, List<CardManager> enemies, int idx) {
        DoubleEffects(card);
    }

    private void DoubleEffects(CardManager card) {
        var newEffects = new List<Effect>(card.effects);
        card.effects.AddRange(newEffects);
    }
}
