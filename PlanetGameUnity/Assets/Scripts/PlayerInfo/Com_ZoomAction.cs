using UnityEngine;

public class Com_ZoomAction : MonoBehaviour,I_SearchAction
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public void OnSearchStarted()
    {
        Debug.Log("Commander����Search�̏������n�܂���");
    }
	public void OnSearchCanceled()
    {
		Debug.Log("Commander����Search�̏������I�����");
	}
}
