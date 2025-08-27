using UnityEngine;

public class AgentBodyCamera : MonoBehaviour,I_BodyCamTrans
{
    [SerializeField, Header("�G�[�W�F���g�̏����ʒu")] Transform startPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Init()
    {
        //����������Agent�̍��W�ɂ���
        transform.position=startPos.position;
        transform.rotation=startPos.rotation;
        Debug.Log(transform.position);
    }
    public void SetCameraTransform(Vector3 cameraPos, float cameraRot_Y)
    {
        transform.position = cameraPos;
        transform.eulerAngles = new Vector3(0, cameraRot_Y, 0);
    }
}
