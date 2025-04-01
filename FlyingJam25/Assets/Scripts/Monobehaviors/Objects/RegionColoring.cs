using UnityEngine;

public class RegionColoring : MonoBehaviour
{
    SpriteRenderer sr;
    [SerializeField] Material original;
    [SerializeField] Material gray;

    private void Awake() {
        sr = GetComponent<SpriteRenderer>();
    }

    public void SetNormalColoring() {
        sr.material = original;
    }

    public void SetEnemyColoring() {
        sr .material = gray;
    }
}
