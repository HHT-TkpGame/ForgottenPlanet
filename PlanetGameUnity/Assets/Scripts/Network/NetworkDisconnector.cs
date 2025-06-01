using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkDisconnector : MonoBehaviour
{
    const string BASE_URI = "https://hht-game.fee-on.com/SynchronizationTest";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    public void OnClick()
    {
        StartCoroutine(Disconnect(BASE_URI + "/api/room/"+MatchingManager.RoomId));
    }

    IEnumerator Disconnect(string uri)
    {
        UnityWebRequest request = new UnityWebRequest(uri,"DELETE");
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();
        if(request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log(request.downloadHandler.text);
            SceneChangeManager.SceneChange("OpeningScene");
        }
        else
        {
            Debug.Log(request.error);
        }
    }
}
