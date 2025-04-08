using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuManager : MonoBehaviour
{
    private UIDocument mainMenu;
    [SerializeField] private GameObject controlsMenu;
    [SerializeField] private SaveManager saveManager; 

    Button newGameB, loadB, controlsB, quitB;

    private void Awake() {
        mainMenu = GetComponent<UIDocument>();
    }

    void OnEnable()
    {
        var mainMenuRoot = mainMenu.rootVisualElement;

        newGameB = mainMenuRoot.Q<Button>("NewGameB");
        loadB = mainMenuRoot.Q<Button>("ContinueB");
        controlsB = mainMenuRoot.Q<Button>("ControlsB");
        quitB = mainMenuRoot.Q<Button>("QuitB");

        StartCoroutine(ConstantValues.DisableButtonsTemporarily(new List<Button> { newGameB, loadB, quitB, controlsB }, ConstantValues.waitTimeOnMenu));

        if (newGameB != null) newGameB.clicked += OnNewGame;
        if (loadB != null) loadB.clicked += OnContinueGame;
        if (quitB != null) quitB.clicked += OnQuitGame;
        if (controlsB != null) controlsB.clicked += OnControls;
    }

    private void OnDisable() {
        if (newGameB != null) newGameB.clicked -= OnNewGame;
        if (loadB != null) loadB.clicked -= OnContinueGame;
        if (quitB != null) quitB.clicked -= OnQuitGame;
        if (controlsB != null) controlsB.clicked -= OnControls;
    }

    private void Update() {
        
    }

    private void OnNewGame() {
        newGameB.SetEnabled(false);
        loadB.SetEnabled(false);
        quitB.SetEnabled(false);
        controlsB.SetEnabled(false);
        saveManager.CleanData();
        SceneManager.LoadScene("Game");
    }

    public void OnContinueGame() {
        newGameB.SetEnabled(false);
        loadB.SetEnabled(false);
        quitB.SetEnabled(false);
        controlsB.SetEnabled(false);
        saveManager.LoadGame();
        SceneManager.LoadScene("Game"); ;    
    }

    public void OnControls() { 
        controlsMenu.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    public void OnQuitGame() {
        newGameB.SetEnabled(false);
        loadB.SetEnabled(false);
        quitB.SetEnabled(false);
        controlsB.SetEnabled(false);
        // Quit the application
#if UNITY_EDITOR
        // Stop play mode in the editor
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Quit the application in a build
        Application.Quit();
#endif

    }
}
