using System;

public static class GameEvents
{
    public static event Action<int> OnScoreChanged; // total score
    public static event Action OnFireballHitEarth;
    public static event Action OnFireballNullified; // for VFX/audio

    public static void RaiseScoreChanged(int total) => OnScoreChanged?.Invoke(total);
    public static void RaiseFireballHitEarth() => OnFireballHitEarth?.Invoke();
    public static void RaiseFireballNullified() => OnFireballNullified?.Invoke();
}