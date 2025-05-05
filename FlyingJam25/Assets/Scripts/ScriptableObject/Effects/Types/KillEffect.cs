using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "KillEffect", menuName = "Scriptable Objects/Effects/KillEffect")]
public class KillEffect : Effect {

    protected override void EffectAction(CardManager card, List<CardManager> allies, List<CardManager> enemies, int idx) {
        KillEnenemy(card);
    }

    private void KillEnenemy(CardManager card) {
        card.attackValues = new int[ConstantValues.cardTypesCount];
        card.effects.Clear();
    }
}
