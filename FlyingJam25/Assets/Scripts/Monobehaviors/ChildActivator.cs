using UnityEngine;

public class ChildActivator : MonoBehaviour
{
    Transform child;
    private void Awake() {
        child = transform.GetChild(0);
    }

    public void OnActivate() {
        child.gameObject.SetActive(true);
    }

    public void OnDeactivate() {
        child.gameObject.SetActive(false);
    }
}
