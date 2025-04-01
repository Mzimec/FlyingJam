using UnityEngine;
using UnityEngine.UIElements;

public class VictoryMenuManager : MonoBehaviour
{
    private UIDocument ui;
    private BattleManager battleManager;
    public string message;

    Label victoryL, pointsL, recruitL;
    Button cancelB;

    private void Awake() {
        ui = GetComponent<UIDocument>();
    }

    private void OnEnable() {
        var root = ui.rootVisualElement;
        
        victoryL = root.Q<Label>("V-D-text");
        pointsL = root.Q<Label>("Points");
        recruitL = root.Q<Label>("Recruit");

        cancelB = root.Q<Button>("CancleB");
        if (cancelB != null) cancelB.clicked += OnCancel;
    }

    private void OnDisable() {
        if(cancelB != null) cancelB.clicked += OnCancel;
    }

    private void OnCancel() {
        battleManager.region.ClearBattleEvent();
        gameObject.SetActive(false);
    }

    public void SetBattleManager(BattleManager b) {
        battleManager = b;
        WriteOutcomes();
    }

    private void WriteOutcomes() {
        victoryL.text = message;
        pointsL.text = $"Attackers: {battleManager.aPoints} points.\n Defenders {battleManager.dPoints} points.";
        recruitL.text = $"New Recruit Value is: {battleManager.region.recruitPoints}.";
    }

}
