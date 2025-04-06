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
    [SerializeField] GameObject victoryMenu;
    [SerializeField] PlayerManager player;
    [SerializeField] VisualTreeAsset visualTree;
    [SerializeField] VisualTreeAsset blankCard;

    BattleManager battleManager;

    UnityEvent<CardVE> response = new UnityEvent<CardVE>();


    VisualElement hand, attackVE, defenseVE, aBlankVE, dBlankVE;

    Label al, dl;

    Button cancelB, battleB;

    List<CardVE> playerCards = new List<CardVE>();
    List<bool> areInHand = new List<bool>();

    private void Awake() {
        battleMenu = GetComponent<UIDocument>();
        response.AddListener(OnCardClick);
    }

    void OnEnable() {
        var root = battleMenu.rootVisualElement;
        hand = root.Q<VisualElement>("Hand");

        attackVE = root.Q<VisualElement>("attackVE");
        defenseVE = root.Q<VisualElement>("defenseVE");

        aBlankVE = root.Q<VisualElement>("ABlankVE");
        dBlankVE = root.Q<VisualElement>("DBlankVE"); 

        cancelB = root.Q<Button>("CancelB");
        battleB = root.Q<Button>("BattleB");

        al = root.Q<Label>("AL");
        dl = root.Q<Label>("DL");

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
            CardVE cve = new CardVE(battleManager.attackers[j], visualTree, response, i, ConstantValues.cardScale);
            playerCards.Add(cve);
            areInHand.Add(false);
            i++;
            /*if (!battleManager.region.isPlayer) {
                cve = new CardVE(battleManager.attackers[j], visualTree, response, i, ConstantValues.cardScale);
                playerCards.Add(cve);
                areInHand.Add(false);
                i++;
            }
            else cve = new CardVE(battleManager.attackers[j], visualTree, ConstantValues.cardScale);*/
            attackVE.Add(cve.ve);
        }

        int emptyAttackersCount = battleManager.region.baseData.battlefieldSize - battleManager.attackers.Count;
        for (int j = 0; j < emptyAttackersCount; j++) {
            VisualElement blank = ConstantValues.CreateEmpty(blankCard);
            aBlankVE.Add(blank);
        }

        for (int j = 0; j < battleManager.defenders.Count; j++) {
            if (battleManager.defenders[j] == null) continue;
            CardVE cve = new CardVE(battleManager.defenders[j], visualTree, ConstantValues.cardScale);
            /*if (battleManager.region.isPlayer) {
                cve = new CardVE(battleManager.defenders[j], visualTree, response, i, ConstantValues.cardScale);
                playerCards.Add(cve);
                areInHand.Add(false);
                i++;
            }
            else cve = new CardVE(battleManager.defenders[j], visualTree, ConstantValues.cardScale);*/
            defenseVE.Add(cve.ve);
        }

        int emptyDefendersCount = battleManager.region.baseData.battlefieldSize - battleManager.defenders.Count;
        for (int j = 0; j < emptyDefendersCount; j++) {
            VisualElement blank = ConstantValues.CreateEmpty(blankCard);
            dBlankVE.Add(blank);
        }
    }

    private void InitializeHand() {
        int i = playerCards.Count;
        foreach (var card in player.hand) {
            CardVE cve = new CardVE(card, visualTree, response, i, ConstantValues.cardScale);
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
            if ((!battleManager.region.isPlayer && battleManager.attackers.Count >= battleManager.region.baseData.battlefieldSize)) {
                return;
            }
            //Debug.Log($"{battleManager.defenders.Count}, {battleManager.attackers.Count}, {battleManager.region.baseData.battlefieldSize}");
            battleManager.AddCard(cve.card);
            hand.Remove(playerCards[index].ve);
            player.hand.Remove(cve.card);
            player.unitsInBattle.Add(cve.card);
            attackVE.Add(playerCards[index].ve);
            aBlankVE.RemoveAt(aBlankVE.childCount - 1);
            /*if (battleManager.region.isPlayer) {
                defenseVE.Add(playerCards[index].ve);
                dBlankVE.RemoveAt(dBlankVE.childCount - 1);
            }
            else {
                attackVE.Add(playerCards[index].ve);
                aBlankVE.RemoveAt(aBlankVE.childCount - 1);
            }*/
        }
        else {
            battleManager.RemoveCard(cve.card);
            player.unitsInBattle.Remove(cve.card);
            player.hand.Add(cve.card);
            attackVE.Remove(playerCards[index].ve);
            var blank = ConstantValues.CreateEmpty(blankCard);
            aBlankVE.Add(blank);
            /*if (battleManager.region.isPlayer) {
                defenseVE.Remove(playerCards[index].ve);
                var blank = ConstantValues.CreateEmpty(blankCard);
                dBlankVE.Add(blank);

            }
            else {
                attackVE.Remove(playerCards[index].ve);
                var blank = ConstantValues.CreateEmpty(blankCard);
                aBlankVE.Add(blank);
            }*/
            hand.Add(playerCards[index].ve);
        }
        areInHand[index] = !areInHand[index];
    }

    private void OnFight() {
        if (battleManager.attackers.Count == 0 || battleManager.defenders.Count == 0) {
            Debug.Log("No cards in battle!");
            return;
        }
        if (victoryMenu != null) {
            victoryMenu.SetActive(true);
            var vm = victoryMenu.GetComponent<VictoryMenuManager>();
            ChangePlayerCards(); 
            vm.message = battleManager.OnFight(player);
            vm.SetBattleManager(battleManager);
        }
        gameObject.SetActive(false);
    }

    private void OnCancel() { 
        if (battleMenu != null) battleMenu.gameObject.SetActive(false);
    }

    public void SetBattle(BattleManager b) {
        battleManager = b;
        InitializeHand();
        InitializePools();
        if (battleManager.region.isPlayer) {
            al.text = "Defenders";
            dl.text = "Attackers";
        }
        else {
            al.text = "Attackers";
            dl.text = "Defenders";
        }
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
