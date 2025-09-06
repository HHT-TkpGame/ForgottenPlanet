using System;
using UnityEngine;
using System.Collections;

public class GameStateRequestPoller : MonoBehaviour
{
    [SerializeField] float interval = 1.0f;
    GameStateManager gameState;
    GameStateRequester requester;
    //イベント定義
    public event Action OnStateUpdated;


    public void Initialize(GameStateRequester requester, GameStateManager gameState)
    {
        this.requester = requester;
        this.gameState = gameState;
    }

    /// <summary>
    /// ゲーム進行状況を定期に取得
    /// </summary>
    /// <returns></returns>
    public IEnumerator PollLoop()
    {
        while (true)
        {
            yield return StartCoroutine(requester.GetState(onSuccess: (prog) =>
            {
                //状態が変わった時だけ発火
                if (prog.game_progress != (int)gameState.CurrentState)
                {
                    OnStateUpdated?.Invoke();
                }
            },
            onError: (err) =>
            {
                Debug.Log(err);
            }
            ));
            yield return new WaitForSeconds(interval);
        }
    }
}
