using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [Header("Buttons")]
    public Button restartBtn;
    public Button mainMenuBtn;

    void Awake()
    {
        // hide initially
        gameObject.SetActive(false);
        GameEvents.OnGameOver += ShowGameOver;
        restartBtn.onClick.AddListener(RestartGame);
        mainMenuBtn.onClick.AddListener(MainMenu);
    }


    void OnDestroy()
    {
        GameEvents.OnGameOver -= ShowGameOver;
        restartBtn.onClick.RemoveAllListeners();
        mainMenuBtn.onClick.RemoveAllListeners();
    }

    void ShowGameOver()
    {
        // show the UI and pause game
        gameObject.SetActive(true);
      //  Time.timeScale = 0f;
    }

    void RestartGame()
    {
       // Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void MainMenu()
    {
       // Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
