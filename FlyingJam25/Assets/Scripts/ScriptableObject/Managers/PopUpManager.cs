using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "PopUpManager", menuName = "Scriptable Objects/PopUpManager")]
public class PopUpManager : ScriptableObject
{
    public void Revolution() {
        PlayerManager playerManager = FindAnyObjectByType<PlayerManager>();
        playerManager.OnRevolution();
    }
}
