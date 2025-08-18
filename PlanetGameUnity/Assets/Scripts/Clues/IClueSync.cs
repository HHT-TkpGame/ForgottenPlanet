using System;
using System.Collections;
using UnityEngine;

public interface IClueSync
{
    IEnumerator GetClue(Action<ClueData> onSuccess, Action<string> onError);
    IEnumerator PostClue(Action onSuccess, Action<string> onError);
    IEnumerator GetClueAndTruth(Action<ClueData> onSuccess, Action<string> onError);
}
