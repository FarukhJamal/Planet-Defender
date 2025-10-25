using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class EncapsulationDetector : MonoBehaviour
{
    public float detectRadius = 1.2f;
    public float minAngleBetween = 25f; // deg between segment directions
    public int requiredSegments = 3;
    public int scorePerNullify = 10;

    SphereCollider _trigger;
    readonly HashSet<Transform> _nearSegments = new HashSet<Transform>();
    Transform _earth;
    IFireball _fireball;

    void Awake()
    {
        _trigger = GetComponent<SphereCollider>();
        _trigger.isTrigger = true;
        _trigger.radius = detectRadius;
        _fireball = GetComponent<IFireball>();
    }

    public void Init(Transform earth)
    {
        _earth = earth;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SnakeSegment"))
        {
            _nearSegments.Add(other.transform);
            TryNullify();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SnakeSegment"))
            _nearSegments.Remove(other.transform);
    }

    void TryNullify()
    {
        if (_nearSegments.Count < requiredSegments || _earth == null) return;

        // Angular diversity check
        var list = new List<Transform>(_nearSegments);
        int validPairs = 0;
        Vector3 center = _earth.position;
        for (int i = 0; i < list.Count; i++)
            for (int j = i + 1; j < list.Count; j++)
            {
                Vector3 a = (list[i].position - center).normalized;
                Vector3 b = (list[j].position - center).normalized;
                float ang = Vector3.Angle(a, b);
                if (ang >= minAngleBetween) validPairs++;
            }
        if (validPairs >= 3) // enough spread
        {
            ScoreService.Instance.Add(scorePerNullify);
            _fireball.Nullify();
        }
    }
}