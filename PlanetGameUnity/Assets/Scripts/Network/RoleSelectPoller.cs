using System;
using System.Collections;
using UnityEngine;

public class RoleSelectPoller : MonoBehaviour
{
    [SerializeField] float interval = 1.0f;
    IRoleSelect roleSelect;


    public void Initialize(IRoleSelect roleSelect)
    {
        this.roleSelect = roleSelect;
    }

    public IEnumerator PollLoop()
    {
        while (true)
        {
            yield return StartCoroutine(roleSelect.GetSelection());

            yield return new WaitForSeconds(interval);
        }
    }
}
