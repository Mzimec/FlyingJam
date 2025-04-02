using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ShowCardManager : MonoBehaviour
{
    [SerializeField] VisualTreeAsset cardTemplate;
    [SerializeField] VisualTreeAsset threeOfTemplate;
    [SerializeField] GameObject baseMenu;

    private UIDocument ui;


    public List<CardManager> deck = new List<CardManager>();
    public string message = "";
    public int count;

    ScrollView deckVE;
    Label headerL, countL;
    Button cancelB;

    private void Awake() {
        ui = GetComponent<UIDocument>();
    }

    private void OnEnable() {
        var root = ui.rootVisualElement;

        deckVE = root.Q<ScrollView>("DeckVE");
        headerL = root.Q<Label>("HeaderL");
        countL = root.Q<Label>("CountL");
        cancelB = root.Q<Button>("CancelB");

        cancelB.clicked += OnCancel;
    }

    private void OnDisable() {
        cancelB.clicked -= OnCancel;
        deckVE.Clear();
    }

    public void Initialize() {
        headerL.text = message;
        countL.text = $"Number of cards is {deck.Count}";

        for (int i = 0; i < deck.Count / 3; i++) { 
            VisualElement tve = threeOfTemplate.CloneTree().Q<VisualElement>("trojice");
            for (int j = 2; j >= 0; j--) {
                CardVE cve = new CardVE(deck[3 * i + j], cardTemplate, 0.55f);
                tve.Add(cve.ve);
            }
            deckVE.Add(tve);
        }

        int remaining = deck.Count % 3;
        if (remaining != 0) {
            VisualElement tve = threeOfTemplate.CloneTree().Q<VisualElement>("trojice");
            for (int j = remaining - 1; j >= 0; j--) {
                CardVE cve = new CardVE(deck[deck.Count - remaining + j], cardTemplate, 0.55f);
                tve.Add(cve.ve);
            }
            deckVE.Add(tve);
        }
    }

    private void OnCancel() {
        baseMenu.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }


}
