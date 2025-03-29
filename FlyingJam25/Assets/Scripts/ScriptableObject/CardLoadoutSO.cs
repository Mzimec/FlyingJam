using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardLoadoutSO", menuName = "Scriptable Objects/CardLoadoutSO")]
public class CardLoadoutSO : ScriptableObject {
    [SerializeField] private List<CardSO> cards;

    public int GetLoadoutValue() {
        int value = 0;
        foreach (CardSO card in cards) {
            value += card.value;
        }
        return value;
    }
}
