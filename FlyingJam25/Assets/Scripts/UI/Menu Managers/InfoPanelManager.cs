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

        regionName = root.Q<Label>("Name");
        regionPT = root.Q<Label>("Provisions");
        regionProvisions = root.Q<Label>("ProvisionsV");
        recruitPT = root.Q<Label>("Recruit");
        recruitScore = root.Q<Label>("RecruitV");

        score = root.Q<Label>("ScoreV");
        resources = root.Q<Label>("ProvisionsV");
        changes = root.Q<Label>("Changes");

        endTurnB = root.Q<Button>("EndTurnB");
        deckB = root.Q<Button>("DeckB");
        menuB = root.Q<Button>("MenuB");

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
