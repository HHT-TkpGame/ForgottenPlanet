using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkDisconnector : MonoBehaviour
{
    public void OnClick()
    {
        StartCoroutine(Disconnect(ApiConfig.BASE_URI + "/api/room/"+MatchingManager.RoomId));
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
