using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance{ get; private set; }
    public GameProgress CurrentState { get; private set; } = GameProgress.Matching;
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
    public void SetProgress(GameProgress newState)
    {
        if(CurrentState == newState) return;
        CurrentState = newState;
        SceneChanger.Instance.Change(newState);
    }
}
