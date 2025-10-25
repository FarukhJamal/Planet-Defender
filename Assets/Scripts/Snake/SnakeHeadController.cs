using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SnakeHeadController : MonoBehaviour
{
    public Transform earth; // center at world origin
    public float orbitRadius = 10f;
    public float moveSpeed = 25f;
    public float turnSpeed = 120f; // degrees/s when using input
    public GameObject segmentPrefab;
    public int initialSegments = 10;
    public float segmentSpacing = 0.6f;
    public MonoBehaviour inputProvider; // drag KeyboardInputReader

    readonly List<Transform> _segments = new List<Transform>();
    IInputReader _input;
    Rigidbody _rb;

    void Awake()
    {
        _input = inputProvider as IInputReader;
        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = false; _rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void Start()
    {
        // Spawn segments behind head along the tangent
        Vector3 normal = (transform.position - earth.position).normalized;
        Vector3 tangent = Vector3.Cross(Vector3.up, normal).normalized;
        Vector3 pos = transform.position;
        for (int i = 0; i < initialSegments; i++)
        {
            pos -= tangent * segmentSpacing;
            var seg = Instantiate(segmentPrefab, pos, Quaternion.identity).transform;
            seg.tag = "SnakeSegment";
            _segments.Add(seg);
        }
    }

    void FixedUpdate()
    {
        if (!earth) return;
        Vector3 normal = (transform.position - earth.position).normalized;
        // keep on sphere
        transform.position = earth.position + normal * orbitRadius;
        // desired forward is along local tangent.
        float turn = _input != null ? _input.GetTurn() : 0f;
        transform.RotateAround(earth.position, Vector3.up, turn * turnSpeed * Time.fixedDeltaTime);
        transform.position = (transform.position - earth.position).normalized * orbitRadius + earth.position;
        transform.LookAt(earth.position, Vector3.up); // face inward for clarity

        // advance along great-circle by projecting forward around Y
        transform.RotateAround(earth.position, transform.right, 0f); // placeholder if you want pitch
        transform.RotateAround(earth.position, Vector3.up, moveSpeed * Time.fixedDeltaTime);

        // update followers
        Vector3 prevPos = transform.position;
        for (int i = 0; i < _segments.Count; i++)
        {
            var seg = _segments[i];
            Vector3 dir = (seg.position - prevPos);
            float dist = dir.magnitude;
            if (dist > segmentSpacing)
            {
                seg.position = prevPos + dir.normalized * segmentSpacing;
            }
            // constrain to sphere shell (nice visual)
            Vector3 n = (seg.position - earth.position).normalized;
            seg.position = earth.position + n * orbitRadius;
            prevPos = seg.position;
        }
    }

    public IReadOnlyList<Transform> Segments => _segments;
}