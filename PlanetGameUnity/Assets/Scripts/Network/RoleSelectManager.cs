using UnityEngine;

public class RoleSelectManager : MonoBehaviour
{
    [SerializeField] RoleUIManager roleUIManager;
    [SerializeField] RoleSelectPoller poller;
    RoleSelect roleSelect;
    
    void Start()
    {
        roleSelect = new RoleSelect();
        poller.Initialize(roleSelect);
        StartCoroutine(poller.PollLoop());
        roleUIManager.Initialized(roleSelect);
    }
}
