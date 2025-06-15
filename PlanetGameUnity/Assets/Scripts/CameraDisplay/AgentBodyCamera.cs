using UnityEngine;

public class AgentBodyCamera : MonoBehaviour,I_BodyCamTrans
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Init()
    {
        //Ç±Ç±è´óàÇÕAgentÇÃç¿ïWÇ…Ç∑ÇÈ
        gameObject.transform.position=Vector3.zero;
    }
    public void SetCameraTransform(Vector3 cameraPos, float cameraRot_Y)
    {
        transform.position = cameraPos;
        transform.eulerAngles = new Vector3(0, cameraRot_Y, 0);
    }
}
