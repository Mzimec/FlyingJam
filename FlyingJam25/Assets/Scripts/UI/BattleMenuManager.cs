using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal.Internal;
using UnityEngine.UIElements;

public class BattleMenuManager : MonoBehaviour
{
    [SerializeField] private UIDocument battleMenu;
    [SerializeField] UIDocument victoryMenu;
    [SerializeField] PlayerManager player;
    [SerializeField] VisualTreeAsset visualTree;

    BattleManager battleManager;


    VisualElement hand;

    private void Awake() {
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var root = battleMenu.rootVisualElement;
        var rootV = victoryMenu.rootVisualElement;
        hand = root.Q<VisualElement>("Pool");

        Button cancelB = root.Q<Button>("CancelB");
        Button battleB = root.Q<Button>("BattleB");

        cancelB.clicked += OnCancel;
        battleB.clicked += OnFight;

        battleMenu.gameObject.SetActive(false);
        victoryMenu.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnFight() {
        battleManager.OnFight();    
    }

    private void OnCancel() { 
        battleMenu.gameObject.SetActive(false);
    }
    private void PopulateHand() {
        if (hand == null) {
            Debug.LogError("Hand ScrollView is not assigned!");
            return;
        }

        // Clear previous children
        hand.Clear();

        // Loop through each card in player's hand and add it to the ScrollView
        foreach (var card in player.hand) {
            UnityEvent response = new UnityEvent();
            response.AddListener(() => OnHandToField(card));
            //CardVe cardVE = new CardVE(card, visualTree, response);  // Create the VisualElement for the card
            //hand.Add(cardVE);  // Add the VisualElement to the ScrollView
        }
    }

    private void OnHandToField(CardManager c) {
        player.hand.Remove(c);
    }
}
