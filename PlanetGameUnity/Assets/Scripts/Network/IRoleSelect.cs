using System;
using System.Collections;
using UnityEngine;

public interface IRoleSelect
{
    IEnumerator GetSelections(Action<SelectionDataList> onSuccess = null, Action<string> onError = null);
    IEnumerator PostSelection(SelectionData data, Action onSuccess = null, Action<string> onError = null);
    IEnumerator PostRole(SelectionData data, Action onSuccess = null, Action<string> onError = null);
    IEnumerator GetHasConflict(Action<bool> onSuccess = null, Action<string> onError = null);
    IEnumerator PostReselection(Action onSuccess = null, Action<string> onError = null);
}
