using UnityEngine;

public class BattleGenerator : MonoBehaviour
{
    GameObject battlePrefab;
    [SerializeField] private float difX;
    [SerializeField] private float difY;
    public void GenerateBattle(RegionManager region) {
        if (battlePrefab == null) return;
        GameObject battleInstace = Instantiate(battlePrefab, region.transform);
        battleInstace.transform.localPosition = new Vector3(difX, difY, 0);
        region.SetBattle(battleInstace);
    }
    
}
