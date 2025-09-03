using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public static SceneChanger Instance { get; private set; }
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
    readonly string[] SCENE_NAMES = { "OpeningScene", "RoleSetScene", "InGameScene", "AnswerScene", "ResultScene" };
    public void Change(GameProgress state)
    {
        SceneManager.LoadScene(SCENE_NAMES[(int)state]);
    }
}
