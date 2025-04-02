using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerManager : MonoBehaviour {
    [SerializeField] private List<CardSO> startingDeck;
    
    public List<CardManager> deck = new List<CardManager>();
    public List<CardManager> hand = new List<CardManager>();
    public List<CardManager> unitsInBattle = new List<CardManager>();
    public List<CardManager> discard = new List<CardManager>();
    public List<CardManager> cardsToDiscard = new List<CardManager>();

    private List<RegionManager> controlledRegions = new List<RegionManager>();

    public int turn = 1;
    public int score = 0;
    public int resources;
    [SerializeField] private int constantResourceDecrement;
    [SerializeField] private float factorResourceDecrement;

    public int ResourcesConsumedPerTurn => (int)(factorResourceDecrement * deck.Count) + constantResourceDecrement;

    [SerializeField] private int constantPillageRate;
    [SerializeField] private float factorPillageRate;

    [SerializeField] private float handToDeck;
    private int HandSize => (int) (handToDeck * (deck.Count + hand.Count + unitsInBattle.Count + discard.Count + cardsToDiscard.Count));

    private int ScoreMultiplier => (turn / 5) + 1;

    private void Awake() {
        foreach (var card in startingDeck) deck.Add(new CardManager(card));
    }

    private void Start() {
        DrawHand();
        List<RegionManager> regions = new List<RegionManager>(FindObjectsByType<RegionManager>(FindObjectsSortMode.None));
        foreach (var region in regions) {
            if (region.isPlayer) controlledRegions.Add(region);
        }
    }

    private void DrawHand() {
        int curHandCount = hand.Count;
        int curHandSize = HandSize;
        for(int i = 0; i < curHandSize - curHandCount; i++) {
            if (deck.Count == 0) ShuffleUpDiscard();
            if (deck.Count != 0) {
                var card = deck.Last();
                if (card == null) Debug.Log("WTF");
                deck.RemoveAt(deck.Count - 1);
                hand.Add(card);
            }
        }
    }

    private void ShuffleUpDiscard() {
        List<CardManager> shuffledDiscard = Shuffle(new List<CardManager>(discard)); // Copy first!
        deck.AddRange(shuffledDiscard); // Append shuffled cards to deck
        discard.Clear();
        Debug.Log($"Deck size after shuffling up discard: {deck.Count}");
    }

    private List<CardManager> Shuffle(List<CardManager> d) {
        for (int i = d.Count - 1; i > 0; i--) {
            int j = Random.Range(0, i + 1);
            (d[i], d[j]) = (d[j], d[i]);
        }
        return d;
    }

    private int PillageRegion(RegionManager region) {
        return region.PillageResources(factorPillageRate, constantPillageRate);
    }

    private int Pillage() {
        int pillaged = 0;
        foreach (var region in controlledRegions) pillaged += PillageRegion(region);
        Debug.Log($"Pillaged {pillaged} provisions");
        return pillaged;
    }

    private void RemoveUnit(CardManager card) {
        if (deck.Contains(card)) deck.Remove(card);
    }

    private void DestroyUnits(int n) { }

    private void UseResources() {
        resources += Pillage();
        resources -= ResourcesConsumedPerTurn;
        if(resources < 0) {
            DestroyUnits(-resources);
            resources = 0;
        }
    }

    public void OnEndTurn() {
        UseResources();
        CountScore();
        turn++;
        SortCardsAtEndTurn();
        DrawHand();
    }

    public void Load(PlayerData data, List<RegionManager> allRegions) {
        deck.Clear();
        hand.Clear();
        discard.Clear();
        unitsInBattle.Clear();
        controlledRegions.Clear();

        deck = data.deck.Select(cardData => new CardManager(cardData)).ToList();
        hand = data.hand.Select(cardData => new CardManager(cardData)).ToList();
        discard = data.discard.Select(cardData => new CardManager(cardData)).ToList();

        resources = data.resources;
        turn = data.turn;
        score = data.score;

        foreach (var region in allRegions) {
            if (region != null && region.isPlayer) controlledRegions.Add(region);
        }
    }

    private void StartTurn() {
        DrawHand();
    }

    private void SortCardsAtEndTurn() {
        foreach(var card in cardsToDiscard) discard.Add(card);
        cardsToDiscard.Clear();
        foreach(var card in unitsInBattle) hand.Add(card);
        unitsInBattle.Clear();
        Debug.Log($"Number of Player cards: {hand.Count + deck.Count + discard.Count}");
    }

    private void CountScore() {
        foreach (var region in controlledRegions) score += ScoreMultiplier;
    }

    public void OnRegionOwnerChanged(RegionManager region) {
        if(controlledRegions.Contains(region)) controlledRegions.Remove(region);
        else controlledRegions.Add(region);
    }
}
