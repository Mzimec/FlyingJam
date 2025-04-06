using UnityEngine;

public class LoadCaller : MonoBehaviour
{
    [SerializeField] SaveManager manager;

    private void Start() {
        if (manager != null) manager.ApplyLoadedData();
    }

    public void OnTurnStart() {
        manager.SaveGame();
        Debug.Log("Game saved");
    }

}
