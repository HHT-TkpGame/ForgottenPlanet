using UnityEngine;

public class InGameEnder : MonoBehaviour
{
    [SerializeField] AgentReturnUI agentReturnUI;
    GameStateRequester requester;
    [SerializeField] GameStateRequestPoller poller;
    [SerializeField] GameTimer gameTimer;
    GameStateManager state;
    void Start()
    {
        requester = new GameStateRequester();
        state = GameStateManager.Instance;
        poller.Initialize(requester, state);
        StartCoroutine(poller.PollLoop());
        agentReturnUI.Init(this);
        if (MatchingManager.IsCommander) {
            gameTimer.OnTimerEnded += IncrementState;
        }
        poller.OnStateUpdated += TransitionToAnswer;
    }
    void OnDestroy()
    {
        if (MatchingManager.IsCommander)
        {
            gameTimer.OnTimerEnded -= IncrementState;
        }
        poller.OnStateUpdated -= TransitionToAnswer;
    }
    public void IncrementState()
    {
        StartCoroutine(requester.PostState());
    }
    void TransitionToAnswer()
    {
        GameStateManager.Instance.SetProgress(GameProgress.Answer);
    }
}
