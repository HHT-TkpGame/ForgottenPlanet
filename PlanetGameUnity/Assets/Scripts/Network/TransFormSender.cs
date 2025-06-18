using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class TransformSender : MonoBehaviour, ITransformStrategy.ITransformSenderStrategy
{
    [SerializeField] GameObject target;
    const string BASE_URI = "https://hht-game.fee-on.com/SynchronizationTest";
    
    public void Initialize()
    {
        StartCoroutine(SendTransformLoop());
    }

    const float REQUEST_INTERVAL = 0.3f;
    IEnumerator SendTransformLoop()
    {
        while (NetworkStateManager.CurrentState == NetworkStateManager.NetworkState.Connected)
        {
            yield return StartCoroutine(SendTransform(BASE_URI + "/api/room/" + 
                MatchingManager.RoomId + "/position", target.transform.position, target.transform.eulerAngles.y));

            yield return new WaitForSeconds(REQUEST_INTERVAL);
        }
    }
    IEnumerator SendTransform(string uri, Vector3 pos, float rotY)
    {
        Debug.Log(uri);
        string json = JsonUtility.ToJson(new PlayerTransform(PlayerIdManager.Id, pos.x, pos.y, pos.z, rotY));
        byte[] rawData = Encoding.UTF8.GetBytes(json);
        UnityWebRequest request = new UnityWebRequest(uri, "POST");
        request.uploadHandler = new UploadHandlerRaw(rawData);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log(request.downloadHandler.text);
        }
        else
        {
            Debug.Log(request.error);
        }
    }
}
