using UnityEngine;

public class RoleSelectManager : MonoBehaviour
{
    [SerializeField] RoleUIManager roleUIManager;
    [SerializeField] RoleSelectPoller poller;
    RoleSelect roleSelect;

    public bool HasConflict { get; set; }
    public bool IsReselection { get; private set; }
    public bool IsHostButtonLocked { get; set; }
    public bool IsGuestButtonLocked { get; set; }

    void Awake()
    {
        roleSelect = new RoleSelect();
        poller.Initialize(roleSelect);
        roleUIManager.Initialized(this, roleSelect);
        poller.OnSelectionUpdated += roleUIManager.UpdateUI;
    }
    private void Start()
    {
        StartCoroutine(poller.PollLoop());
    }
}
