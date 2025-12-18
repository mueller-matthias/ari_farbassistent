using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ARPreviewLoader : MonoBehaviour
{
    [Header("Scene Names (exact)")]
    public string arSceneName = "AR";

    [Header("Auto load AR behind menu")]
    public bool loadOnStart = true;

    public static WallPainter WallPainterInstance { get; private set; }

    public IEnumerator Start()
    {
        if (!loadOnStart) yield break;

        // AR Scene additiv laden
        if (!SceneManager.GetSceneByName(arSceneName).isLoaded)
        {
            var op = SceneManager.LoadSceneAsync(arSceneName, LoadSceneMode.Additive);
            while (!op.isDone) yield return null;
        }

        // ✅ AR Scene aktiv setzen (wichtig bei Additive)
        var arScene = SceneManager.GetSceneByName(arSceneName);
        if (arScene.IsValid() && arScene.isLoaded)
            SceneManager.SetActiveScene(arScene);

        // WallPainter finden (in der AR Szene)
        CacheWallPainter();
    }

    public void CacheWallPainter()
    {
        WallPainterInstance = FindFirstObjectByType<WallPainter>();

        if (WallPainterInstance == null)
            Debug.LogError("ARPreviewLoader: Kein WallPainter in der AR Scene gefunden!");
    }
}
