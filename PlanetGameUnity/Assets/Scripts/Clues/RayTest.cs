using UnityEngine;
using UnityEngine.InputSystem;

public class RayTest : MonoBehaviour
{
	//[SerializeField, Header("�J����")] GameObject cameraObj;
	//const float MAX_HOLDTIME = 2f;
    //float holdTime = 0f;
    //bool actionExecuted=false;

    //�X�L�����p��Ray�̍ő勗��(�Z��)
    //const float RAYMAXLENGTH = 2f;

    //Ray ray;

    //PlayerInput input;
    //InputAction scanAction;
    //InputSystem_Actions inputActions;

    //bool isScanning=false;

    //BoxCollider boxCol;

    PlayerController controller;


    public void GetPlayerController(PlayerController playerController)
    {
        controller = playerController;
    }

	private void OnTriggerStay(Collider other)
	{
        Debug.Log("other" + other.name);

        if (other.CompareTag("Clue"))
        {
            controller.Scan(other.gameObject);
        }
        else
        {
            controller.ResetHold();
        }
	}
	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Clue"))
		{
			controller.ResetHold();
		}

	}
}


