using UnityEngine;

public class InGameEnder : MonoBehaviour
{
    GameStateRequester requester;
    [SerializeField] GameStateRequestPoller poller;
    [SerializeField] GameTimer gameTimer;
    void Start()
    {
        requester = new GameStateRequester();
        if (MatchingManager.IsCommander) {
            gameTimer.OnTimerEnded += SendRequest;
        }
        poller.OnStateUpdated += TransitionToAnswer;
    }
    void OnDestroy()
    {
        if (MatchingManager.IsCommander)
        {
            gameTimer.OnTimerEnded -= SendRequest;
        }
        poller.OnStateUpdated -= TransitionToAnswer;
    }
    void SendRequest()
    {
        StartCoroutine(requester.PostState());
    }
    void TransitionToAnswer()
    {
        GameStateManager.Instance.SetProgress(GameProgress.Answer);
    }
}
