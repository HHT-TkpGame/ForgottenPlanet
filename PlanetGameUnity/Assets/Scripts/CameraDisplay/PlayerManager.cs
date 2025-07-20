using System.Buffers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerManager : MonoBehaviour
{
    I_PlayerDefaultFunctions i_function;
    I_BodyCamTrans i_camTrans;
    ITransformGetterStrategy iTransformGetter;
    [SerializeField] TransformGetter transformGetter;

    [SerializeField,Header("�ǂ����̃v���C���[���g����")]bool isCommander;

    [SerializeField, Header("�R�}���_�[")] Commander commander;
	[SerializeField, Header("�G�[�W�F���g")] Agent agent;
    [SerializeField, Header("�G�[�W�F���g�̃{�f�B�J����")] AgentBodyCamera bodyCamera;


	
    PlayerInput input;

	InputAction moveAct;
	InputAction lookAct;
    InputAction scanAct;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
		input = GetComponent<PlayerInput>();
		moveAct = input.actions["Move"];
		lookAct = input.actions["Look"];
		//scanAct = input.actions["Search"];

		//scanAct.started += OnScanStarted;
		//scanAct.canceled += OnScanCanceled;

        if(isCommander)
		//if (MatchingManager.IsCommander)
        {
            i_function = commander;
            i_camTrans = bodyCamera;
            iTransformGetter = transformGetter;
        }
        else
        {
			i_function = agent;
		}

        i_function.Init();
        i_function.SetStartPos();
		i_camTrans?.Init();
	}

    //void OnScanStarted(InputAction.CallbackContext context)
    //{
    //	if (MatchingManager.IsCommander) { return; }
    //       agent.OnScanStarted();

    //}
    //void OnScanCanceled(InputAction.CallbackContext context)
    //{
    //    if (MatchingManager.IsCommander) { return; }
    //    agent.OnScanCanceled();
    //}
    // Update is called once per frame
    void Update()
    {
        i_function.Move(moveAct.ReadValue<Vector2>());
        i_function.Look(lookAct.ReadValue<Vector2>());
        
        //�����̓T�[�o�[�֌W
        //��������Vector3��Position��������Float��Rotation
        i_camTrans?.SetCameraTransform(iTransformGetter.Pos, iTransformGetter.RotY);
    }
}
