using UnityEngine;

public class AgentBodyCamera : MonoBehaviour,I_BodyCamTrans
{
    [SerializeField, Header("エージェントの初期位置")] Transform startPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Init()
    {
        //ここ将来はAgentの座標にする
        gameObject.transform.position=startPos.position;
    }
    public void SetCameraTransform(Vector3 cameraPos, float cameraRot_Y)
    {
        transform.position = cameraPos;
        Debug.Log(transform.position);
        //transform.position = cameraPos;
        transform.eulerAngles = new Vector3(0, cameraRot_Y, 0);
    }
}
