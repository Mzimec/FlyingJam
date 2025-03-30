using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "GameManager", menuName = "Scriptable Objects/Manager/NewGameManager")]
public class NewGameManager : ScriptableObject {
    [SerializeField] private GameObject mainMenu;
    public bool iReadyForNewGame = true;
    public void ResetGame() {
        iReadyForNewGame = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
