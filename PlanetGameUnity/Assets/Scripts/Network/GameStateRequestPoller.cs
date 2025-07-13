using System;
using UnityEngine;
using System.Collections;

public class GameStateRequestPoller : MonoBehaviour
{
    [SerializeField] float interval = 1.0f;
    GameStateManager gameState;
    GameStateRequester requester;
    //�C�x���g��`
    public event Action OnStateUpdated;


    public void Initialize(GameStateRequester requester, GameStateManager gameState)
    {
        this.requester = requester;
        this.gameState = gameState;
    }

    /// <summary>
    /// �Q�[���i�s�󋵂����Ɏ擾
    /// </summary>
    /// <returns></returns>
    public IEnumerator PollLoop()
    {
        while (true)
        {
            yield return StartCoroutine(requester.GetState(onSuccess: (prog) =>
            {
                //Debug.Log("svr:"+prog.game_progress);
                //Debug.Log("loc:"+gameState.CurrentProgress);
                //��Ԃ��ς��������������
                if (prog.game_progress != (int)gameState.CurrentProgress)
                {
                    OnStateUpdated?.Invoke();
                    Debug.Log("Success");
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
