using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class EndMenuManager : MonoBehaviour
{
    UIDocument ui;
    [SerializeField] PlayerManager player;

    Label score;
    Button quitB, toMenuB;

    private void Awake() {
        ui = GetComponent<UIDocument>();
    }

    private void OnEnable() {
        var root = ui.rootVisualElement;
        score = root.Q<Label>("Score");
        quitB = root.Q<Button>("QuitB");
        toMenuB = root.Q<Button>("MenuB");

        StartCoroutine(ConstantValues.DisableButtonsTemporarily(new List<Button> { quitB, toMenuB }, ConstantValues.waitTimeOnMenu));

        score.text = $"Your conquest of Russia took {player.turn} months.\n\nYou gained {player.score} fame during that time.";

        if (quitB != null) quitB.clicked += OnQuit;
        if (toMenuB != null) toMenuB.clicked += OnToMenu;
    }

    void OnDisable() {
        if (quitB != null) quitB.clicked -= OnQuit;
        if (toMenuB != null) toMenuB.clicked -= OnToMenu;
    }

    private void OnQuit() {
#if UNITY_EDITOR
        // Stop play mode in the editor
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Quit the application in a build
        Application.Quit();
#endif

    }

    private void OnToMenu() {
        SceneManager.LoadScene("MainMenu");
    }
}
