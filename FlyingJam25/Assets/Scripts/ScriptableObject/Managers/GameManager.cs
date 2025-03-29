using UnityEngine;

[CreateAssetMenu(fileName = "GameManager", menuName = "Scriptable Objects/Manager/GameManager")]
public class GameManager : ScriptableObject {
    public int turn = 1;

    public void OnEndTurn() { 
        turn++;
        Debug.Log($"Start of turn {turn}");
    }
}
