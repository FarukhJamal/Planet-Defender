using System;

public static class GameEvents
{
    public static event Action<int> OnScoreChanged;
    public static event Action OnFireballHitEarth;
    public static event Action OnFireballNullified;
    public static event Action OnGameOver;

    public static event Action OnSegmentLost;
    public static event Action OnSegmentGained;

    public static void RaiseScoreChanged(int total) => OnScoreChanged?.Invoke(total);
    public static void RaiseFireballHitEarth() => OnFireballHitEarth?.Invoke();
    public static void RaiseFireballNullified() => OnFireballNullified?.Invoke();
    public static void RaiseGameOver() => OnGameOver?.Invoke();

    public static void RaiseSegmentLost() => OnSegmentLost?.Invoke();
    public static void RaiseSegmentGained() => OnSegmentGained?.Invoke();
}
