using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum ESide { ALLY, ENEMY, BOTH };
public abstract class Effect : ScriptableObject{
    public ESide side;
    public string description;
    [SerializeField] protected int index;
    public virtual void Execute(CardManager card) { }
}

public abstract class Effect<T> : Effect {
    [SerializeField] protected T value;
    [SerializeField] protected List<UnityAction<CardManager, T>> responses;

    public override void Execute(CardManager cards) {
        foreach (var response in responses) {
            response.Invoke(cards, value);
        }
    }
}

[CreateAssetMenu(fileName = "NoneTypeEffect", menuName = "Effects/NonTypeEffect")]
public class EmptyEffect : Effect<Empty> { }

[CreateAssetMenu(fileName = "IntEffect", menuName = "Effects/IntEffect")]
public class IntEffect : Effect<int> { }

[CreateAssetMenu(fileName = "FloatEffect", menuName = "Effects/Floatffect")]
public class FloatEffect : Effect<float> { }
