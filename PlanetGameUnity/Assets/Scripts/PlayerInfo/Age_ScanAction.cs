using UnityEngine;

public class Age_ScanAction : MonoBehaviour, I_SearchAction
{
	//Agentの中身を移行

	[SerializeField, Header("スキャン用のコライダー")] GameObject scanColObj;
	////分けて別のものにしたほうがいいかも
	////onScanStartedのとこでSetActiveするものとScanでSetActiveするものは
	////扇形が真上で待機する体制になってしまう
	[SerializeField, Header("スキャン用のモデル")] GameObject scanModel;


	ScanColliderBehavior scanColliderBehavior;
	BoxCollider scanBoxCollider;

	const float MAX_HOLDTIME = 2f;
	float holdTime = 0f;
	bool actionExecuted = false;
	const int SCAN_ROT_RATIO = -90;

	public void SetUp(Transform camera)
	{
		scanColObj.transform.SetParent(camera);
		scanColObj.transform.localPosition = Vector3.zero;
		scanColObj.transform.eulerAngles = Vector3.zero;

		scanColliderBehavior = scanColObj.GetComponent<ScanColliderBehavior>();
		scanColliderBehavior.GetScanAction(this);

		scanBoxCollider = scanColObj.GetComponent<BoxCollider>();

		scanBoxCollider.enabled = false;

		ResetHold();

		scanModel.SetActive(false);
	}

	public void OnSearchStarted()
	{
		Debug.Log("Agent側でSearchの処理が始まった");
		scanBoxCollider.enabled = true;
		scanModel.SetActive(true);
	}

	public void OnSearchCanceled()
	{
		Debug.Log("Agent側でSearchの処理が終わった");
		scanBoxCollider.enabled = false;
		scanModel.SetActive(false);
		ResetHold();
	}

	/// <summary>
	/// サーチアクションが始まったらColliderが出現
	/// ColliderのOnTriggerStayからScanメソッドを呼び
	/// ExecuteActionでそのObjectがClueを持っているなら値を取得
	/// </summary>
	/// <param name="scanObj"></param>
	public void Scan(GameObject scanObj)
	{
		holdTime += Time.deltaTime;
		float scanRot = holdTime * SCAN_ROT_RATIO;
		//一定時間Scanメソッドが呼び出されたら
		//actionExecutedはこの後すぐもう一回下のメソッドを呼ぶの防止
		if (holdTime >= MAX_HOLDTIME && !actionExecuted)
		{
			ExecuteAction(scanObj);
			actionExecuted = true;
		}
		if (scanRot > SCAN_ROT_RATIO * 2)
		{
			scanModel.transform.localEulerAngles = new Vector3(scanRot, 180, -45);
		}
	}

	void ExecuteAction(GameObject target)
	{

		//target.GetComponent<ClueBehavior>
		ClueBehavior clue = target.GetComponent<ClueBehavior>();
		//Bool値を見てTrueなら
		if (clue.HasClue)
		{
			//手がかりの番号を手に入れる
			int clueNum = clue.ClueNum;
			//送るなどする
			StartCoroutine(CluesDataGetter.Instance.ClueClient.PostClue(
				clueNum,
				onSuccess: () =>
				{
					Debug.Log($"Success: 手がかり番号{clueNum}");
				},
				onError: (err) =>
				{
                    Debug.Log($"Failure: 手がかり番号{clueNum}");
                })
			);
		}
		else
		{
			//falseならDebugする
			Debug.Log("手がかりはなかったようだ....");
		}
		//targetのMeshRendererをEnableする一旦
		target.SetActive(false);
	}
	public void ResetHold()
	{
		//スキャンのボタンを押しているが手がかりに当たっていない時の処理

		holdTime = 0f;
		actionExecuted = false;

		//エフェクトなどになったときはそれ用のエフェクトを再生するなど
		scanModel.transform.localEulerAngles = new Vector3(0, 180, -45);
	}
}