using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BattleManager : MonoBehaviour {
    public RegionManager region;
    private IsClickable clickable;
    private ChildActivator battleMenu;

    public List<CardManager> attackers;
    public List<CardManager> defenders;

    public int[] aOffensiveValues = new int[ConstantValues.cardTypesCount];
    public int[] dOffensiveValues = new int[ConstantValues.cardTypesCount];
    public int[] aDefensiveValues = new int[ConstantValues.cardTypesCount];
    public int[] dDefensiveValues = new int[ConstantValues.cardTypesCount];

    public int aPoints, dPoints;

    private void Awake() {
        region = GetComponentInParent<RegionManager>();
        if (region == null) {
            Debug.LogError("BattleManager is not inside a RegionManager!"); 
            return;
        }

        clickable = GetComponent<IsClickable>();
        if (clickable != null) {
            clickable.OnClicked += HandleClick; // Subscribe to the event
        }

        battleMenu = GameObject.FindWithTag("BattleMenu").GetComponent<ChildActivator>();

        int size = region.baseData.battlefieldSize;
        attackers = new List<CardManager>();
        defenders = new List<CardManager>();

        GenerateOpponentCards();
    }

    private void OnDestroy() {
        if (clickable != null) {
            clickable.OnClicked -= HandleClick; // Unsubscribe when destroyed
        }
    }

    private void GenerateOpponentCards() {
        var cardsToEnter = region.GenerateLoadOut();
        foreach (var card in cardsToEnter) {
            /*if(region.isPlayer) attackers.Add(new CardManager(card));
            else defenders.Add(new CardManager(card));*/
            defenders.Add(new CardManager(card));
        }
    }

    private void ChangeAttackPool(int[] pool, List<CardManager> cards) {
        foreach(var card in cards) {
            if (card.hasAttack) {
                for (int i = 0; i < pool.Length; i++) {
                    if (card.attackValues.Length > i) pool[i] += card.attackValues[i];
                }
            }
        }
    }

    private void ChangeVulnerabilityPool(int[] pool, List<CardManager> cards) {
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

    private string BattleOutcomes(PlayerManager p) {
        string res;
        if (aPoints > dPoints) {
            /*if (region.isPlayer) {
                region.recruitPoints -= (int)(((float)dPoints / aPoints) * region.recruitPoints);
                res = "Defeat";
                p.injuryValue += 2;
            }
            else {
                res = "Victory";
                p.injuryValue++;
            }
            region.isPlayer = !region.isPlayer;
            region.OnChangeOwner.Raise(region);*/
            res = "Victory";
            p.injuryValue++;
            if (!region.isPlayer) {
                region.isPlayer = true;
                region.OnChangeOwner.Raise(region);
            }
        }
        else {
            /*if (region.isPlayer) {
                res = "Victory";
                p.injuryValue++;
            }
            else {
                region.recruitPoints -= (int)(((float)aPoints / dPoints) * region.recruitPoints);
                res = "Defeat";
                p.injuryValue += 2;
            }*/
            res = "Defeat";
            p.injuryValue += 2;
            region.recruitPoints -= (int)(((float)aPoints / (aPoints+dPoints)) * region.recruitPoints);
            if (region.isPlayer) {
                region.isPlayer = false;
                region.OnChangeOwner.Raise(region);
            }
        }
        return res;
    }

    private string RunBattle(PlayerManager p) {
        foreach(var card in attackers) {
            if (card.hasEffects) card.ApplyEffects(attackers, defenders);
        }
        foreach (var card in defenders) {
            if (card.hasEffects) card.ApplyEffects(defenders, attackers);
        }

        CountPools();
        CountVictoryPoints();
        return BattleOutcomes(p);
              
    }

    private void CountVictoryPoints() {
        for (int i = 0; i < ConstantValues.cardTypesCount; i++) {
            dDefensiveValues[i] -= aOffensiveValues[i];
            if (dDefensiveValues[i] < 0) dDefensiveValues[i] = 0;
            aDefensiveValues[i] -= dOffensiveValues[i];
            if (aDefensiveValues[i] < 0) aDefensiveValues[i] = 0;
        }

        for (int i = 0; i < ConstantValues.cardTypesCount; i++) {
            dPoints += dOffensiveValues[i] + dDefensiveValues[i];
            aPoints += aOffensiveValues[i] + aDefensiveValues[i];
        }
    }
    
    public string OnFight(PlayerManager p) {
        return RunBattle(p);
    }

    public void AddCard(CardManager card) {
        /*if (region.isPlayer) {
            if(defenders.Count < region.baseData.battlefieldSize) defenders.Add(card);
        }
        else {
            if(attackers.Count < region.baseData.battlefieldSize) attackers.Add(card);
        }*/
        if (attackers.Count < region.baseData.battlefieldSize) attackers.Add(card);
    }

    public void RemoveCard(CardManager card) {
        /*if (region.isPlayer) {
            if(defenders.Contains(card)) defenders.Remove(card);
        }
        else {
            if (attackers.Contains(card)) attackers.Remove(card);
        }*/
        if (attackers.Contains(card)) attackers.Remove(card);
    }

    public void OpenBattle() {
        //Debug.Log("OpenBattle");
        if (battleMenu != null) {
            battleMenu.OnActivate();
            var bm = battleMenu.GetComponentInChildren<BattleMenuManager>();
            if (bm != null) bm.SetBattle(this);
            else Debug.Log("No BattleMenuManager Child");
        }
        else Debug.Log("No Battle Menu!");
    }

    public void HandleClick(InputControl control) {
        if (control == Mouse.current.leftButton) {
            OpenBattle();
        }
    }
}
