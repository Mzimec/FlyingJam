using UnityEngine;

public class Persistance : MonoBehaviour
{
    public static Persistance Instance;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject); // Avoid duplicates
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Persist between scenes
    }
}
