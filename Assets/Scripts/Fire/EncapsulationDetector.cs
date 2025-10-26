using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class EncapsulationDetector : MonoBehaviour
{
    [Header("Encapsulation Settings")]
    public float detectionRadius = 1.5f; // distance around fireball to detect segments
    public int requiredSegments = 3;     // how many snake segments needed to nullify
    public LayerMask segmentMask = ~0;   // optionally restrict to "SnakeSegment" layer

    private Transform _earth;
    private BaseFireball _fireball;

    public void Init(Transform earthRef)
    {
        _earth = earthRef;
        _fireball = GetComponent<BaseFireball>();
    }

    void Update()
    {
        if (!_fireball || !_fireball.Body) return;

        // detect snake segments near this fireball
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, segmentMask);
        int segmentCount = 0;

        foreach (var h in hits)
        {
            if (h.CompareTag("SnakeSegment"))
                segmentCount++;
        }

        // if surrounded by enough snake segments → nullify
        if (segmentCount >= requiredSegments)
        {
            _fireball.Nullify();
            ScoreService.Instance?.AddScore(1);
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
#endif
}
