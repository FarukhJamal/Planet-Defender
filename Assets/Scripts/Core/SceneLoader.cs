using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    private class LoaderHook : MonoBehaviour { }

    public static void LoadGame()
    {
        var go = new GameObject("SceneLoaderHook");
        Object.DontDestroyOnLoad(go);
        go.AddComponent<LoaderHook>().StartCoroutine(LoadRoutine(go));
    }

    private static IEnumerator LoadRoutine(GameObject hook)
    {
        var op = SceneManager.LoadSceneAsync("Game", LoadSceneMode.Single);
        while (!op.isDone) yield return null;
        Object.Destroy(hook); // Game scene handles its own overlay now
    }
}
