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

        attackVE = root.Q<VisualElement>("attackVE");
        defenseVE = root.Q<VisualElement>("defenseVE");

        cancelB = root.Q<Button>("CancelB");
        battleB = root.Q<Button>("BattleB");

        if (cancelB != null) cancelB.clicked += OnCancel;
        if (battleB != null) battleB.clicked += OnFight;
    }

    private void InitializePools() {
        if (battleManager == null) {
            Debug.LogError("BattleManager is null! Did you forget to call SetBattle()?");
            return;
        }

        int i = playerCards.Count;
        for(int j = 0; j < battleManager.attackers.Count; j++) {
            if (battleManager.attackers[j] == null) continue;
            CardVE cve; 
            if (!battleManager.region.isPlayer) {
                cve = new CardVE(battleManager.attackers[j], visualTree, response, i, 0.6f);
                playerCards.Add(cve);
                areInHand.Add(false);
                i++;
            }
            else cve = new CardVE(battleManager.attackers[j], visualTree, 0.6f);
            attackVE.Add(cve.ve);
        }
        for (int j = 0; j < battleManager.defenders.Count; j++) {
            if (battleManager.defenders[j] == null) continue;
            CardVE cve;
            if (battleManager.region.isPlayer) {
                cve = new CardVE(battleManager.defenders[j], visualTree, response, i, 0.6f);
                playerCards.Add(cve);
                areInHand.Add(false);
                i++;
            }
            else cve = new CardVE(battleManager.defenders[j], visualTree, 0.6f);
            defenseVE.Add(cve.ve);
        }
    }

    private void InitializeHand() {
        int i = playerCards.Count;
        foreach (var card in player.hand) {
            CardVE cve = new CardVE(card, visualTree, response, i, 0.6f);
            playerCards.Add(cve);
            areInHand.Add(true);
            hand.Add(cve.ve);
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
            if ((battleManager.region.isPlayer && battleManager.defenders.Count >= battleManager.region.baseData.battlefieldSize) ||
                (!battleManager.region.isPlayer && battleManager.attackers.Count >= battleManager.region.baseData.battlefieldSize)) {
                return;
            }
            //Debug.Log($"{battleManager.defenders.Count}, {battleManager.attackers.Count}, {battleManager.region.baseData.battlefieldSize}");
            battleManager.AddCard(cve.card);
            hand.Remove(playerCards[index].ve);
            player.hand.Remove(cve.card);
            player.unitsInBattle.Add(cve.card);
            if (battleManager.region.isPlayer) defenseVE.Add(playerCards[index].ve);
            else attackVE.Add(playerCards[index].ve); 
        }
        else {
            battleManager.RemoveCard(cve.card);
            player.unitsInBattle.Remove(cve.card);
            player.hand.Add(cve.card);
            if (battleManager.region.isPlayer) defenseVE.Remove(playerCards[index].ve);
            else attackVE.Remove(playerCards[index].ve);
            hand.Add(playerCards[index].ve);
        }
        areInHand[index] = !areInHand[index];
    }

    private void OnFight() {
        if (victoryMenu != null) {
            victoryMenu.gameObject.SetActive(true);
            var vm = victoryMenu.GetComponent<VictoryMenuManager>();
            vm.message = battleManager.OnFight();
            vm.SetBattleManager(battleManager);
            ChangePlayerCards();
        } 
        battleMenu.gameObject.SetActive(false);
    }

    private void OnCancel() { 
        if (battleMenu != null) battleMenu.gameObject.SetActive(false);
    }

    public void SetBattle(BattleManager b) {
        battleManager = b;
        InitializeHand();
        InitializePools();
    }

    private void ChangePlayerCards() {
        if (battleManager.region.isPlayer) {
            foreach (var card in battleManager.defenders) {
                if (player.unitsInBattle.Contains(card)) {
                    player.unitsInBattle.Remove(card);
                    player.cardsToDiscard.Add(card);
                }
            }
        }
        else {
            foreach (var card in battleManager.attackers) {
                if (player.unitsInBattle.Contains(card)) {
                    player.unitsInBattle.Remove(card);
                    player.cardsToDiscard.Add(card);
                }
            }
        }
    }
}
