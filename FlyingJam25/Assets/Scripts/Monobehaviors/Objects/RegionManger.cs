using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class RegionManager : MonoBehaviour {

    [SerializeField] public RegionSO baseData;

    [SerializeField] public int recruitPoints;
    [SerializeField] private int resources;
    [SerializeField] private int regionDistance;

    [SerializeField] private bool isAttacked = false;

    [SerializeField] public bool isPlayer;
    [SerializeField] private SideSO player;

    [SerializeField] private List<RegionManager> neighbors;
    [SerializeField] private BattleGenerator battleGenerator;
    private GameObject battle;


    public float GetResourcesToBase => (float)resources / baseData.resources;

    public bool IsRiotable => !IsBorder && GetResourcesToBase < baseData.riotBoundary;

    private float chanceMultiplier = 0.05f;
    private int GetChanceValue => (int)(1.0f / chanceMultiplier) + 1;


    private bool IsBorder { get {
            bool res = false;
            foreach(RegionManager neigbor in neighbors) if(neigbor.isPlayer != isPlayer) {
                    res = true;
                    break;
                }
            return res;
        } }

    public int PillageResources(float factor, int constant) {
        var res = (int)(factor  * GetResourcesToBase) + constant;
        if (resources < res) res = resources;
        resources -= res;
        return res;
    }

    public List<CardSO> GenerateLoadOut() {
        var loadout = new List<CardSO>();
        int rp;
        if (!isPlayer) rp = recruitPoints;
        else rp = baseData.recruitPoints;
        var availableUnits = new List<CardSO>(baseData.cardsToGenerate);
        for (int i = 0; i < baseData.battlefieldSize; i++) {
            if (availableUnits.Count() == 0) break;
            availableUnits.Where(card => card.value < rp).ToList();
            var cardToAdd = availableUnits[Random.Range(0, availableUnits.Count())];
            loadout.Add(cardToAdd);
            rp -= cardToAdd.value;
        }
        return loadout;
    }

    private void GenerateBattleEvent(float chance) {
        float random = chanceMultiplier * Random.Range(0, GetChanceValue);
        if (chance >= random) battleGenerator.GenerateBattle(this);
    }

    private void GenerateBattleEvent() {
        if (isPlayer) { 
            if (IsBorder || baseData.riotBoundary > GetResourcesToBase) GenerateBattleEvent(baseData.riotChance);
        } 
        else {
            if(IsBorder) GenerateBattleEvent(1.0f);
        }
    }

    private void ClearBattleEvent() {
        if (battle != null) {
            Destroy(battle);
            battle = null;
        }
    }

    private void ActualizeRecruitPoints() {
        if (!isAttacked) {
            recruitPoints += baseData.recruitRefreshRate;
            if (recruitPoints > baseData.recruitPoints) recruitPoints = baseData.recruitPoints;
        }
    }

    public void OnEndTurn() {
        ActualizeRecruitPoints();
        ClearBattleEvent();
        GenerateBattleEvent();
    }

    public void SetBattle(GameObject battleInstance) {
        battle = battleInstance;
    }

}
