using UnityEngine;
using UnityEngine.UIElements;

public class VictoryMenuManager : MonoBehaviour
{
    private UIDocument ui;
    private BattleManager battleManager;
    public string message;

    Label victoryL, fpl, rpl, msl,
        paa, pac, par, pas, pda, pdc, pdr, pds,
        eaa, eac, ear, eas, eda, edc, edr, eds;
    Button cancelB;

    private void Awake() {
        ui = GetComponent<UIDocument>();
    }

    private void OnEnable() {
        var root = ui.rootVisualElement;
        
        victoryL = root.Q<Label>("V-D-text");
        fpl = root.Q<Label>("FPL");
        rpl = root.Q<Label>("RPL");
        msl = root.Q<Label>("MSL");

        paa = root.Q<Label>("PAA");
        pac = root.Q<Label>("PAC");
        par = root.Q<Label>("PAR");
        pas = root.Q<Label>("PAS");

        pda = root.Q<Label>("PDA");
        pdc = root.Q<Label>("PDC");
        pdr = root.Q<Label>("PDR");
        pds = root.Q<Label>("PDS");

        eaa = root.Q<Label>("EAA");
        eac = root.Q<Label>("EAC");
        ear = root.Q<Label>("EAR");
        eas = root.Q<Label>("EAS");

        eda = root.Q<Label>("EDA");
        edc = root.Q<Label>("EDC");
        edr = root.Q<Label>("EDR");
        eds = root.Q<Label>("EDS");

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
        fpl.text = battleManager.aPoints.ToString();
        rpl.text = battleManager.dPoints.ToString();
        msl.text = battleManager.region.recruitPoints.ToString();

        paa.text = battleManager.aOffensiveValues[0].ToString();
        pac.text = battleManager.aOffensiveValues[1].ToString();
        par.text = battleManager.aOffensiveValues[2].ToString();
        pas.text = battleManager.aOffensiveValues[3].ToString();

        pda.text = battleManager.aDefensiveValues[0].ToString();
        pdc.text = battleManager.aDefensiveValues[1].ToString();
        pdr.text = battleManager.aDefensiveValues[2].ToString();
        pds.text = battleManager.aDefensiveValues[3].ToString();

        eaa.text = battleManager.dOffensiveValues[0].ToString();
        eac.text = battleManager.dOffensiveValues[1].ToString();
        ear.text = battleManager.dOffensiveValues[2].ToString();
        eas.text = battleManager.dOffensiveValues[3].ToString();

        eda.text = battleManager.dDefensiveValues[0].ToString();
        edc.text = battleManager.dDefensiveValues[1].ToString();
        edr.text = battleManager.dDefensiveValues[2].ToString();
        eds.text = battleManager.dDefensiveValues[3].ToString();
    }

}
