using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class RegionManager : MonoBehaviour {

    public RegionSO baseData;
    RegionColoring rc;

    public string regionName;

    public int recruitPoints;
    public int resources;
    public int regionDistance;

    public bool isAttacked = false;
    public bool isPlayer;

    [SerializeField] private List<RegionManager> neighbors;
    private BattleGenerator battleGenerator;
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
        /*loadout = loadout.Where(card => card != null && card.prefPos != null)
                 .OrderBy(card => card.prefPos.value)
                 .ToList();*/
        return loadout;
    }

    private void GenerateBattleEvent(float chance) {
        float random = chanceMultiplier * Random.Range(0, GetChanceValue);
        if (chance >= random) {
            if (battleGenerator != null) {
                isAttacked = true;
                battleGenerator.GenerateBattle(this);
            }
            else Debug.Log("Null battleGenrator in RegionManager");
        }
    }

    private void GenerateBattleEvent() {
        if (isPlayer) { 
            if (IsBorder || baseData.riotBoundary > GetResourcesToBase) GenerateBattleEvent(baseData.riotChance);
        } 
        else {
            if(IsBorder) GenerateBattleEvent(1.0f);
        }
    }

    public void ClearBattleEvent() {
        if (battle != null) {
            Destroy(battle.gameObject);
            battle = null;
        }
        if (rc != null) {
            if (isPlayer) rc.SetNormalColoring();
            else rc.SetEnemyColoring();
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

    public void Load(RegionData data) {
        recruitPoints = data.recruitPoints;
        resources = data.resources;
        regionDistance = data.regionDistance;
        isAttacked = data.isAttacked;
        isPlayer = data.isPlayer;
    }


    private void Awake() {
        battleGenerator = GetComponentInParent<BattleGenerator>();
        rc = GetComponent<RegionColoring>();
    }

    private void Start() {
        if (!isPlayer && rc != null) rc.SetEnemyColoring();
    }
}
