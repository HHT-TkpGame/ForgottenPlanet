using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class TransformGetter : MonoBehaviour, ITransformStrategy.ITransformGetterStrategy
{
    public Vector3 Pos { get; private set; }
    public float RotY {  get; private set; }
    [SerializeField] GameObject target;
    const string BASE_URI = "https://hht-game.fee-on.com/SynchronizationTest";
    public void Initialize()
    {
        StartCoroutine(GetTransformLoop());
    }
    
    const float REQUEST_INTERVAL = 0.3f;
    IEnumerator GetTransformLoop()
    {
        while (NetworkStateManager.CurrentState == NetworkStateManager.NetworkState.Connected)
        {
            yield return StartCoroutine(GetTransform(BASE_URI+"/api/room/"+MatchingManager.RoomId + "/position"));
            Debug.Log("GetPos");
            yield return new WaitForSeconds(REQUEST_INTERVAL);
        }
    }
    IEnumerator GetTransform(string uri)
    {
        Debug.Log(uri);
        UnityWebRequest request = new UnityWebRequest(uri, "GET");
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        {
            string res = request.downloadHandler.text;
            PlayerTransform json = JsonUtility.FromJson<PlayerTransform>(res);
            Pos = new Vector3(json.x, json.y, json.z);
            RotY = json.rot_y;
            Debug.Log("pos:" + Pos + " / rotY:" + RotY);
        }
        else
        {
            Debug.Log(request.error);
        }
    }
}
