using System;
using System.Collections;
using UnityEngine;

public interface IClueSync
{
    IEnumerator PostClue(Action onSuccess, Action onError);
    IEnumerator GetClue(Action<ClueData> onSuccess, Action onError);
    IEnumerator PostTruth(Action onSuccess, Action onError);
    IEnumerator GetTruth(Action<Truth> onSuccess, Action onError);
}
