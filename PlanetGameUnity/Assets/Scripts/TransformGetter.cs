using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class TransformGetter : MonoBehaviour
{
    [SerializeField] GameObject target;
    const string BASE_URI = "https://hht-game.fee-on.com/SynchronizationTest";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!MatchingManager.IsCommander) { return; }
        StartCoroutine(GetTransformLoop());
    }

    // Update is called once per frame
    void Update()
    {
        if (!MatchingManager.IsCommander) { return; }
        target.transform.position = targetPos;
        target.transform.eulerAngles = new Vector3(0, rotY, 0);
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
    Vector3 targetPos;
    float rotY;
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
            Debug.Log(json.x + json.y + json.z);
            targetPos = new Vector3(json.x, json.y, json.z);
            rotY = json.rot_y;
        }
        else if (request.responseCode == 404)
        {
            Debug.Log("�ʐM���ؒf����܂���");
            NetworkStateManager.SetState(NetworkStateManager.NetworkState.Disconnected);
        }
        else
        {
            Debug.Log(request.error);
        }
    }
}
