using UnityEngine;

public interface I_BodyCamTrans
{
	void Init();
	void SetCameraTransform(Vector3 cameraPos,float cameraRot_Y);
}
