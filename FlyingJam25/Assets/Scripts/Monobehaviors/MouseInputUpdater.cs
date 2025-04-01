using UnityEngine;

public class MouseInputUpdater : MonoBehaviour
{
    [SerializeField] private MouseInputManager mim;

    private void Awake() {
        if(mim != null) mim.Initialize();
    }
    void Update()
    {
        if(mim != null) mim.Update();
    }
}
