using System;
using System.Collections;
using UnityEngine;

public interface IRoleSelect
{
    IEnumerator GetSelection(Action<RoleDataList> onSuccess = null, Action<string> onError = null);
    IEnumerator PostRole(RoleData data, Action onSuccess = null, Action<string> onError = null);
    IEnumerator GetHasConflict(Action<bool> onSuccess = null, Action<string> onError = null);
    IEnumerator PostReselection(Action onSuccess = null, Action<string> onError = null);
}
