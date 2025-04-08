using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class HungerMenuManager : MonoBehaviour {
    UIDocument ui;
    [SerializeField] VisualTreeAsset blankTemplate;
    [SerializeField] VisualTreeAsset cardTemplate;
    [SerializeField] PlayerManager player;
    [SerializeField] EmptyEvent onHungerResolved;

    UnityEvent<CardVE> response = new UnityEvent<CardVE>();
    UnityEvent<CardVE> removeCard = new UnityEvent<CardVE>();

    int cardsToDestroy;

    List<CardManager> destroyed = new List<CardManager>();

    VisualElement destroyVE, dBlankVE;
    ScrollView handVE;
    Button commitB;

    private void Awake() {
        ui = GetComponent<UIDocument>();
        response.AddListener(OnCardClick);
        removeCard.AddListener(OnCardRemove);
    }

    private void OnEnable() {
        var root = ui.rootVisualElement;
        destroyVE = root.Q<VisualElement>("SacrificeVE");
        handVE = root.Q<ScrollView>("Hand");
        commitB = root.Q<Button>("CommitB");
        dBlankVE = root.Q<VisualElement>("SBlankVE");

        StartCoroutine(ConstantValues.DisableButtonsTemporarily(new List<Button> { commitB }, ConstantValues.waitTimeOnMenu));

        if (commitB != null) commitB.clicked += OnCommit;
        cardsToDestroy = player.ToDestroyCount;
        InitializeCards();
        InitializePools();
    }

    private void OnDisable() {
        if (commitB != null) commitB.clicked -= OnCommit;
        destroyed.Clear();
        destroyVE.Clear();
    }

    private void InitializeCards() {
        List<CardManager> cards = new List<CardManager>();
        cards.AddRange(player.deck);
        cards.AddRange(player.discard);
        cards.AddRange(player.hand);
        cards.OrderBy(x => x.baseData.value);
        foreach (var card in cards) {
            CardVE cve = new CardVE(card, cardTemplate, response, 0, ConstantValues.cardScale);
            StartCoroutine(ConstantValues.DisableButtonsTemporarily(new List<Button> { cve.ve.Q<Button>("Cudl") }, ConstantValues.waitTimeOnMenu));
            handVE.Add(cve.ve);
        }
    }

    private void InitializePools() {
        for (int i = 0; i < cardsToDestroy; i++) {
            var ve1 = ConstantValues.CreateEmpty(blankTemplate);
            dBlankVE.Add(ve1);
        }
    }

    private void OnCardClick(CardVE card) {
        if (card.card == null) return;
        if (destroyed.Count >= cardsToDestroy) return;
        destroyed.Add(card.card);
        var cve = new CardVE(card.card, cardTemplate, removeCard, 0, ConstantValues.cardScale);
        destroyVE.Add(cve.ve);
        dBlankVE.RemoveAt(dBlankVE.childCount - 1);
        handVE.Remove(card.ve);
    }

    private void OnCardRemove(CardVE card) {
        if (card.card == null) return;
        if (destroyed.Contains(card.card)) {
            destroyed.Remove(card.card);
            destroyVE.Remove(card.ve);
        }
        var ve1 = ConstantValues.CreateEmpty(blankTemplate);
        var ve2 = ConstantValues.CreateEmpty(blankTemplate);
        dBlankVE.Add(ve1);
        var cve = new CardVE(card.card, cardTemplate, response, 0, ConstantValues.cardScale);
        handVE.Add(cve.ve);

    }

    private void OnCommit() {
        if (destroyed.Count != cardsToDestroy) return;
        foreach (var card in destroyed) {
            if (player.deck.Contains(card)) player.deck.Remove(card);
            if (player.hand.Contains(card)) player.hand.Remove(card);
            if (player.discard.Contains(card)) player.discard.Remove(card);
        }
        onHungerResolved.Raise(new Empty());
        gameObject.SetActive(false);
    }
}
