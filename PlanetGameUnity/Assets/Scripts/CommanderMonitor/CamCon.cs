using UnityEngine;

public class CamCon : MonoBehaviour
{
    [SerializeField] GameObject camPivot;
    [SerializeField] Camera cam;
    [SerializeField] float sensitivity; // �}�E�X���x
    [SerializeField] float pitchMin;  // �㉺��]�̍ŏ��p�x
    [SerializeField] float pitchMax;   // �㉺��]�̍ő�p�x
    float pitch; // ���݂̏㉺�p�x
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

        // ������]�iY���j: �J�����̐e����]
        camPivot.transform.Rotate(Vector3.up, mouseX, Space.World);

        // ������]�iX���j: �㉺�̊p�x�ɐ�����������
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);
        cam.transform.localEulerAngles = new Vector3(pitch, 0f, 0f);
    }
}
