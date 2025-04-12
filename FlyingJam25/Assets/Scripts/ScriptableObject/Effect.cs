using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum ESide { ALLY, ENEMY, BOTH };
public abstract class Effect : ScriptableObject{
    public ESide side;
    public string description;
    public virtual void Execute(List<CardManager> allies, List<CardManager> enemies, CardManager source) { }
}

public abstract class Effect<T> : Effect {
    [SerializeField] protected T value;
    [SerializeField] protected List<UnityAction<List<CardManager>, List<CardManager>, CardManager, T>> responses;

    public override void Execute(List<CardManager> allies, List<CardManager> enemies, CardManager source) {
        foreach (var response in responses) {
            response.Invoke(allies, enemies, source, value);
        }
    }
}

[CreateAssetMenu(fileName = "NoneTypeEffect", menuName = "Effects/NonTypeEffect")]
public class EmptyEffect : Effect<Empty> { }

[CreateAssetMenu(fileName = "IntEffect", menuName = "Effects/IntEffect")]
public class IntEffect : Effect<int> { }

[CreateAssetMenu(fileName = "FloatEffect", menuName = "Effects/Floatffect")]
public class FloatEffect : Effect<float> { }
