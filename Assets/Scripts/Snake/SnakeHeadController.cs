using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SnakeHeadController : MonoBehaviour
{
    [Header("References")]
    public Transform earth;
    public MonoBehaviour inputProvider; // KeyboardInputReader
    public GameObject segmentPrefab;

    [Header("Movement")]
    public float orbitRadius = 11f;
    public float moveSpeed = 8f;
    public float turnSpeed = 120f;
    public float hoverHeight = 2f; // fixed height above the Earth's surface
    public int initialSegments = 10;
    public float segmentSpacing = 0.6f;
    public int minSegmentsBeforeGameOver = 3;

    private IInputReader _input;
    private readonly List<Transform> _segments = new List<Transform>();

    private float _currentAngle; // tracks Y rotation around Earth
    private Vector3 _center;

    void Awake()
    {
        _input = inputProvider as IInputReader;
    }

    void Start()
    {
        if (!earth)
        {
            Debug.LogError("Earth reference missing!");
            enabled = false;
            return;
        }

        _center = earth.position;
        _currentAngle = 0f;

        // Start position (above Earth)
        transform.position = _center + new Vector3(0f, hoverHeight, orbitRadius);
        transform.rotation = Quaternion.LookRotation(Vector3.back, Vector3.up);

        // Spawn initial segments
        Vector3 pos = transform.position;
        Vector3 forward = -transform.forward;
        for (int i = 0; i < initialSegments; i++)
        {
            pos += forward * segmentSpacing;
            var seg = Instantiate(segmentPrefab, pos, Quaternion.identity);
            seg.tag = "SnakeSegment";
            _segments.Add(seg.transform);
        }

        // Subscribe to game events
        GameEvents.OnSegmentLost += RemoveSegment;
        GameEvents.OnSegmentGained += AddSegment;
    }

    void OnDestroy()
    {
        GameEvents.OnSegmentLost -= RemoveSegment;
        GameEvents.OnSegmentGained -= AddSegment;
    }

    void FixedUpdate()
    {
        if (_input == null)
            _input = inputProvider as IInputReader;
        if (!earth) return;

        float turn = _input.GetTurn();

        // --- Orbit rotation along Y-axis ---
        _currentAngle += turn * turnSpeed * Time.fixedDeltaTime;

        // --- Calculate orbit position ---
        float rad = _currentAngle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(Mathf.Sin(rad), 0f, Mathf.Cos(rad)) * orbitRadius;
        Vector3 targetPos = _center + offset + Vector3.up * hoverHeight;

        // Smooth movement
        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.fixedDeltaTime);

        // --- Always face tangent to orbit ---
        Vector3 tangent = Vector3.Cross(Vector3.up, transform.position - _center);
        transform.rotation = Quaternion.LookRotation(tangent, Vector3.up);

        // --- Update Snake Segments ---
        Vector3 prevPos = transform.position;
        for (int i = 0; i < _segments.Count; i++)
        {
            Transform seg = _segments[i];
            Vector3 dir = seg.position - prevPos;
            float dist = dir.magnitude;

            if (dist > segmentSpacing)
                seg.position = prevPos + dir.normalized * segmentSpacing;

            // Keep segments at fixed hover height
            seg.position = new Vector3(seg.position.x, _center.y + hoverHeight, seg.position.z);
            prevPos = seg.position;
        }
    }

    // -------------------------------
    // 🔻 Segment / Game Logic
    // -------------------------------

    void RemoveSegment()
    {
        if (_segments.Count > 0)
        {
            var last = _segments[_segments.Count - 1];
            Destroy(last.gameObject);
            _segments.RemoveAt(_segments.Count - 1);

            if (_segments.Count < minSegmentsBeforeGameOver)
            {
                Debug.Log("Snake too short — Game Over!");
                GameEvents.RaiseGameOver();
            }
        }
    }

    void AddSegment()
    {
        if (_segments.Count == 0) return;
        Transform tail = _segments[_segments.Count - 1];
        Vector3 pos = tail.position - tail.forward * segmentSpacing;
        var seg = Instantiate(segmentPrefab, pos, Quaternion.identity);
        seg.tag = "SnakeSegment";
        _segments.Add(seg.transform);
    }
}
