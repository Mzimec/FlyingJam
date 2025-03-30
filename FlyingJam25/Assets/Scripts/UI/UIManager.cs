using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    [SerializeField] private UIDocument mainMenu;

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

    }

    public void OnContinueGame() {
        mainMenu.gameObject.SetActive(false);    
    }

    public void OnQuitGame() { }
}
