using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [SerializeField, Header("ÉJÉÅÉâ")] GameObject cameraObj;

    PlayerInput input;

    InputAction moveAct;
    InputAction lookAct;

    CharacterController characterController;

    Vector2 moveAxis;
    Vector2 lookAxis;
	
    Vector3 rot;
    Vector3 currentRot;

	const int MOVESPEED = 5;
    const int GRAVITY = 200;

    const int ROTSPEED = 90;

    float verticalVelocity=0;

    bool finSetUp;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        //if (!MatchingManager.IsCommander) { return; }

        //Updateì‡Ç≈ìÆÇ≠InputÇÃìoò^
        input = GetComponent<PlayerInput>();
        moveAct = input.actions["Move"];
        lookAct = input.actions["Look"];

        characterController = GetComponent<CharacterController>();
        currentRot = transform.eulerAngles;

        cameraObj.transform.eulerAngles=Vector3.zero;

        finSetUp = true;
    }

    // Update is called once per frame
    void Update()
    {
        //if (!MatchingManager.IsCommander) { return; }
        Move();
        Look();
    }

    void Move()
    {
        moveAxis=moveAct.ReadValue<Vector2>();

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
        //Debug.Log(cameraObj.transform.eulerAngles);
        //if (!MatchingManager.IsCommander) { return; }

        lookAxis= lookAct.ReadValue<Vector2>();

        float rot_Y=lookAxis.x;
        float rot_X=lookAxis.y;

        rot = new Vector3(rot_X * ROTSPEED, rot_Y * ROTSPEED,0 );

        rot*=Time.deltaTime;


		//ç∂âEÇÕÇªÇÃÇ‹Ç‹è„â∫ÇÕãtÇ…ÇµÇƒégÇ¢ÇΩÇ¢ÇÃÇ≈cameraÇÃâÒì]ÇÕÉ}ÉCÉiÉXÇä|ÇØÇÈ
		transform.eulerAngles += new Vector3(0, rot.y, 0);
		cameraObj.transform.eulerAngles += new Vector3(-rot.x, 0, 0);

		if (cameraObj.transform.eulerAngles.x > 45 &&cameraObj.transform.eulerAngles.x<360-45)
        {
			cameraObj.transform.eulerAngles += new Vector3(rot.x, 0, 0);
		}

	}
}
