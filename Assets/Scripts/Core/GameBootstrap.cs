using System.Collections;
using UnityEngine;

public class GameBootstrap : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject earthPrefab;
    public GameObject snakeHeadPrefab;
    public GameObject snakeSegmentPrefab;
    public GameObject fireballSpawnerPrefab;

    [Header("Scene References")]
    public MonoBehaviour inputProviderInScene; // assign DiscreteTurnInputReader or KeyboardInputReader from scene
    public LoadingOverlay loadingOverlay;      // assign from scene
    public Camera mainCamera;                  // assign Main Camera (fixed top-down)

    [Header("Snake Settings")]
    public int initialSegments = 10;
    public float orbitRadius = 11f; // Earth radius (e.g. 10) + small offset above surface

    private Transform _earth;
    private GameObject _snakeHead;
    private GameObject _spawner;

    private IEnumerator Start()
    {
        // Show overlay immediately (its CanvasGroup alpha should start at 1)
        yield return StartCoroutine(SpawnAll());

        // small wait to ensure physics settles
        yield return new WaitForEndOfFrame();

        // fade out loading overlay
        if (loadingOverlay)
            yield return loadingOverlay.FadeOut(0.5f);

        // Enable spawner AFTER fade
        if (_spawner)
            _spawner.SetActive(true);
    }

    private IEnumerator SpawnAll()
    {
        // ---------- 1) Earth ----------
        var earth = Instantiate(earthPrefab, Vector3.zero, Quaternion.identity);
        _earth = earth.transform;

        // ---------- 2) Setup Main Camera ----------
        if (mainCamera)
        {
            // Fixed top-down view
            mainCamera.transform.position = new Vector3(0f, 40f, 0f);
            mainCamera.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        }

        // ---------- 3) Spawn Snake Head ----------
        Vector3 startDir = Vector3.forward; // top edge (north pole from camera view)
        Vector3 startPos = _earth.position + startDir.normalized * orbitRadius;

        _snakeHead = Instantiate(snakeHeadPrefab, startPos, Quaternion.identity);

        var headCtrl = _snakeHead.GetComponent<SnakeHeadController>();
        headCtrl.earth = _earth;
        headCtrl.segmentPrefab = snakeSegmentPrefab;
        headCtrl.initialSegments = initialSegments;
        headCtrl.orbitRadius = orbitRadius;



        // Dynamically assign the Input Provider from scene
        if (inputProviderInScene != null)
        {
            headCtrl.inputProvider = inputProviderInScene;
        }
        else
        {
            // optional fallback: auto-find one implementing IInputReader
            var inputReader = FindObjectOfType<MonoBehaviour>(true);
            if (inputReader is IInputReader)
                headCtrl.inputProvider = inputReader;
            else
                Debug.LogWarning("No input provider assigned or found for SnakeHead!");
        }

        // ---------- 4) Spawn Fireball Spawner (initially inactive) ----------
        _spawner = Instantiate(fireballSpawnerPrefab);
        _spawner.SetActive(false);

        var spawnerComp = _spawner.GetComponent<FireballSpawner>();
        spawnerComp.earth = _earth;

        yield return null;
    }
}
