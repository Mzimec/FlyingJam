using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal.Internal;
using UnityEngine.UIElements;

public class BattleMenuManager : MonoBehaviour {
    private UIDocument battleMenu;
    [SerializeField] UIDocument victoryMenu;
    [SerializeField] PlayerManager player;
    [SerializeField] VisualTreeAsset visualTree;

    BattleManager battleManager;

    UnityEvent<CardVE> response = new UnityEvent<CardVE>();


    VisualElement hand, attackVE, defenseVE;

    Button cancelB, battleB;

    List<CardVE> playerCards = new List<CardVE>();
    List<bool> areInHand = new List<bool>();

    private void Awake() {
        battleMenu = GetComponent<UIDocument>();
        response.AddListener(OnCardClick);
    }

    void OnEnable() {
        var root = battleMenu.rootVisualElement;
        hand = root.Q<VisualElement>("Pool");

        attackVE = root.Q<VisualElement>("AttackVE");
        defenseVE = root.Q<VisualElement>("DefenseVE");

        cancelB = root.Q<Button>("CancelB");
        battleB = root.Q<Button>("BattleB");

        if (cancelB != null) cancelB.clicked += OnCancel;
        if (battleB != null) battleB.clicked += OnFight;
                
        InitializeHand();
        InitializePools();
    }

    private void InitializePools() {
        int i = playerCards.Count;
        foreach(var card in battleManager.attackers) {
            CardVE cve = new CardVE(card, visualTree, response, i);
            if (!battleManager.region.isPlayer) {
                playerCards.Add(cve);
                areInHand.Add(false);
                i++;
            }
            attackVE.Add(cve);
        }
        foreach (var card in battleManager.defenders) {
            CardVE cve = new CardVE(card, visualTree, response, i);
            if (battleManager.region.isPlayer) {
                playerCards.Add(cve);
                areInHand.Add(false);
                i++;
            }
            defenseVE.Add(cve);
        }
    }

    private void InitializeHand() {
        int i = playerCards.Count;
        foreach (var card in player.hand) {
            CardVE cve = new CardVE(card, visualTree, response, i);
            playerCards.Add(cve);
            areInHand.Add(true);
            hand.Add(cve);
            i++;
       } 
    }

    void OnDisable() {
        if (cancelB != null) cancelB.clicked -= OnCancel;
        if (battleB != null) battleB.clicked -= OnFight;
        playerCards.Clear();
        areInHand.Clear();
        battleManager = null;
    }

    private void OnCardClick(CardVE cve) {
        int index = cve.index;
        if (areInHand[index]) {
            battleManager.AddCard(cve.card);
            hand.Remove(playerCards[index]);
            player.hand.Remove(cve.card);
            player.unitsInBattle.Add(cve.card);
            if (battleManager.region.isPlayer) defenseVE.Add(playerCards[index]);
            else attackVE.Add(playerCards[index]);
        }
        else {
            hand.Add(playerCards[index]);
            battleManager.RemoveCard(cve.card);
            player.hand.Add(cve.card);
            player.unitsInBattle.Remove(cve.card);
            if (battleManager.region.isPlayer) defenseVE.Remove(playerCards[index]);
            else attackVE.Remove(playerCards[index]);
        }
        areInHand[index] = !areInHand[index];
    }

    private void OnFight() {
        if (victoryMenu != null) {
            victoryMenu.gameObject.SetActive(true);
            var vm = victoryMenu.GetComponent<VictoryMenuManager>();
            vm.SetBattleManager(battleManager);
            vm.message = battleManager.OnFight();
        } 
        battleMenu.gameObject.SetActive(false);
    }

    private void OnCancel() { 
        if (battleMenu != null) battleMenu.gameObject.SetActive(false);
    }

    public void SetBattle(BattleManager b) {
        battleManager = b;
    }
}
