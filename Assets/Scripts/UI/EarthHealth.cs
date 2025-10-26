using UnityEngine;
using UnityEngine.UI;

public class EarthHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 5;
    private int _currentHealth;

    [Header("UI References")]
    public Slider healthBar;         // world-space slider
    public Camera mainCamera;        // assign manually or auto-detect
    public GameOverUI gameOverPanel; // optional UI panel shown on Game Over

    private bool _isGameOver = false;
    

    void Start()
    {
        _currentHealth = maxHealth;
        UpdateUI();

        if (mainCamera == null)
            mainCamera = Camera.main;

        GameEvents.OnFireballHitEarth += OnFireballHitEarth;
        if (gameOverPanel != null)
            gameOverPanel.gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        GameEvents.OnFireballHitEarth -= OnFireballHitEarth;
    }

    void Update()
    {
        // Keep health bar facing camera (billboard effect)
        if (healthBar != null && mainCamera != null)
        {
            healthBar.transform.LookAt(
                healthBar.transform.position + mainCamera.transform.rotation * Vector3.forward,
                mainCamera.transform.rotation * Vector3.up
            );
        }
    }

    void OnFireballHitEarth()
    {
        if (_isGameOver) return;

        _currentHealth--;
        UpdateUI();

        // Notify snake to remove a segment
        GameEvents.RaiseSegmentLost();

        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            GameOver();
        }
    }

    void UpdateUI()
    {
        if (healthBar != null)
            healthBar.value = (float)_currentHealth / maxHealth;
    }

    void GameOver()
    {
        if (_isGameOver) return;

        _isGameOver = true;
        Debug.Log(" Earth Destroyed — Game Over!");

        // Raise global event (for other systems like UI, input, spawner, etc.)
        GameEvents.RaiseGameOver();

        // Show Game Over UI
        if (gameOverPanel != null)
            gameOverPanel.gameObject.SetActive(true);

        // Optionally stop time
       // Time.timeScale = 0f;
    }
}



