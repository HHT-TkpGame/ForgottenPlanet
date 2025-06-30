using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class RoleSelect : MonoBehaviour
{
    const string BASE_URI = "https://hht-game.fee-on.com/SynchronizationTest";
    
    //IEnumerator Check
    IEnumerator SendRole(string uri)
    {
        string json = JsonUtility.ToJson("ssss");
        UnityWebRequest request = new UnityWebRequest(BASE_URI,"POST");
        yield return null;
    }    
}
