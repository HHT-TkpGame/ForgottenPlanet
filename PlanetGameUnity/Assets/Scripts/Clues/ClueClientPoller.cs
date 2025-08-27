using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueClientPoller : MonoBehaviour
{
    CluesManager cluesManager;
    const float REQUEST_INTERVAL = 4f;
    public event Action<List<ClueSharedInfo>> OnSharedUpdated;
    
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
                    List<ClueSharedInfo> sharedClues = new List<ClueSharedInfo>();
                    for (int i = 0; i < res.shared_clues.Length; i++) 
                    {
                        if (res.shared_clues[i].is_shared != cluesManager.MatchClues.isShared[i])
                        {
                            //���L���̍X�V���������肪����������X�g�ɒǉ�
                            sharedClues.Add(res.shared_clues[i]);
                            Debug.Log(res.shared_clues[i]);
                        }
                    }
                    if (sharedClues.Count > 0)
                    {
                        OnSharedUpdated?.Invoke(sharedClues);
                    }
                },
                onError: (err) =>
                {
                    Debug.Log("���L�擾���s");
                }));
            yield return new WaitForSeconds(REQUEST_INTERVAL);
        }
    }
}
