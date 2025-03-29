using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IEffect {
    public void Execute(CardManager cards);
}

[CreateAssetMenu(fileName = "NoneTypeEffect", menuName = "Effects/NonTypeEffect")]
public class Effect : ScriptableObject, IEffect {
    [SerializeField] protected int index;
    [SerializeField] protected List<UnityAction<CardManager>> responses;

    public void Execute(CardManager cards) {
        foreach(var response in responses) {
            response.Invoke(cards);
        }
    }
}

public abstract class Effect<T> : Effect {
    [SerializeField] protected T value; 
}

[CreateAssetMenu(fileName = "IntEffect", menuName = "Effects/IntEffect")]
public class IntEffect : Effect<int> { }

[CreateAssetMenu(fileName = "FloatEffect", menuName = "Effects/Floatffect")]
public class FloatEffect : Effect<float> { }
