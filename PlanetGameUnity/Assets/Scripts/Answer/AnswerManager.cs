using UnityEngine;

public class AnswerManager : MonoBehaviour
{
    AnswerRequester answerRequester;
    GameStateRequester stateRequester;
    [SerializeField] GameStateRequestPoller poller;
    [SerializeField] AnswerUIController answerUIController;
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;//前のシーンでlockedの可能性があるので解除する
        answerRequester = new AnswerRequester();
        stateRequester = new GameStateRequester();
        poller.Initialize(stateRequester, GameStateManager.Instance);
        StartCoroutine(poller.PollLoop());
        answerUIController.OnSendButtonClicked += SendAnswer;
        poller.OnStateUpdated += TransitionToResult;
    }
    void OnDestroy()
    {
        answerUIController.OnSendButtonClicked -= SendAnswer;
        poller.OnStateUpdated -= TransitionToResult;
    }

    void SendAnswer(int currentSelected)
    {
        StartCoroutine(answerRequester.PostAnswer(currentSelected,onSuccess: () =>
        {
            Debug.Log(currentSelected);
            StartCoroutine(stateRequester.PostState());
        },
        onError: () =>
        {
            Debug.Log("Fai");
        }));
    }
    void TransitionToResult()
    {
        GameStateManager.Instance.SetProgress(GameProgress.Result);
    }
}
