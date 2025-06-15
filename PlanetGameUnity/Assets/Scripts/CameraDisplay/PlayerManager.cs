using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerManager : MonoBehaviour
{
    I_PlayerDefaultFunctions i_function;
    I_BodyCamTrans i_camTrans;

    [SerializeField,Header("どっちのプレイヤーを使うか")]bool isCommander;

    [SerializeField, Header("コマンダー")] Commander commander;
	[SerializeField, Header("エージェント")] Agent agent;
    [SerializeField, Header("エージェントのボディカメラ")] AgentBodyCamera bodyCamera;

	PlayerInput input;

	InputAction moveAct;
	InputAction lookAct;

    Vector3 StartPos=Vector3.zero;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
		input = GetComponent<PlayerInput>();
		moveAct = input.actions["Move"];
		lookAct = input.actions["Look"];

		if (isCommander)
        {
            i_function = commander;
            i_camTrans = bodyCamera;

        }
        else
        {
			i_function = agent;
		}

        i_function.Init();
        //i_function.SetStartPos(StartPos);
		i_camTrans?.Init();
	}

    float a = 0;
    float b = 0;
    // Update is called once per frame
    void Update()
    {
        i_function.Move(moveAct.ReadValue<Vector2>());
        i_function.Look(lookAct.ReadValue<Vector2>());

        //a += 0.1f;
        b += 0.1f;
        //引数はサーバー関係
        //第一引数はVector3のPosition第二引数はFloatのRotation
        i_camTrans?.SetCameraTransform(new Vector3(0, a, 0), b);
    }
}
