using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour {
    public RegionManager region;

    public CardManager[] attackers;
    public CardManager[] defenders;

    private int[] aOffensiveValues = new int[ConstantValues.cardTypesCount];
    private int[] dOffensiveValues = new int[ConstantValues.cardTypesCount];
    private int[] aDefensiveValues = new int[ConstantValues.cardTypesCount];
    private int[] dDefensiveValues = new int[ConstantValues.cardTypesCount];

    public int aPoints, dPoints;

    private void Awake() {
        region = GetComponentInParent<RegionManager>();
        if (region == null) {
            Debug.LogError("BattleManager is not inside a RegionManager!");
            return;
        }

        int size = region.baseData.battlefieldSize;
        attackers = new CardManager[size];
        defenders = new CardManager[size];
    }

    private void GenerateOpponentCards() {
        var cardsToEnter = region.GenerateLoadOut();
        if (region.isPlayer) {
            for (int i = 0; i < attackers.Length; i++) attackers[i] = new CardManager(cardsToEnter[i]);
        }
        else {
            for (int i = 0; i < defenders.Length; i++) defenders[i] = new CardManager(cardsToEnter[i]);
        }
    }

    private void ChangeAttackPool(int[] pool, CardManager[] cards) {
        foreach(var card in cards) {
            if (card.hasAttack) {
                for (int i = 0; i < pool.Length; i++) {
                    if (card.attackValues.Length > i) pool[i] += card.attackValues[i];
                }
            }
        }
    }

    private void ChangeVulnerabilityPool(int[] pool, CardManager[] cards) {
        foreach (var card in cards) {
            for (int i = 0; i < pool.Length; i++) pool[i] += card.vulnerabilityValues[i];
        }
    }

    private void CountPools() {
        ChangeAttackPool(aOffensiveValues, attackers);
        ChangeAttackPool(dOffensiveValues, defenders);
        ChangeVulnerabilityPool(aDefensiveValues, attackers);
        ChangeVulnerabilityPool(dDefensiveValues, defenders);
    }

    private string BattleOutcomes() {
        string res;
        if (aPoints > dPoints) {
            if (region.isPlayer) {
                Debug.Log("Defeat");
                region.recruitPoints -= (int)(((float)dPoints / aPoints) * region.recruitPoints);
                res = "Defeat";
            }
            else Debug.Log("Victory");
            region.isPlayer = !region.isPlayer;
            res = "Victory";
        }
        else {
            if (region.isPlayer) {
                Debug.Log("Victory");
                res = "Victory";
            }
            else {
                Debug.Log("Defeat");
                region.recruitPoints -= (int)(((float)aPoints / dPoints) * region.recruitPoints);
                res = "Defeat";
            }
        }
        return res;
    }

    private string RunBattle() {
        foreach(var card in attackers) {
            if (card.hasEffects) card.ApplyEffects(attackers, defenders);
        }
        foreach (var card in defenders) {
            if (card.hasEffects) card.ApplyEffects(defenders, attackers);
        }

        CountPools();
        CountVictoryPoints();
        return BattleOutcomes();
              
    }

    private void CountVictoryPoints() {
        for (int i = 0; i < ConstantValues.cardTypesCount; i++) {
            dDefensiveValues[i] -= aOffensiveValues[i];
            if (dDefensiveValues[i] < 0) dDefensiveValues[i] = 0;
            aDefensiveValues[i] -= dOffensiveValues[i];
            if (aDefensiveValues[i] < 0) aDefensiveValues[i] = 0;
        }

        for (int i = 0; i <= ConstantValues.cardTypesCount; i++) {
            dPoints += dOffensiveValues[i] + dDefensiveValues[i];
            aPoints += aOffensiveValues[i] + aDefensiveValues[i];
        }
    }
    
    public string OnFight() {
        return RunBattle();
    }

    public void AddCard(CardManager card) {
        if (region.isPlayer) {
            for (int i = 0; i < defenders.Length; i++) {
                if (defenders[i] == null) {
                    defenders[i] = card;
                    break;
                }
            }
        }
        else {
            for (int i = 0; i < attackers.Length; i++) {
                if (attackers[i] == null) {
                    attackers[i] = card;
                    break;
                }
            }
        }
    }

    public void RemoveCard(CardManager card) {
        if (region.isPlayer) {
            for (int i = 0; i < defenders.Length; i++) {
                if (defenders[i] == card) {
                    defenders[i] = null;
                    break;
                }
            }
        }
        else {
            for (int i = 0; i < attackers.Length; i++) {
                if (attackers[i] == card) {
                    attackers[i] = null;
                    break;
                }
            }
        }
    }
}
