using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class DeckMenuManager : MonoBehaviour
{
    [SerializeField] PlayerManager player;
    [SerializeField] VisualTreeAsset cardTemplate;
    [SerializeField] ShowCardManager showCardsUI;
    private UIDocument ui;

    Button cancelB, allCardsB, deckB, discardB, handB, battleB;

    private void Awake() {
        ui = GetComponent<UIDocument>(); 
    }

    private void OnEnable() {
        var root = ui.rootVisualElement;

        cancelB = root.Q<Button>("CancelB");
        allCardsB = root.Q<Button>("AllCardsB");
        deckB = root.Q<Button>("DeckB");
        discardB = root.Q<Button>("DiscardB");
        handB = root.Q<Button>("HandB");
        battleB = root.Q<Button>("BattlefieldB");

        var infoL = root.Q<Label>("InfoL");
        infoL.text =
            $"Number of all cards: {GetAllCards().Count}\n" +
            $"Cards in deck: {player.deck.Count}" +
            $"\nCards in discard pile: {GetDiscardPile().Count}" +
            $"\nCards in hand: {player.hand.Count}" +
            $"\nCards in battlefield: {player.unitsInBattle.Count}";

        cancelB.clicked += OnCancel;
        allCardsB.clicked += OnCards;
        deckB.clicked += OnDeck;
        discardB.clicked += OnDiscard;
        handB.clicked += OnHand;
        battleB.clicked += OnBattle;
    }

    private void OnDisable() {
        cancelB.clicked -= OnCancel;
        allCardsB.clicked -= OnCards;
        deckB.clicked -= OnDeck;
        discardB.clicked -= OnDiscard;
        handB.clicked -= OnHand;
        battleB.clicked -= OnBattle;
    }

    private void OnCancel() { 
        gameObject.SetActive(false);
    }

    private List<CardManager> GetAllCards() {
        var res = new List<CardManager>();
        res.AddRange(player.deck);
        res.AddRange(player.discard);
        res.AddRange(player.cardsToDiscard);
        res.AddRange(player.hand);
        res.AddRange(player.unitsInBattle);
        return res;
    }

    private List<CardManager> GetDiscardPile() {
        var res = new List<CardManager>();
        res.AddRange(player.discard);
        res.AddRange(player.cardsToDiscard);
        return res;
    }

    private void OnCards() {
        if (showCardsUI != null) {
            showCardsUI.gameObject.SetActive(true);
            showCardsUI.message = "All Player Cards";
            showCardsUI.deck = GetAllCards();
            showCardsUI.Initialize();
        }
        gameObject.SetActive(false);
    }

    private void OnDiscard() {
        if (showCardsUI != null) {
            showCardsUI.gameObject.SetActive(true);
            showCardsUI.message = "Discard Pile";
            showCardsUI.deck = GetDiscardPile();
            showCardsUI.Initialize();
        }
        gameObject.SetActive(false);
    }

    private void OnDeck() {
        if (showCardsUI != null) {
            showCardsUI.gameObject.SetActive(true);
            showCardsUI.message = "Remaining Deck";
            showCardsUI.deck = new List<CardManager>(player.deck);
            showCardsUI.Initialize();
        }
        gameObject.SetActive(false);
    }

    private void OnHand() {
        if (showCardsUI != null) {
            showCardsUI.gameObject.SetActive(true);
            showCardsUI.message = "Player's Hand";
            showCardsUI.deck = new List<CardManager>(player.hand);
            showCardsUI.Initialize();
        }
        gameObject.SetActive(false);
    }

    private void OnBattle() {
        if (showCardsUI != null) {
            showCardsUI.gameObject.SetActive(true);
            showCardsUI.message = "Cards in Battle";
            showCardsUI.deck = new List<CardManager>(player.unitsInBattle);
            showCardsUI.Initialize();
            gameObject.SetActive(false);
        }
    }


}
