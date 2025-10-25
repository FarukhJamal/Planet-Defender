using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button exitButton;

    private void Awake()
    {
        if (playButton) playButton.onClick.AddListener(OnPlay);
        if (exitButton) exitButton.onClick.AddListener(OnExit);
    }

    private void OnPlay() => SceneLoader.LoadGame();

    private void OnExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
