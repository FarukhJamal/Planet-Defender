using UnityEngine;
public class ScoreService : MonoBehaviour
{
    public static ScoreService Instance { get; private set; }
    public int Score { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Add(int amount)
    {
        Score += amount;
        GameEvents.RaiseScoreChanged(Score);
    }
}