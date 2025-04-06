using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class InfoPanelManager : MonoBehaviour
{
    UIDocument menu;
    [SerializeField] GameObject deckMenu;
    [SerializeField] GameObject mainMenu;
    [SerializeField] MouseInputManager mouse;
    [SerializeField] PlayerManager player;
    [SerializeField] EmptyEvent onTurnEnd;

    Label regionName, regionPT, regionProvisions, recruitPT, recruitScore, score, resources, changes;

    Button endTurnB, deckB, menuB;

    private void Awake() {
        menu = GetComponent<UIDocument>();
    }

    private void OnEnable() {
        var root = menu.rootVisualElement;

        var regionVE = root.Q<VisualElement>("RegionContainer");
        regionName = regionVE.Q<Label>("Name");
        regionPT = regionVE.Q<Label>("Provisions");
        regionProvisions = regionVE.Q<Label>("ProvisionsV");
        recruitPT = regionVE.Q<Label>("Recruit");
        recruitScore = regionVE.Q<Label>("RecruitV");

        var infoVE = root.Q<VisualElement>("InfoContainer");
        score = infoVE.Q<Label>("ScoreV");
        resources = infoVE.Q<Label>("ProvisionsV");
        changes = infoVE.Q<Label>("Changes");

        endTurnB = infoVE.Q<Button>("EndTurn");
        deckB = infoVE.Q<Button>("Deck");
        menuB = infoVE.Q<Button>("Menu");

        if(endTurnB != null) endTurnB.clicked += OnEndTurnClicked;
        if (deckB != null) deckB.clicked += OnDeckClicked;
        if (menuB != null) menuB.clicked += OnMenuClicked;
    }

    private void OnDisable() {
        if (endTurnB != null) endTurnB.clicked -= OnEndTurnClicked;
        if (deckB != null) deckB.clicked -= OnDeckClicked;
        if (menuB != null) menuB.clicked -= OnMenuClicked;
    }

    private void Update() {
        RegionManager region = null;
        if (mouse.hit.collider != null) {
            region = mouse.hit.collider.GetComponent<RegionManager>();
        }
        BattleManager bm;
        if (region == null && mouse.hit.collider != null) {
            bm = mouse.hit.collider.GetComponent<BattleManager>();
            if (bm != null) region = bm.region;
        }

        if (region != null) {
            regionName.text = region.regionName;
            regionPT.text = "Provisions:";
            regionProvisions.text = region.resources.ToString();
            recruitPT.text = "Recruit Points";
            recruitScore.text = region.recruitPoints.ToString();
        }

        else {
            regionName.text = "";
            regionPT.text = "";
            regionProvisions.text = "";
            recruitPT.text = "";
            recruitScore.text = "";
        }

        score.text = player.score.ToString();
        resources.text = player.resources.ToString();
        changes.text = "";
    }

    private void OnEndTurnClicked() {
        onTurnEnd.Raise(new Empty());
    }

    private void OnDeckClicked() {
        if(deckMenu != null) deckMenu.SetActive(true);
    }

    private void OnMenuClicked() {
        if (mainMenu != null) mainMenu.SetActive(true);
    }
}
