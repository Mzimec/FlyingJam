using UnityEngine;

public class EventManager : MonoBehaviour
{
    [SerializeField] EmptyEvent onAllBattlesCleared;
    [SerializeField] EmptyEvent onAllBattlesGenerated;
    int battlesCleared = 0;
    int battlesGenerated = 0;
    public void OnBattleCleared() {
        battlesCleared++;
        if (battlesCleared == 30) {
            onAllBattlesCleared.Raise(new Empty());
            battlesCleared = 0;
        }
    }

    public void OnBattleGenerated() {
        battlesGenerated++;
        if (battlesGenerated == 30) {
            onAllBattlesGenerated.Raise(new Empty());
            battlesGenerated = 0;
        }
    }
}
