using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;


[CreateAssetMenu(fileName = "SaveManager", menuName = "Scriptable Objects/Managers/SaveManager")]
public class SaveManager : ScriptableObject
{

    private string path => Path.Combine(Application.persistentDataPath, "savegame.json");
    private DataToSave loadedData = null;

    public void SaveGame() {
        List<RegionManager> regions = new List<RegionManager>(FindObjectsByType<RegionManager>(FindObjectsSortMode.InstanceID));
        PlayerManager player = FindFirstObjectByType<PlayerManager>();
        if (player == null || regions.Count != ConstantValues.regionCount) return;
        DataToSave dataToSave = new DataToSave(regions, player);
        string json = JsonUtility.ToJson(dataToSave, true);
        File.WriteAllText(path, json);
    }

    public void LoadGame() {
        if (!File.Exists(path)) {
            Debug.LogWarning("Save file not found!");
            return;
        }

        string json = File.ReadAllText(path);
        loadedData = JsonUtility.FromJson<DataToSave>(json);
    }

    public void ApplyLoadedData() {
        if (loadedData == null) return;

        List<RegionManager> regions = new List<RegionManager>(FindObjectsByType<RegionManager>(FindObjectsSortMode.InstanceID));
        PlayerManager player = FindFirstObjectByType<PlayerManager>();

        for (int i = 0; i < regions.Count; i++) {
            regions[i].Load(loadedData.regions[i]);
        }

        player.Load(loadedData.player, regions);
        loadedData = null;

    }

    public void CleanData() {
        loadedData = null;
    }
}
