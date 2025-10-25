using UnityEngine;

public class FireballSpawner : MonoBehaviour
{
    public BaseFireball fireballPrefab;
    public Transform earth;
    public float radius = 40f; // spawn sphere radius
    public float interval = 1.5f;
    float _t;

    void Update()
    {
        _t += Time.deltaTime;
        if (_t >= interval)
        {
            _t = 0f;
            Spawn();
        }
    }

    void Spawn()
    {
        // random point in sky hemisphere above Earth
        Vector3 dir = Random.onUnitSphere; dir.y = Mathf.Abs(dir.y); dir.Normalize();
        Vector3 start = earth.position + dir * radius;
        var f = Instantiate(fireballPrefab);
        var enc = f.GetComponent<EncapsulationDetector>();
        if (enc != null) enc.Init(earth);
        f.Init(start, earth.position);
    }
}