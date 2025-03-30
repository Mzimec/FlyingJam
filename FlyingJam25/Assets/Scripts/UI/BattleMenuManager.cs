using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;
using UnityEngine.UIElements;

public class BattleMenuManager : MonoBehaviour
{
    [SerializeField] private UIDocument battleMenu;
    [SerializeField] PlayerManager player;
    [SerializeField] VisualTreeAsset visualTree;

    BattleManager battleManager;


    ScrollView hand;

    private void Awake() {
    }

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

    private string GetEffectDescription(CardManager card) {
        string res = "";
        if (card == null) Debug.LogError("Null Card");

        foreach (var effect in card.effects) res += effect.description + "\n";
        return res

    }

    private VisualElement DrawCard(CardManager card) {
        VisualElement ve = visualTree.CloneTree();

        Label cardNameLabel = ve.Q<Label>("CARD-name");
        cardNameLabel.text = card.baseData.cardName;

        Label effectLabel = ve.Q<Label>("EFFECT");
        string effectDescription = 
        return ve;

    }
}
