using UnityEngine;

public class ScanColliderBehavior : MonoBehaviour
{
	Agent agent;


	public void GetPlayerController(Agent agent)
	{
		this.agent = agent;
	}

	private void OnTriggerStay(Collider other)
	{
		//if (other.CompareTag("Clue"))
		//{
		//	agent.Scan(other.gameObject);
		//}
		//else
		//{
		//	agent.ResetHold();
		//}
	}
}
