using System.Buffers;
using System.Security;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerManager : MonoBehaviour
{
    I_PlayerDefaultFunctions i_function;
    I_BodyCamTrans i_camTrans;
    ITransformGetterStrategy iTransformGetter;
    I_SearchAction iSearchAction;
    [SerializeField] TransformGetter transformGetter;

    [SerializeField,Header("どっちのプレイヤーを使うか")]bool isCommander;

    [SerializeField, Header("コマンダー")] Commander commander;
	[SerializeField, Header("エージェント")] Agent agent;
    [SerializeField, Header("エージェントのボディカメラ")] AgentBodyCamera bodyCamera;

    //SearchAction用のSerializeField
    //[SerializeField,Header("両プレイヤーの探す系の行動")]SearchActionController searchController;
	[SerializeField, Header("コマンダーの探す系の行動")] Com_ZoomAction zoomAction;
    [SerializeField, Header("エージェントの探す系の行動")] Age_ScanAction scanAction;

    SearchActionController searchController;

	PlayerInput input;

	InputAction moveAct;
	InputAction lookAct;
    InputAction searchAct;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
		input = GetComponent<PlayerInput>();
		moveAct = input.actions["Move"];
		lookAct = input.actions["Look"];
        searchAct = input.actions["Search"];


        if (isCommander)
		//if (MatchingManager.IsCommander)
        {
            i_function = commander;
            i_camTrans = bodyCamera;
            iTransformGetter = transformGetter;
            iSearchAction = zoomAction;
        }
        else
        {
			i_function = agent;
            iSearchAction = scanAction;
		}

		i_function.Init();
        i_function.SetStartPos();
		i_camTrans?.Init();

        searchController = new SearchActionController();
        searchController.Init(iSearchAction);

		searchAct.started += OnSearchStarted;
		searchAct.canceled += OnSearchCanceled;
	}

    void OnSearchStarted(InputAction.CallbackContext context)
    {
		searchController.OnSearchStarted();
        //iSearchAction.OnSearchStarted();
	}
	void OnSearchCanceled(InputAction.CallbackContext context)
    {
		searchController.OnSearchCanceled();
        //iSearchAction.OnSearchCanceled();
	}
	// Update is called once per frame
	void Update()
    {
        i_function.Move(moveAct.ReadValue<Vector2>());
        i_function.Look(lookAct.ReadValue<Vector2>());
        
        //引数はサーバー関係
        //第一引数はVector3のPosition第二引数はFloatのRotation
        //i_camTrans?.SetCameraTransform(iTransformGetter.Pos, iTransformGetter.RotY);
    }
}
