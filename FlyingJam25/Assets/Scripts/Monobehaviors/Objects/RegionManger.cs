using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class RegionManager : MonoBehaviour {

    public RegionSO baseData;
    RegionColoring rc;
    [SerializeField] EmptyEvent onBattleCleared;
    [SerializeField] EmptyEvent onBattleGenerated;

    public string regionName;

    public int recruitPoints;
    public int resources;
    public int regionDistance;

    public bool isAttacked = false;
    public bool isPlayer;

    [SerializeField] private List<RegionManager> neighbors;
    public RegionEvent OnChangeOwner;
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
        } 
    }

    public int GetPilageRes(float factor, int constant) { 
        int res = baseData.resourceRefreshRate;
        if (isPlayer) res = (int)(factor * GetResourcesToBase + constant) * baseData.resourceRefreshRate;
        if (res > resources) res = resources;
        return res;
    }

    public int PillageResources(float factor, int constant) {
        var res = GetPilageRes(factor, constant);
        resources -= res;
        return res;
    }

    public List<CardSO> GenerateLoadOut() {
        var loadout = new List<CardSO>();
        int rp;
        var availableUnits = new List<CardSO>(baseData.cardsToGenerate);
        if (!isPlayer) {
            rp = recruitPoints;
            int minimalRP = availableUnits.Any() ? availableUnits.Min(card => card.value) : 0;
            if (rp < minimalRP) rp = minimalRP;
        }
        else rp = baseData.recruitPoints;
        for (int i = 0; i < baseData.battlefieldSize; i++) {
            availableUnits = availableUnits.Where(card => card.value <= rp).ToList();
            if (availableUnits.Count() == 0) break;
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
        StartCoroutine(BattleEventCoroutine(chance));
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
        if (battle != null) StartCoroutine(ClearBattleCoroutine());
    }

    private void ActualizeRecruitPoints() {
        if (!isAttacked && !isPlayer) {
            recruitPoints += baseData.recruitRefreshRate;
            if (recruitPoints > baseData.recruitPoints) recruitPoints = baseData.recruitPoints;
        }
    }

    public void OnEndTurn() {
        StartCoroutine(OnEndTurnCoroutine());
    }

    public IEnumerator OnEndTurnCoroutine() {
        SolveRemainingBattle();
        yield return StartCoroutine(ClearBattleCoroutine());
        ActualizeRecruitPoints();
        isAttacked = false;
        onBattleCleared.Raise(new Empty());
        //GenerateBattleEvent();
    }

    public void OnStartTurn() {
        GenerateBattleEvent();
        onBattleGenerated.Raise(new Empty());
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
        Start();
    }


    private void Awake() {
        battleGenerator = GetComponent<BattleGenerator>();
        rc = GetComponent<RegionColoring>();
        resources = baseData.resources;
        recruitPoints = baseData .recruitPoints;
    }

    private void Start() {
        if (!isPlayer && rc != null) rc.SetEnemyColoring();
    }

    private void SolveRemainingBattle() {
        if (battle != null && isPlayer) { 
            isPlayer = false;
            recruitPoints = baseData.recruitPoints;
            OnChangeOwner.Raise(this);
        }
    }

    private IEnumerator BattleEventCoroutine(float chance) {
        float random = chanceMultiplier * Random.Range(0, GetChanceValue);
        float duration = 0.6f;
        float timer = 0f;
        if (chance < random) {
            yield return new WaitForSeconds(duration);
            yield break;
        }

        if (battleGenerator == null) {
            Debug.Log("Null battleGenerator in RegionManager");
            yield return new WaitForSeconds(duration);
            yield break;
        }

        isAttacked = true;

        // Create the battle object (or whatever visuals you use)
        battleGenerator.GenerateBattle(this);
        var clickable = battle.GetComponent<IsClickable>();
        if (clickable != null) clickable.isEnabled = false;

        // Optional: assign initial scale and animate
        Transform battleTransform = battle.transform;
        battleTransform.localScale = Vector3.zero;

        while (timer < duration) {
            float t = timer / duration;
            battleTransform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);
            timer += Time.deltaTime;
            yield return null;
        }

        // Ensure final scale
        battleTransform.localScale = Vector3.one;
        if (clickable != null) clickable.isEnabled = true;
    }

    private IEnumerator ClearBattleCoroutine() {
        float duration = 0.6f;
        float timer = 0f;

        if (battle == null) {
            yield return new WaitForSeconds(duration);
            yield break;
        }
        var battleTransform = battle.transform;
        var clickable = battle.GetComponent<IsClickable>();
        if (clickable != null) clickable.isEnabled = false;

        Vector3 originalScale = battleTransform.localScale;

        // Determine target color from material
        float target = isPlayer ? 1.0f : 0.0f;

        // Start fading color
        if (rc != null) StartCoroutine(rc.LerpToMaterialColor(target, duration));

        // Shrink battle
        while (timer < duration) {
            float t = timer / duration;
            battleTransform.localScale = Vector3.Lerp(originalScale, Vector3.zero, t);
            timer += Time.deltaTime;
            yield return null;
        }

        battleTransform.localScale = Vector3.zero;
        Destroy(battle.gameObject);
        battle = null;

        // Finish with final material assignment
        if (rc != null) {
            if (isPlayer) rc.SetNormalColoring();
            else rc.SetEnemyColoring();
        }
    }

    public void RecolorRegion(float target, float duration) {
        if (rc != null) StartCoroutine(rc.LerpToMaterialColor(target, duration));
    }   
}
