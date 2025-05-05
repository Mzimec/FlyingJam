
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "EffectManager", menuName = "Scriptable Objects/Managers/EffectManager")]
public class EffectManager : ScriptableObject {
    public void EffectAll(UnityAction<dynamic> action, List<CardManager> cards) {
        foreach (var card in cards) {
            action.Invoke(card);
        }
    }

    public void EffectOposing(UnityAction<dynamic> action, List<CardManager> cards, int idx) {
        for (int i = 0; i < cards.Count; i++) {
            if (i == idx) {
                action.Invoke(cards[i]);
            }
        }
    }

    public void EffectNeighbours(UnityAction<dynamic> action, List<CardManager> cards, int idx) {
        for (int i = 0; i < cards.Count; i++) {
            if (i == idx - 1 || i == idx + 1) {
                action.Invoke(cards[i]);
            }
        }
    }

    public void EffectFront(UnityAction<dynamic> action, List<CardManager> cards, int idx) {
        for (int i = 0; i < cards.Count; i++) {
            if (i == idx + -1) {
                action.Invoke(cards[i]);
            }
        }
    }

    public void EffectBack(UnityAction<dynamic> action, List<CardManager> cards, int idx) {
        for (int i = 0; i < cards.Count; i++) {
            if (i == idx + 1) {
                action.Invoke(cards[i]);
            }
        }
    }

    public void EffectAllFront(UnityAction<dynamic> action, List<CardManager> cards, int idx) {
        for (int i = 0; i < cards.Count; i++) {
            if (i < idx) {
                action.Invoke(cards[i]);
            }
        }
    }

    public void EffectAllBack(UnityAction<dynamic> action, List<CardManager> cards, int idx) {
        for (int i = 0; i < cards.Count; i++) {
            if (i > idx) {
                action.Invoke(cards[i]);
            }
        }
    }

    public void KillEnenemy(CardManager card) {
        card.attackValues = new int[ConstantValues.cardTypesCount];
        card.effects.Clear();
    }

    public void ChangeValue(CardManager card, int value, int type, bool p) {
        if (!p) return;
        if (type < 5) {
            if (card.attackValues.Length > type) {
                card.attackValues[type] += value;
                if (card.attackValues[type] < 0) {
                    card.attackValues[type] = 0;
                }
            }
        }
        else {
            type -= 4;
            if (card.vulnerabilityValues.Length > type) {
                card.vulnerabilityValues[type] += value;
                if (card.vulnerabilityValues[type] < 0) {
                    card.vulnerabilityValues[type] = 0;
                }
            }
        }
    }

    public void TransformCard(CardManager card, List<CardManager> cards, int idx) {
        card.attackValues = new int[cards[idx].attackValues.Length];
        Array.Copy(cards[idx].attackValues, card.attackValues, cards[idx].attackValues.Length);
        card.vulnerabilityValues = new int[cards[idx].vulnerabilityValues.Length];
        Array.Copy(cards[idx].vulnerabilityValues, card.vulnerabilityValues, cards[idx].vulnerabilityValues.Length);
        card.effects = new List<Effect>(cards[idx].effects);
    }

    public void TransformValues(CardManager card, int typeFrom, int typeTo) {
        int value = 0;
        if (typeFrom < 5) {
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

        ChangeValue(card,value, typeTo, true);
    }

    public void IncreaseValueByMe(CardManager card, CardManager source, int type, bool p) {
        int value = 0;
        if (type < 5) {
            value = source.attackValues[type];
        }
        else {
            type -= 4;
            value = source.vulnerabilityValues[type];
        }

        ChangeValue(card, value, type, p);
    }

    public void DecreaseValueByMe(CardManager card, CardManager source, int type, bool p) {
        int value = 0;
        if (type < 5) {
            value = source.attackValues[type];
        }
        else {
            type -= 4;
            value = source.vulnerabilityValues[type];
        }
        ChangeValue(card, -value, type, p);
    }

    public void DoubleEffects(CardManager card) {
        var newEffects = new List<Effect>(card.effects);
        card.effects.AddRange(newEffects);
    }
}

