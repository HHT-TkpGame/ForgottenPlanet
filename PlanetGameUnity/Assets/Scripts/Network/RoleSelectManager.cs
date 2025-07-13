using UnityEngine;

public class RoleSelectManager : MonoBehaviour
{
    [SerializeField] RoleUIManager roleUIManager;
    [SerializeField] RoleSelectPoller poller;
    RoleSelect roleSelect;
    GameStateManager gameState;
    GameStateRequester requester;
    [SerializeField] GameStateRequestPoller requestPoller;

    public bool HasConflict { get; set; }
    public bool IsReselection { get; private set; }
    public bool IsHostButtonLocked { get; set; }
    public bool IsGuestButtonLocked { get; set; }

    void Awake()
    {
        roleSelect = new RoleSelect();
        requester = new GameStateRequester();
        poller.OnSelectionUpdated += roleUIManager.UpdateUI;
        requestPoller.OnStateUpdated += TransitionToInGame;
    }
    private void Start()
    {
        gameState = GameStateManager.Instance;
        poller.Initialize(roleSelect, roleUIManager);
        roleUIManager.Initialized(this, roleSelect, requester);
        requestPoller.Initialize(requester, gameState);
        StartCoroutine(poller.PollLoop());
        StartCoroutine(requestPoller.PollLoop());
    }
    void TransitionToInGame()
    {
        SceneChangeManager.SceneChange("InGameScene");
    }
}
