using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class LoadingOverlay : MonoBehaviour
{
    CanvasGroup _cg;

    void Awake() { _cg = GetComponent<CanvasGroup>(); _cg.alpha = 1f; _cg.blocksRaycasts = true; }

    public IEnumerator FadeOut(float duration = 0.5f)
    {
        float t = 0f; float start = _cg.alpha;
        while (t < duration)
        {
            t += Time.deltaTime;
            _cg.alpha = Mathf.Lerp(start, 0f, t / duration);
            yield return null;
        }
        _cg.alpha = 0f;
        _cg.blocksRaycasts = false;
        gameObject.SetActive(false);
    }
}
