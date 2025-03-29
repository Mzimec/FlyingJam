using UnityEngine;

public class GameManager : ScriptableObject {
    public int turn = 0;

    public void OnEndTurn() { 
        turn++;
        Debug.Log($"Start of turn {turn}");
    }
}
