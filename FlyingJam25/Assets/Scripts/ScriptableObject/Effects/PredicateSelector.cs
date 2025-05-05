using UnityEngine;

public enum EPredicateType {
    Zero,
    NonZero,
    Any
}

[CreateAssetMenu(fileName = "PredicateSelector", menuName = "Scriptable Objects/PredicateSelector")]
public class PredicateSelector : ScriptableObject
{
    public bool Predicate(EPredicateType e, int value) {
        switch (e) {
            case EPredicateType.Zero:
                return value == 0;
            case EPredicateType.NonZero:
                return value > 0;
            case EPredicateType.Any:
                return true;
            default:
                return false;
        }
    }
}
