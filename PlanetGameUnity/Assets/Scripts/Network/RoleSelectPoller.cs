using System;
using System.Collections;
using UnityEngine;

public class RoleSelectPoller : MonoBehaviour
{
    [SerializeField] float interval = 1.0f;
    IRoleSelect roleSelect;
    //イベント定義
    public event Action<RoleDataList> OnSelectionUpdated;


    public void Initialize(IRoleSelect roleSelect)
    {
        this.roleSelect = roleSelect;
    }

    public IEnumerator PollLoop()
    {
        while (true)
        {
            yield return StartCoroutine(roleSelect.GetSelection(onSuccess: (dataList) =>
                {
                    Debug.Log(dataList.Selections);
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
