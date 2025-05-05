using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WinEEffect", menuName = "Scriptable Objects/Effects/WinEEffect")]
public class WinEEffect : ChangeValuesEffect {
    protected override void EffectAction(CardManager card, List<CardManager> allies, List<CardManager> enemies, int idx) {
        WinBattle(allies, enemies);
    }

    private void WinBattle(List<CardManager> allies, List<CardManager> enemies) {
        int[] sumAlly = new int[ConstantValues.cardTypesCount];
        int[] sumEnemy = new int[ConstantValues.cardTypesCount];
        foreach (var enemy in enemies) {
            for (int i = 0; i < ConstantValues.cardTypesCount; i++) {
                sumEnemy[i] += enemy.attackValues[i];
            }
        }
        foreach (var ally in allies) {
            for (int i = 0; i < ConstantValues.cardTypesCount; i++) {
                sumAlly[i] += ally.vulnerabilityValues[i];
            }
        }
        if (sumAlly == sumEnemy) {
            foreach (var ally in allies) {
                ChangeValue(ally);
            }
        }
    }
}
