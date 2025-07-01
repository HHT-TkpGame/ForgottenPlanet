using System.Collections;
using UnityEngine;

public interface IRoleSelect
{
    IEnumerator GetSelection();
    IEnumerator PostRole(RoleData data);
    IEnumerator GetHasConflict();
    IEnumerator PostReselection();
    public bool HasConflict { get; }
    public bool IsReselection { get; }
    public bool IsHostButtonLocked { get; }
    public bool IsGuestButtonLocked { get; }
}
