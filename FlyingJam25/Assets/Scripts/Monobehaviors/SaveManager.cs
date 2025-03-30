using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    private string path;
    private List<RegionManager> regions;
    private PlayerManager player;

    private void Awake() {
        path = Path.Combine(Application.persistentDataPath, "savegame.json");
        regions = new List<RegionManager>(FindObjectsByType<RegionManager>(FindObjectsSortMode.None));
        player = GetComponent<PlayerManager>();
    }

    public void SaveGame() { 
        DataToSave data = new DataToSave(regions, player);
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);

    }

    public void LoadGame() {
        if (!File.Exists(path)) {
            Debug.LogWarning("Save file not found!");
            return;
        }

        string json = File.ReadAllText(path);
        DataToSave data = JsonUtility.FromJson<DataToSave>(json);

        for (int i = 0; i < regions.Count; i++) {
            regions[i].Load(data.regions[i]);
        }

        player.Load(data.player, regions);
    }

    public void ResetGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
