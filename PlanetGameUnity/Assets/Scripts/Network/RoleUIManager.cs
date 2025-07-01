using UnityEngine;

public class RoleUIManager : MonoBehaviour
{
    IRoleSelect roleSelect;
    public void Initialized(IRoleSelect roleSelect)
    {
        this.roleSelect = roleSelect;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
