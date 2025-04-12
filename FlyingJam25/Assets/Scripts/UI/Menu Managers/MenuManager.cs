using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MenuManager : MonoBehaviour
{
    private UIDocument mainMenu;
    [SerializeField] private GameObject controlsMenu;

    Button backB, quitTMB, controlsB, quitB;

    private void Awake() {
        mainMenu = GetComponent<UIDocument>();
    }

    void OnEnable()
    {
        var mainMenuRoot = mainMenu.rootVisualElement;

        backB = mainMenuRoot.Q<Button>("BackB");
        quitTMB = mainMenuRoot.Q<Button>("QuitTMB");
        controlsB = mainMenuRoot.Q<Button>("ControlsB");
        quitB = mainMenuRoot.Q<Button>("QuitB");

        StartCoroutine(ConstantValues.DisableButtonsTemporarily(new List<Button> { backB, quitTMB, quitB, controlsB }, ConstantValues.waitTimeOnMenu));

        if (backB != null) backB.clicked += OnBack;
        if (quitTMB != null) quitTMB.clicked += OnQuitTM;
        if (quitB != null) quitB.clicked += OnQuitGame;
        if (controlsB != null) controlsB.clicked += OnControls;
    }

    private void OnDisable() {
        if (backB != null) backB.clicked -= OnBack;
        if (quitTMB != null) quitTMB.clicked -= OnQuitTM;
        if (quitB != null) quitB.clicked -= OnQuitGame;
        if (controlsB != null) controlsB.clicked -= OnControls;
    }

    private void OnBack() {
        quitTMB.SetEnabled(false);
        backB.SetEnabled(false);
        quitB.SetEnabled(false);
        controlsB.SetEnabled(false);
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }

    public void OnQuitTM() {
        quitTMB.SetEnabled(false);
        backB.SetEnabled(false);
        quitB.SetEnabled(false);
        controlsB.SetEnabled(false);
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");    
    }

    public void OnControls() {
        quitTMB.SetEnabled(false);
        backB.SetEnabled(false);
        quitB.SetEnabled(false);
        controlsB.SetEnabled(false);
        controlsMenu.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    public void OnQuitGame() {
        quitTMB.SetEnabled(false);
        backB.SetEnabled(false);
        quitB.SetEnabled(false);
        controlsB.SetEnabled(false);
        Time.timeScale = 1;
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
