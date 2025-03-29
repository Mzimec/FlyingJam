using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {
    [SerializeField] private List<CardManager> deck;
    private List<CardManager> hand;
    private List<CardManager> unitsInBattle;

    private List<RegionManager> controlledRegions;

    [SerializeField] private int resources;
    [SerializeField] private int constantResourceDecrement;
    [SerializeField] private float factorResourceDecrement;

    public int ResourcesConsumedPerTurn => (int)(factorResourceDecrement * deck.Count) + constantResourceDecrement;

    [SerializeField] private int constantPillageRate;
    [SerializeField] private float factorPillageRate;


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
}
