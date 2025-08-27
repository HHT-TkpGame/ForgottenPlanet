using System;
using System.Collections;
using UnityEngine;

public interface IClueSync
{
    IEnumerator GetClue(Action<ClueSharedInfoList> onSuccess, Action<string> onError);
    IEnumerator PostClue(int clueId, Action onSuccess, Action<string> onError);
    IEnumerator GetClueAndTruth(Action<ServerCurrentMatchClues> onSuccess, Action<string> onError);
}
