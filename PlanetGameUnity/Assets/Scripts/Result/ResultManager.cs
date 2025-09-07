using UnityEngine;

public class ResultManager : MonoBehaviour
{
    const int DEFAULT_ANSWER = 1;
    AnswerRequester answerRequester;
    [SerializeField] GameTimer timer;
    [SerializeField] ResultUIController controller;
    GameStateManager stateManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        stateManager = GameStateManager.Instance;
        timer.enabled = false;
        answerRequester = new AnswerRequester();
        StartCoroutine(answerRequester.FetchAnswerAndFoundClues(
            onSuccess: (ans, foundClues) =>
            {
                controller.SetDisplayData(ans, foundClues);
            },
            onError: () =>
            {
                controller.SetDisplayData(DEFAULT_ANSWER, 0);
            }));
        timer.OnTimerEnded += TransitionToTitle;
        controller.onScrollEnded += EnableTimer;
        controller.OnEnded += TransitionToTitle;
    }
    void EnableTimer()
    {
        timer.enabled = true;
    }
    void TransitionToTitle()
    {
        stateManager.SetProgress(GameProgress.Matching);
        //シングルトンのオブジェクトを破棄
        ChatManager.Instance?.Dispose();
        CluesDataGetter.Instance?.Dispose();
    }
}
