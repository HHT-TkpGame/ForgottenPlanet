using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [SerializeField, Header("カメラ")] GameObject cameraObj;
    
    PlayerInput input;
    InputAction moveAct;
    CharacterController characterController;
    Vector2 moveAxis;
    Vector2 lookAxis;
	Vector3 rot;
    Vector3 currentRot;

	const int MOVESPEED = 5;
    const int GRAVITY = 200;

    const int ROTSPEED = 60;

    float verticalVelocity=0;

    bool finSetUp;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        if (!MatchingManager.IsCommander) { return; }
        input = GetComponent<PlayerInput>();
        characterController = GetComponent<CharacterController>();
        currentRot = transform.eulerAngles;
        finSetUp = true;
    }

    /// <summary>
    /// InputAction側から呼ばれる
    /// </summary>
    /// <param name="context"></param>
    public void OnMove(InputAction.CallbackContext context)
    {
        if (!MatchingManager.IsCommander) { return; }
        if (finSetUp)
        {
            //Debug.Log("ddd");
            //moveAxis=context.
            moveAxis = context.ReadValue<Vector2>();
        }
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        if (!MatchingManager.IsCommander) { return; }
        if (finSetUp)
        {
            lookAxis = context.ReadValue<Vector2>();
            //Debug.Log(lookAxis);
            Look();
        }
	}
    // Update is called once per frame
    void Update()
    {
        if (!MatchingManager.IsCommander) { return; }
        Move();
    }




    void Move()
    {
        if (!characterController.isGrounded)
        {
            verticalVelocity = -GRAVITY * Time.deltaTime;
        }

		Vector3 moveDir = new Vector3(moveAxis.x * MOVESPEED, verticalVelocity, moveAxis.y * MOVESPEED);
        moveDir=transform.TransformDirection(moveDir);
		characterController.Move(moveDir * Time.deltaTime);
	}
    void Look()
    {
        if (!MatchingManager.IsCommander) { return; }
        //デッドゾーンを作る

        float rot_Y=lookAxis.x;
        float rot_X=lookAxis.y;

        rot = new Vector3(rot_X * ROTSPEED, rot_Y * ROTSPEED,0 );

        rot*=Time.deltaTime;

        float angle_X=MathF.Abs( transform.eulerAngles.x-currentRot.x);

        Debug.Log(angle_X);
        //if (angle_X > 90)
        {
            transform.eulerAngles += new Vector3(0, rot.y, 0);
            cameraObj.transform.eulerAngles += new Vector3(rot.x,0,0);
        }

	}
}
