using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    public TMP_Text scoreText;

    void OnEnable() => GameEvents.OnScoreChanged += UpdateScore;
    void OnDisable() => GameEvents.OnScoreChanged -= UpdateScore;

    void Start() => UpdateScore(ScoreService.Instance != null ? ScoreService.Instance.Score : 0);

    void UpdateScore(int total)
    {
        if (scoreText) scoreText.text = $" {total}";
    }
}