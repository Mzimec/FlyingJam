using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class FrostbiteMenuManager : MonoBehaviour {
    UIDocument ui;
    [SerializeField] VisualTreeAsset blankTemplate;
    [SerializeField] VisualTreeAsset cardTemplate;
    [SerializeField] PlayerManager player;
    [SerializeField] EmptyEvent onFrostbiteResolved;

    UnityEvent<CardVE> responseL = new UnityEvent<CardVE>();
    UnityEvent<CardVE> responseR = new UnityEvent<CardVE>();
    UnityEvent<CardVE> removeCard = new UnityEvent<CardVE>();

    int cardsToFreeze;

    List<CardManager> effected = new List<CardManager>();
    List<CardManager> attacked = new List<CardManager>();

    VisualElement effectVE, attackVE, eBlankVE, aBlankVE, eVE, aVE;
    ScrollView handVE;
    Button commitB;

    private void Awake() {
        ui = GetComponent<UIDocument>();
        responseL.AddListener(OnCardLeftClick);
        responseR.AddListener(OnCardRightClick);
        removeCard.AddListener(OnCardRemove);
    }

    private void OnEnable() {
        var root = ui.rootVisualElement;
        effectVE = root.Q<VisualElement>("EffectVE");
        attackVE = root.Q<VisualElement>("AttackVE");
        handVE = root.Q<ScrollView>("Hand");
        commitB = root.Q<Button>("CommitB");
        eBlankVE = root.Q<VisualElement>("EBlankVE");
        aBlankVE = root.Q<VisualElement>("ABlankVE");
        eVE = root.Q<VisualElement>("E");
        aVE = root.Q<VisualElement>("A");
        if (commitB != null) commitB.clicked += OnCommit;
        cardsToFreeze = player.InjuryValue;
        InitializeCards();
        InitializePools();
    }

    private void OnDisable() {
        if (commitB != null) commitB.clicked -= OnCommit;
        effected.Clear();
        attacked.Clear();
        effectVE.Clear();
        attackVE.Clear();
    }

    private void InitializeCards() {
        foreach (var card in player.cardsToDiscard) {
            CardVE cve = new CardVE(card, cardTemplate, responseL, responseR, ConstantValues.cardScale);
            handVE.Add(cve.ve);
        }
    }

    private void InitializePools() {
        for (int i = 0; i < cardsToFreeze; i++) {
            var ve1 = ConstantValues.CreateEmpty(blankTemplate);
            var ve2 = ConstantValues.CreateEmpty(blankTemplate);
            eBlankVE.Add(ve1);
            aBlankVE.Add(ve2);
        }
    }

    private void OnCardLeftClick(CardVE card) {
        if (card.card == null) return;
        if (!card.card.hasEffects) return;
        if (effected.Contains(card.card)) return;
        if (attacked.Contains(card.card)) return;
        if (effected.Count + attacked.Count >= cardsToFreeze) return;
        effected.Add(card.card);
        var cve = new CardVE(card.card, cardTemplate, removeCard, 0, ConstantValues.cardScale);
        effectVE.Add(cve.ve);
        eBlankVE.RemoveAt(eBlankVE.childCount - 1);
        aBlankVE.RemoveAt(aBlankVE.childCount - 1);
        handVE.Remove(card.ve);
        if(attackVE.childCount + aBlankVE.childCount == 0) {
            aVE.style.display = DisplayStyle.None;
        }
    }

    private void OnCardRightClick(CardVE card) {
        if (card.card == null) return;
        if (!card.card.hasAttack) return;
        if (effected.Contains(card.card)) return;
        if (attacked.Contains(card.card)) return;
        if (effected.Count + attacked.Count >= cardsToFreeze) return;
        attacked.Add(card.card);
        var cve = new CardVE(card.card, cardTemplate, removeCard, 0, ConstantValues.cardScale);
        attackVE.Add(cve.ve);
        eBlankVE.RemoveAt(eBlankVE.childCount - 1);
        aBlankVE.RemoveAt(aBlankVE.childCount - 1);
        handVE.Remove(card.ve);
        if (effectVE.childCount + eBlankVE.childCount == 0) {
            eVE.style.display = DisplayStyle.None;
        }
    }

    private void OnCardRemove(CardVE card) {
        if (card.card == null) return;
        if (effected.Contains(card.card)) {
            effected.Remove(card.card);
            effectVE.Remove(card.ve);
        }
        if (attacked.Contains(card.card)) {
            attacked.Remove(card.card);
            attackVE.Remove(card.ve);
        }
        var ve1 = ConstantValues.CreateEmpty(blankTemplate);
        var ve2 = ConstantValues.CreateEmpty(blankTemplate);
        eBlankVE.Add(ve1);
        aBlankVE.Add(ve2);
        var cve = new CardVE(card.card, cardTemplate, responseL, responseR, ConstantValues.cardScale);
        handVE.Add(cve.ve);
        eVE.style.display = DisplayStyle.Flex;
        aVE.style.display = DisplayStyle.Flex;

    }

    private void OnCommit() {
        if (effected.Count + attacked.Count != cardsToFreeze) return;
        foreach (var card in effected) {
            card.hasEffects = false;
            card.effects.Clear();
            if (!card.hasAttack) player.cardsToDiscard.Remove(card);
        }
        foreach (var card in attacked) {
            card.hasAttack = false;
            card.attackValues = new int[card.baseData.attackValues.Length];
            if (!card.hasEffects) player.cardsToDiscard.Remove(card);
        }
        onFrostbiteResolved.Raise(new Empty());
        gameObject.SetActive(false);
    }
}
