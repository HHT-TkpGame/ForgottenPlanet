using System;
using System.Collections;
using UnityEngine;

public class ClueSync : IClueSync
{
    public IEnumerator PostClue(Action onSuccess, Action onError)
    {
        yield return null;
    }
    public IEnumerator GetClue(Action<ClueData> onSuccess, Action onError)
    {
        yield return null;
    }
    public IEnumerator PostTruth(Action onSuccess, Action onError)
    {
        yield return null;
    }
    public IEnumerator GetTruth(Action<Truth>onSuccess, Action onError)
    {
        yield return null;
    }
}
