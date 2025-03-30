using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[CreateAssetMenu(fileName = "CardSO", menuName = "Scriptable Objects/Data/CardSO")]
public class CardSO : ScriptableObject {
    public Sprite sprite;
    public string cardName;
    public int value;
    public PrefferedPosSO prefPos;
    public int[] attackValues;
    public int[] vulnerabilityValues;
    public List<Effect> effects;
}
