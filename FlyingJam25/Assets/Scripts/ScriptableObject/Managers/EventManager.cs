using UnityEngine;

public class EventManager : MonoBehaviour
{
    [SerializeField] EmptyEvent onAllBattlesCleared;
    int battlesCleared = 0;
    public void OnBattleCleared() {
        battlesCleared++;
        if (battlesCleared == 30) {
            onAllBattlesCleared.Raise(new Empty());
            battlesCleared = 0;
        }
    }
}
