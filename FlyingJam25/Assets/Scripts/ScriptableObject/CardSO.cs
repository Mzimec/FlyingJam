using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum EPrefferedPosition{ FRONT, MID, BACK };

[CreateAssetMenu(fileName = "CardSO", menuName = "Scriptable Objects/CardSO")]
public class CardSO : ScriptableObject {
    public Sprite sprite;
    public string cardName;
    public int value;
    public EPrefferedPosition prefferedPosition;
    public int[] attackValues;
    public int[] vulnerabilityValues;
    public List<Effect> effects;
}
