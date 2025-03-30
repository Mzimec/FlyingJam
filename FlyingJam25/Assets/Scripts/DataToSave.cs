using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class RegionData {
    public int recruitPoints;
    public int resources;
    public int regionDistance;
    public bool isAttacked;
    public bool isPlayer;

    public RegionData(RegionManager regionManager) {
        recruitPoints = regionManager.recruitPoints;
        resources = regionManager.resources;
        regionDistance = regionManager.regionDistance;
        isAttacked = regionManager.isAttacked;
        isPlayer = regionManager.isPlayer;
    }
}

[System.Serializable]
public class PlayerData {
    public List<CardData> deck;
    public List<CardData> hand;
    public List<CardData> discard;
    public int resources;
    public int turn;

    public PlayerData(PlayerManager player) {
        deck = player.deck.Select(card => new CardData(card)).ToList();
        hand = player.hand.Select(card => new CardData(card)).ToList();
        discard = player.discard.Select(card => new CardData(card)).ToList();
        resources = player.resources;
        turn = player.turn;
    }

}

[System.Serializable]
public class CardData {
    public CardSO baseData;
    public bool hasEffects = false;
    public bool hasAttack = false;
    public int[] attackValues;
    public int[] vulnerabilityValues;
    public List<Effect> effects;

    public CardData(CardManager card) {
        baseData = card.baseData;   
        hasEffects = card.hasEffects;   
        hasAttack = card.hasAttack;     
        attackValues = new int[card.attackValues.Length]; 
        vulnerabilityValues = new int[card.vulnerabilityValues.Length]; 
        card.attackValues.CopyTo(attackValues, 0);
        card.vulnerabilityValues.CopyTo(vulnerabilityValues, 0);
        effects = new List<Effect>(card.effects);
    }
}


[System.Serializable]
public class DataToSave {
    public List<RegionData> regions;
    public PlayerData player;
    public DataToSave(List<RegionManager> regionManagers, PlayerManager playerManager) {
        regions = regionManagers.Select(region => new RegionData(region)).ToList();
        player = new PlayerData(playerManager);
    }
}
