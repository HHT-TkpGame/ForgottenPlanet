using UnityEngine;

public class NetworkStateManager : MonoBehaviour
{
    public enum NetworkState
    {
        Connected,
        Disconnected,
    }
    public static NetworkState CurrentState {  get; private set; }
    public static void SetState(NetworkState newState)
    {
        CurrentState = newState;
    }
}
