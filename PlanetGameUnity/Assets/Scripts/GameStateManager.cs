using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance{ get; private set; }
    public GameProgress CurrentProgress { get; private set; } = GameProgress.Matching;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void SetProgress(GameProgress progress)
    {
        CurrentProgress = progress;
        Debug.Log("updateProg: " + progress.ToString());
    }
}
