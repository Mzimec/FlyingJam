using UnityEngine;

public class LoadCaller : MonoBehaviour
{
    [SerializeField] SaveManager manager;

    private void Awake() {
        if (manager != null) manager.ApplyLoadedData();
    }
}
