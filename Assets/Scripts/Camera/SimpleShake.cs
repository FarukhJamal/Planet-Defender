using UnityEngine;

public class SimpleShake : MonoBehaviour
{
    Vector3 _basePos; float _time; float _duration; float _intensity;

    void Awake() => _basePos = transform.localPosition;

    void OnEnable() => GameEvents.OnFireballHitEarth += Shake;
    void OnDisable() => GameEvents.OnFireballHitEarth -= Shake;

    void Shake()
    {
        _duration = 0.4f; _intensity = 0.25f; _time = _duration;
    }

    void LateUpdate()
    {
        if (_time <= 0f) { transform.localPosition = _basePos; return; }
        _time -= Time.deltaTime;
        transform.localPosition = _basePos + Random.insideUnitSphere * _intensity;
    }
}