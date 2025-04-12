using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "PopUp", menuName = "Scriptable Objects/PopUp")]
public class PopUp : ScriptableObject
{
    public string description;
    public string title;
    public Sprite image;
    public UnityEvent response;

    public void Execute() {
        response.Invoke();
    }

}
