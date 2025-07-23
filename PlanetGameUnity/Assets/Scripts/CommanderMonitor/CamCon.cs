using UnityEngine;

public class CamCon : MonoBehaviour
{
    [SerializeField] GameObject camPivot;
    [SerializeField] Camera cam;
    [SerializeField] float sensitivity; // マウス感度
    [SerializeField] float pitchMin;  // 上下回転の最小角度
    [SerializeField] float pitchMax;   // 上下回転の最大角度
    float pitch; // 現在の上下角度
    float rayDistance = 5f;

    void Update()
    {
        Look();

        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = new Ray(cam.transform.position, cam.transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
            {
                CommanderMonitor monitor = hit.collider.GetComponent<CommanderMonitor>();
                if (monitor != null)
                {
                    //monitor.Zoom();
                }
            }
        }
    }
    void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        // 水平回転（Y軸）: カメラの親を回転
        camPivot.transform.Rotate(Vector3.up, mouseX, Space.World);

        // 垂直回転（X軸）: 上下の角度に制限をかける
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);
        cam.transform.localEulerAngles = new Vector3(pitch, 0f, 0f);
    }
}
