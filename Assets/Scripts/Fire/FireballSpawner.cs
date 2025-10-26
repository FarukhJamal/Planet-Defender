using System.Collections.Generic;
using UnityEngine;

public class FireballSpawner : MonoBehaviour
{
    [Header("References")]
    public Transform earth;
    public GameObject fireballPrefab;

    [Header("Settings")]
    public int poolSize = 30;
    public float spawnInterval = 2f;
    public float spawnDistance = 20f;
    public bool randomizeSpawnInterval = true;
    public float randomOffset = 0.5f; // e.g., interval ±0.5s variation

    private List<GameObject> _pool = new List<GameObject>();
    private float _timer;
    private bool _isGameOver = false;
    private int _nextIndex = 0; // for cyclic reuse

    void Start()
    {
        // Create object pool
        for (int i = 0; i < poolSize; i++)
        {
            var fb = Instantiate(fireballPrefab, transform);
            fb.SetActive(false);
            _pool.Add(fb);
        }

        // Subscribe to game over event
        GameEvents.OnGameOver += OnGameOver;
    }

    void OnDestroy()
    {
        // Unsubscribe for safety
        GameEvents.OnGameOver -= OnGameOver;
    }

    void Update()
    {
        if (_isGameOver) return; // stop spawning

        _timer += Time.deltaTime;
        if (_timer >= spawnInterval)
        {
            SpawnFireball();
            _timer = 0f;

            if (randomizeSpawnInterval)
                spawnInterval = Mathf.Max(0.5f, spawnInterval + Random.Range(-randomOffset, randomOffset));
        }
    }

    void SpawnFireball()
    {
        var fireball = GetNextFireball();
        if (fireball == null) return;

        Vector3 randomDir = Random.onUnitSphere;
        Vector3 spawnPos = earth.position + randomDir * spawnDistance;

        fireball.transform.position = spawnPos;
        fireball.transform.rotation = Quaternion.LookRotation((earth.position - spawnPos).normalized);

        var baseFireball = fireball.GetComponent<BaseFireball>();
        var detector = fireball.GetComponent<EncapsulationDetector>();

        baseFireball.Init(earth);
        detector.Init(earth);

        fireball.SetActive(true);
    }

    //  Recycle system — reuses fireballs in sequence
    GameObject GetNextFireball()
    {
        var fb = _pool[_nextIndex];
        _nextIndex = (_nextIndex + 1) % _pool.Count; // move cyclically

        // If it's still active, reset it before reusing
        if (fb!=null && fb.activeInHierarchy)
        {
            fb.SetActive(false); // deactivate instantly
        }

        return fb;
    }

    void OnGameOver()
    {
        _isGameOver = true;

        // Disable all active fireballs
        foreach (var fb in _pool)
        {
            if (fb.activeInHierarchy)
                fb.SetActive(false);
        }

        Debug.Log(" Fireball Spawning Stopped — Game Over.");
    }
}
