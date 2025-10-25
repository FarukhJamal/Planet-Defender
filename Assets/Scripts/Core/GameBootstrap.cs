using System.Collections;
using UnityEngine;

public class GameBootstrap : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject earthPrefab;
    public GameObject snakeHeadPrefab;
    public GameObject snakeSegmentPrefab;
    public GameObject fireballSpawnerPrefab;

    [Header("UI")]
    public LoadingOverlay loadingOverlay; // assign from scene

    [Header("Camera")]
    public CameraOrbit orbitCam;          // assign Main Camera component
    public SimpleShake cameraShake;       // assign Main Camera component

    [Header("Snake")]
    public int initialSegments = 10;
    public float orbitRadius = 10f;

    private Transform _earth;
    private GameObject _spawner;

    private IEnumerator Start()
    {
        // Show overlay immediately (alpha should already be 1 via Awake)
        yield return StartCoroutine(SpawnAll());
        // Small buffer to ensure physics settled
        yield return new WaitForEndOfFrame();
        if (loadingOverlay) yield return loadingOverlay.FadeOut(0.5f);
        // Enable spawner AFTER fade
        if (_spawner) _spawner.SetActive(true);
    }

    private IEnumerator SpawnAll()
    {
        // 1) Earth
        var earth = Instantiate(earthPrefab, Vector3.zero, Quaternion.identity);
        _earth = earth.transform;

        // 2) Camera target
        if (orbitCam) orbitCam.target = _earth;

        // 3) Snake head
        var head = Instantiate(snakeHeadPrefab);
        // place on orbit radius at some longitude
        Vector3 startDir = new Vector3(0, 0, 1); // north
        head.transform.position = startDir.normalized * orbitRadius;
        var headCtrl = head.GetComponent<SnakeHeadController>();
        headCtrl.earth = _earth;
        headCtrl.segmentPrefab = snakeSegmentPrefab;
        headCtrl.initialSegments = initialSegments;
        headCtrl.orbitRadius = orbitRadius;

        // 4) Fireball spawner (initially inactive)
        _spawner = Instantiate(fireballSpawnerPrefab);
        _spawner.SetActive(false);
        var sp = _spawner.GetComponent<FireballSpawner>();
        sp.earth = _earth;

        // 5) Hook EncapsulationDetector to earth on the fireball prefab (done in prefab),
        //    but spawner will pass earth to instances (already in previous plan).
        yield return null;
    }
}
