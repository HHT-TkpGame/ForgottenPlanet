using System;
using System.Collections;
using UnityEngine;

public class ClueClientPoller : MonoBehaviour
{
    CluesManager cluesManager;
    const float REQUEST_INTERVAL = 4f;
    public event Action<ClueSharedInfo> OnSharedUpdated;

    public void Init(CluesManager cluesManager)
    {
        this.cluesManager = cluesManager;
    }
    public void StartLoop()
    {
        StartCoroutine(FetchClueLoop());
    }
    IEnumerator FetchClueLoop()
    {
        while (true)
        {
            yield return StartCoroutine(CluesDataGetter.Instance.ClueClient.GetClue(
                onSuccess:(res) =>
                {
                    for (int i = 0; i < res.shared_clues.Length; i++) 
                    {
                        if (res.shared_clues[i].is_shared != cluesManager.MatchClues.isShared[i])
                        {
                            OnSharedUpdated(res.shared_clues[i]);
                        }
                    }
                },
                onError: (err) =>
                {
                    Debug.Log("‹¤—LŽæ“¾Ž¸”s");
                }));
            yield return new WaitForSeconds(REQUEST_INTERVAL);
        }
    }
}
