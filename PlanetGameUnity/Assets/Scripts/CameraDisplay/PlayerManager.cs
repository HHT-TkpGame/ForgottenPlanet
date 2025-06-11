using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerManager : MonoBehaviour
{
    I_PlayerDefaultFunctions i_function;

    [SerializeField,Header("どっちのプレイヤーを使うか")]bool isCommander;

    [SerializeField, Header("コマンダー")] Commander commander;
	[SerializeField, Header("エージェント")] Agent agent;

	PlayerInput input;

	InputAction moveAct;
	InputAction lookAct;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
		input = GetComponent<PlayerInput>();
		moveAct = input.actions["Move"];
		lookAct = input.actions["Look"];

		if (isCommander)
        {
            i_function = commander;
        }
        else
        {
			i_function = agent;
		}
        i_function.Init();
    }

    // Update is called once per frame
    void Update()
    {
        i_function.Move(moveAct.ReadValue<Vector2>());
        i_function.Look(lookAct.ReadValue<Vector2>());
    }
}
