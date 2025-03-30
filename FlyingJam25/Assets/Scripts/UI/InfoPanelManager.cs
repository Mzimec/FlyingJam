using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class InfoPanelManager : MonoBehaviour
{
    [SerializeField] UIDocument menu;
    [SerializeField] UIDocument deckMenu;
    [SerializeField] UIDocument mainMenu;
    [SerializeField] MouseInputManager mouse;
    [SerializeField] PlayerManager player;

    RegionManager region;
    EmptyEvent onTurnEnd;

    Label regionName;
    Label regionPT;
    Label regionProvisions;
    Label recruitPT;
    Label recruitScore;
    Label score;
    Label resources;
    Label changes;

    Button endTurnB;
    Button deckB;
    Button menuB;

    private void Start() {
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

        endTurnB.clicked += OnEndTurnClicked;
        deckB.clicked += OnDeckClicked;
        menuB.clicked += OnMenuClicked;
    }

    private void Update() {
        if (mouse.hit.collider != null) {
            region = mouse.hit.collider.GetComponent<RegionManager>();
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
        deckMenu.gameObject.SetActive(true);
    }

    private void OnMenuClicked() {
        mainMenu.gameObject.SetActive(true);
    }
}
