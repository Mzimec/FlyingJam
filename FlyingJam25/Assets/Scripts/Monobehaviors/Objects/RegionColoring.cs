using System.Collections;
using UnityEngine;

public class RegionColoring : MonoBehaviour
{
    SpriteRenderer sr;

    private void Awake() {
        sr = GetComponent<SpriteRenderer>();
    }

    public void SetNormalColoring() {
        sr.material.SetFloat("_tint", 1.0f);
    }

    public void SetEnemyColoring() {
        sr .material.SetFloat("_tint", 0.0f);
    }

    public IEnumerator LerpToMaterialColor(float target, float duration) {
        if (!sr.material.HasProperty("_tint")) {
            Debug.LogWarning("Material does not have a _tint property.");
            yield break;
        }

        float start = sr.material.GetFloat("_tint");
        float time = 0f;

        while (time < duration) {
            float t = time / duration;
            float value = Mathf.Lerp(start, target, t);
            sr.material.SetFloat("_tint", value);
            time += Time.deltaTime;
            yield return null;
        }

        sr.material.SetFloat("_tint", 0.0f);
    }
}
