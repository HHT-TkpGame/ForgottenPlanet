using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class TransformSender : MonoBehaviour
{
    [SerializeField] GameObject target;
    const string BASE_URI = "https://hht-game.fee-on.com/SynchronizationTest";
    // Start is called before the first frame update
    void Start()
    {
        if (MatchingManager.IsCommander) { return; }
        StartCoroutine(SendTransformLoop());
    }
    const float SPEED = 3f;
    float speed = SPEED;
    void Update()
    {
        if(MatchingManager.IsCommander) { return; }
        if(target.transform.position.x > 5)
        {
            speed = -SPEED;
        }
        else if(target.transform.position.x < -5)
        {
            speed = SPEED;
        }
        target.transform.Translate(speed * Time.deltaTime,0,0);
    }

    const float REQUEST_INTERVAL = 0.5f;
    IEnumerator SendTransformLoop()
    {
        while (true)
        {
            yield return StartCoroutine(SendTransform(BASE_URI + "/api/room/" + 
                MatchingManager.RoomId + "/position", target.transform.position));

            yield return new WaitForSeconds(REQUEST_INTERVAL);
        }
    }
    IEnumerator SendTransform(string uri, Vector3 pos)
    {
        Debug.Log(uri);
        string json = JsonUtility.ToJson(new PlayerTransform(PlayerIdManager.Id, pos.x, pos.y, pos.z));
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
