using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private UIDocument mainMenu;
    [SerializeField] private SaveManager saveManager;
    [SerializeField] private NewGameManager newGameManager;

    void Start()
    {
        var mainMenuRoot = mainMenu.rootVisualElement;


        var newGameB = mainMenuRoot.Q<Button>("NewGameB");
        var continueB = mainMenuRoot.Q<Button>("ContinueB");
        var quitB = mainMenuRoot.Q<Button>("QuitB");

        if (newGameB != null) newGameB.clicked += OnNewGame;
        if (continueB != null) continueB.clicked += OnContinueGame;
        if (quitB != null) quitB.clicked += OnQuitGame;

        if (!newGameManager.iReadyForNewGame) {
            mainMenu.gameObject.SetActive(false);
            newGameManager.iReadyForNewGame = true;
        }
    }

    private void OnNewGame() {
        newGameManager.ResetGame();
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
