using UnityEngine;
using System.Collections;
using System;

public class ChatClientPoller : MonoBehaviour
{
    const float REQUEST_INTERVAL = 4f;
    
    public void StartLoop(Action<ChatDataList> onFetched)
    {
        StartCoroutine(FetchChatLoop(onFetched));
    }
    //ポーリングで定期的にチャットを取得する
    IEnumerator FetchChatLoop(Action<ChatDataList> onFetched)
    {
        while (NetworkStateManager.CurrentState == NetworkStateManager.NetworkState.Connected)
        {
            yield return StartCoroutine(ChatManager.Instance.Client.FetchChats(onFetched, () => { }));
            yield return new WaitForSeconds(REQUEST_INTERVAL);
        }
    }
}
