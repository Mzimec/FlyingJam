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
        SceneManager.LoadScene("Game");
    }

    public void OnContinueGame() {
        saveManager.LoadGame();
        SceneManager.LoadScene("Game"); ;    
    }

    public void OnControls() { 
        controlsMenu.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    public void OnQuitGame() {
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
