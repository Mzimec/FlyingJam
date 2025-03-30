using UnityEngine;
using UnityEngine.UIElements;

public class BattleMenuManager : MonoBehaviour
{
    [SerializeField] private UIDocument battleMenu;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var root = battleMenu.rootVisualElement;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnFight() { }

    private void OnCancel() { }

    private void DrawCard(CardManager card) {

    }
}
