using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum TargetingType {
    All,
    Oposing,
    Neighbours,
    Front,
    Back,
    AllFront,
    AllBack
}
public abstract class Effect : ScriptableObject {
    [SerializeField] protected TargetingType targetingType;
    [SerializeField] protected bool isOnEnemy;


    protected abstract void EffectAction(CardManager target, List<CardManager> allies, List<CardManager> enemies, int idx);

    //[SerializeField] private int value;
    //[SerializeField] private int typeFrom;
    //[SerializeField] private int typeTo;

    public void Execute(List<CardManager> allies, List<CardManager> enemies, int idx) {
        List<CardManager> targetList;
        if (isOnEnemy) targetList = allies;
        else targetList = enemies;
        switch (targetingType) {
            case TargetingType.All:
                EffectAll(EffectAction, targetList, allies, enemies, idx);
                break;
            case TargetingType.Oposing:
                EffectOposing(EffectAction, targetList, allies, enemies, idx);
                break;
            case TargetingType.Neighbours:
                EffectNeighbours(EffectAction, targetList, allies, enemies, idx);
                break;
            case TargetingType.Front:
                EffectFront(EffectAction, targetList, allies, enemies, idx);
                break;
            case TargetingType.Back:
                EffectBack(EffectAction, targetList, allies, enemies, idx);
                break;
            case TargetingType.AllFront:
                EffectAllFront(EffectAction, targetList, allies, enemies, idx);
                break;
            case TargetingType.AllBack:
                EffectAllBack(EffectAction, targetList, allies, enemies, idx);
                break;
        }
    }

    protected void EffectAll(UnityAction<CardManager, List<CardManager>, List<CardManager>, int> action,
        List<CardManager> cards, List<CardManager> allies, List<CardManager> enemies, int idx) {
        foreach (var card in cards) {
            action.Invoke(card, allies, enemies, idx);
        }
    }

    protected void EffectOposing(UnityAction<CardManager, List<CardManager>, List<CardManager>, int> action,
        List<CardManager> cards, List<CardManager> allies, List<CardManager> enemies, int idx) {
        for (int i = 0; i < cards.Count; i++) {
            if (i == idx) {
                action.Invoke(cards[i], allies, enemies, idx);
            }
        }
    }

    protected void EffectNeighbours(UnityAction<CardManager, List<CardManager>, List<CardManager>, int> action,
        List<CardManager> cards, List<CardManager> allies, List<CardManager> enemies, int idx) {
        for (int i = 0; i < cards.Count; i++) {
            if (i == idx - 1 || i == idx + 1) {
                action.Invoke(cards[i], allies, enemies, idx);
            }
        }
    }

    protected void EffectFront(UnityAction<CardManager, List<CardManager>, List<CardManager>, int> action,
        List<CardManager> cards, List<CardManager> allies, List<CardManager> enemies, int idx) {
        for (int i = 0; i < cards.Count; i++) {
            if (i == idx + -1) {
                action.Invoke(cards[i], allies, enemies, idx);
            }
        }
    }

    protected void EffectBack(UnityAction<CardManager, List<CardManager>, List<CardManager>, int> action,
        List<CardManager> cards, List<CardManager> allies, List<CardManager> enemies, int idx) {
        for (int i = 0; i < cards.Count; i++) {
            if (i == idx + 1) {
                action.Invoke(cards[i], allies, enemies, idx);
            }
        }
    }

    protected void EffectAllFront(UnityAction<CardManager, List<CardManager>, List<CardManager>, int> action,
        List<CardManager> cards, List<CardManager> allies, List<CardManager> enemies, int idx) {
        for (int i = 0; i < cards.Count; i++) {
            if (i < idx) {
                action.Invoke(cards[i], allies, enemies, idx);
            }
        }
    }

    protected void EffectAllBack(UnityAction<CardManager, List<CardManager>, List<CardManager>, int> action,
        List<CardManager> cards, List<CardManager> allies, List<CardManager> enemies, int idx) {
        for (int i = 0; i < cards.Count; i++) {
            if (i > idx) {
                action.Invoke(cards[i], allies, enemies, idx);
            }
        }
    }
}







