using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour {
    private RegionManager region;

    private CardManager[] attackers;
    private CardManager[] defenders;

    private int[] aOffensiveValues = new int[ConstantValues.cardTypesCount];
    private int[] dOffensiveValues = new int[ConstantValues.cardTypesCount];
    private int[] aDefensiveValues = new int[ConstantValues.cardTypesCount];
    private int[] dDefensiveValues = new int[ConstantValues.cardTypesCount];

    private void Awake() {
        region = GetComponentInParent<RegionManager>();
        if (region != null) {
            Debug.LogError("BattleManager is not inside a RegionManager!");
            return;
        }

        int size = region.baseData.battlefieldSize;
        attackers = new CardManager[size];
        defenders = new CardManager[size];
    }

    private void GenerateOpponentCards() {
        if (region.IsPlayer) {
           var cardsToEnter = region.GenerateLoadOut();
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
        ChangeAttackPool(dDefensiveValues, defenders);
        ChangeVulnerabilityPool(aDefensiveValues, attackers);
        ChangeVulnerabilityPool(dDefensiveValues, defenders);
    }


    private void DetermineVictor() { }
    public void OnFight() {

    }
}
