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
                //��Ԃ��ς��������������
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
