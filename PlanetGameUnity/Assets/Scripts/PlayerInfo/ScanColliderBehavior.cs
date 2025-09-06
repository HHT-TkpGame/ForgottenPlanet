using UnityEngine;
using UnityEngine.InputSystem.XR;

public class ScanColliderBehavior : MonoBehaviour
{
	Age_ScanAction scanAct;


	public void GetScanAction(Age_ScanAction scanAct)
	{
		this.scanAct = scanAct;
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.CompareTag("Clue"))
		{
			scanAct.Scan(other.gameObject);
		}
		else
		{
			//別オブジェクトと接触していたらスキャンがリセットされてしまうのでコメントアウト
			//影響あれば戻す
			//scanAct.ResetHold();
		}
	}
	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Clue"))
		{
			scanAct.ResetHold();
		}
	}
}
