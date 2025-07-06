using System;
using System.Collections;
using UnityEngine;

public class RoleSelectPoller : MonoBehaviour
{
    [SerializeField] float interval = 1.0f;
    IRoleSelect roleSelect;
    RoleUIManager roleUI;
    //イベント定義
    public event Action<SelectionDataList> OnSelectionUpdated;


    public void Initialize(IRoleSelect roleSelect, RoleUIManager roleUI)
    {
        this.roleSelect = roleSelect;
        this.roleUI = roleUI;
    }

    /// <summary>
    /// カーソル上の役職データを定期的に送信
    /// </summary>
    /// <returns></returns>
    public IEnumerator PollLoop()
    {
        while (true)
        {
            yield return StartCoroutine(roleSelect.PostRole(new SelectionData(PlayerIdManager.Id, roleUI.IsCommander), onSuccess: () =>
            {
                Debug.Log("Success");
            },
            onError: (err) =>
            {
                Debug.Log(err);
            }
            ));
            yield return StartCoroutine(roleSelect.GetSelections(onSuccess: (dataList) =>
                {
                    Debug.Log(dataList.selections);
                    OnSelectionUpdated?.Invoke(dataList);
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
