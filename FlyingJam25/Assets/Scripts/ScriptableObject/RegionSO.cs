using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "RegionSO", menuName = "Scriptable Objects/RegionSO")]
public class RegionSO : ScriptableObject
{
    public int recruitPoints;
    public int recruitRefreshRate;

    public int resources;
    public int resourceRefreshRate;

    public float riotChance = 0.2f;
    public float riotBoundary;
    
    public int battlefieldSize;
    public List<CardSO> cardsToGenerate;
}
