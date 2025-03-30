using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal.Internal;
using UnityEngine.UIElements;

public class BattleMenuManager : MonoBehaviour {
    [SerializeField] private UIDocument battleMenu;
    [SerializeField] UIDocument victoryMenu;
    [SerializeField] PlayerManager player;
    [SerializeField] VisualTreeAsset visualTree;

    BattleManager battleManager;

    UnityEvent<CardVE> response = new UnityEvent<CardVE>();


    VisualElement hand;
    VisualElement attackVE;
    VisualElement defenseVE;

    List<CardVE> playerCards;
    List<bool> areInHand = new List<bool>();

    Label victory;
    Label points;
    Label recruit;



    private void Awake() {
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var root = battleMenu.rootVisualElement;
        var rootV = victoryMenu.rootVisualElement;
        hand = root.Q<VisualElement>("Pool");

        attackVE = root.Q<VisualElement>("AttackVE");
        defenseVE = root.Q<VisualElement>("DefenseVE");

        Button cancelB = root.Q<Button>("CancelB");
        Button battleB = root.Q<Button>("BattleB");

        cancelB.clicked += OnCancel;
        battleB.clicked += OnFight;

        victory = rootV.Q<Label>("V-D-text");
        points = rootV.Q<Label>("Points");
        recruit = rootV.Q<Label>("Recruit");

        Button cancelBV = rootV.Q<Button>("CancleB");
        cancelBV.clicked += OnCancelV;


        response.AddListener(OnCardClick);

        battleMenu.gameObject.SetActive(false);
        victoryMenu.gameObject.SetActive(false);
    }

    private void OnEnable() {
       for(int i = 0; i < player.hand.Count; i++ ) {
            CardVE cve = new CardVE(player.hand[i], visualTree, response, i);
            playerCards.Add(cve);
            areInHand.Add(true);
       } 
    }

    private void OnDisable() {
        for (int i = playerCards.Count - 1; i >= 0; i--) {
            if (areInHand[i]) {
                areInHand.RemoveAt(i);
                playerCards.RemoveAt(i);
            }
        }

        for(int i = 0; i < playerCards.Count; i++) {
            playerCards[i].index = i;
        }
    }

    private void OnCardClick(CardVE cve) {
        int index = cve.index;
        if (areInHand[index]) {
            hand.Remove(playerCards[index]);
            if (battleManager.region.isPlayer) defenseVE.Add(playerCards[index]);
            else attackVE.Add(playerCards[index]);
        }
        else {
            if (battleManager.region.isPlayer) defenseVE.Remove(playerCards[index]);
            else attackVE.Remove(playerCards[index]);
            hand.Add(playerCards[index]);
        }
        areInHand[index] = !areInHand[index];
    }

    private void OnFight() {
        string status = battleManager.OnFight();
        victoryMenu.gameObject.SetActive(true);
        WriteOutcomes(status);
        battleMenu.gameObject.SetActive(false);
    }

    private void OnCancel() { 
        battleMenu.gameObject.SetActive(false);
    }

    private void OnCancelV() {
        victoryMenu.gameObject?.SetActive(false);
    }

    private void WriteOutcomes(string s) {
        victory.text = s;
        points.text = $"Attackers: {battleManager.aPoints} points.\n Defenders {battleManager.dPoints} points.";
        recruit.text = $"New Recruit Value is: {battleManager.region.recruitPoints}.";
    }
}
