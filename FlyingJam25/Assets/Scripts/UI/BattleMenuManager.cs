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
        if (card == null) {
            Debug.LogError("Card is null.");
            return string.Empty; 
        }

        if (!card.hasEffects) {
            return string.Empty;
        }

        var sb = new System.Text.StringBuilder();

        foreach (var effect in card.effects) {
            sb.AppendLine(effect.description);  
        }

        return sb.ToString();

    }

    private VisualElement DrawCard(CardManager card) {
        VisualElement ve = visualTree.CloneTree();

        Label cardNameLabel = ve.Q<Label>("CARD-name");
        cardNameLabel.text = card.baseData.cardName;

        Label effectLabel = ve.Q<Label>("EFFECT");
        string effectDescription = GetEffectDescription(card);

        VisualElement attackElement = ve.Q<VisualElement>("AA");
        Label attackLabel = attackElement.Q<Label>("aaT");
        attackLabel.text = card.attackValues[0].ToString();

        return ve;

    }
}
