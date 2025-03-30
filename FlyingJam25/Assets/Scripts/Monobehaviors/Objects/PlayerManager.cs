using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerManager : MonoBehaviour {
    [SerializeField] private List<CardSO> startingDeck;
    
    public List<CardManager> deck = new List<CardManager>();
    public List<CardManager> hand = new List<CardManager>();
    public List<CardManager> unitsInBattle = new List<CardManager>();
    public List<CardManager> discard = new List<CardManager>();

    private List<RegionManager> controlledRegions;

    public int turn = 1;
    public int score = 0;
    public int resources;
    [SerializeField] private int constantResourceDecrement;
    [SerializeField] private float factorResourceDecrement;

    public int ResourcesConsumedPerTurn => (int)(factorResourceDecrement * deck.Count) + constantResourceDecrement;

    [SerializeField] private int constantPillageRate;
    [SerializeField] private float factorPillageRate;

    [SerializeField] private float handToDeck;
    private int HandSize => (int) handToDeck * deck.Count;

    private void Awake() {
        foreach (var card in startingDeck) deck.Add(new CardManager(card));

    }

    private void DrawHand() {
        for(int i = 0; i < HandSize - hand.Count; i++) {
            if (deck.Count == 0) ShuffleUpDiscard();
            var card = deck.Last();
            deck.RemoveAt(deck.Count - 1);
            hand.Add(card);
        }
    }

    private void ShuffleUpDiscard() {
        deck = Shuffle(discard);
        discard.Clear();
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
}
