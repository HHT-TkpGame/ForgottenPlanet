using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class RoleSetter : MonoBehaviour
{
    const string BASE_URI = "https://hht-game.fee-on.com/SynchronizationTest";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator SendRole(string uri)
    {
        string json = JsonUtility.ToJson("ssss");
        UnityWebRequest request = new UnityWebRequest(BASE_URI,"POST");
        yield return null;
    }    
}
