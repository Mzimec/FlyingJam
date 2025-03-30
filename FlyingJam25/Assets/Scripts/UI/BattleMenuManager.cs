using UnityEngine;
using UnityEngine.UIElements;

public class BattleMenuManager : MonoBehaviour
{
    [SerializeField] private UIDocument battleMenu;
    [SerializeField] PlayerManager player;

    BattleManager battleManager;

    ScrollView hand;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var root = battleMenu.rootVisualElement;
        hand = root.Q<ScrollView>("Hand");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnFight() { }

    private void OnCancel() { 
        battleMenu.gameObject.SetActive(false);
    }

    private void DrawCard(CardManager card) {

    }
}
