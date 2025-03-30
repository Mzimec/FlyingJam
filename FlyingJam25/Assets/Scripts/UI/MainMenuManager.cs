using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private UIDocument mainMenu;
    [SerializeField] private SaveManager saveManager;

    void Start()
    {
        var mainMenuRoot = mainMenu.rootVisualElement;


        var newGameB = mainMenuRoot.Q<Button>("NewGameB");
        var continueB = mainMenuRoot.Q<Button>("ContinueB");
        var quitB = mainMenuRoot.Q<Button>("QuitB");

        if (newGameB != null) newGameB.clicked += OnNewGame;
        if (continueB != null) continueB.clicked += OnContinueGame;
        if (quitB != null) quitB.clicked += OnQuitGame;

    }

    private void OnNewGame() {
        saveManager.ResetGame();
        mainMenu.gameObject.SetActive(false);
    }

    public void OnContinueGame() {
        mainMenu.gameObject.SetActive(false);    
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
