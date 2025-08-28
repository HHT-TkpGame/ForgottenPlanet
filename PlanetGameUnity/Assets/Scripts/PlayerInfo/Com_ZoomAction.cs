using UnityEngine;

public class Com_ZoomAction : MonoBehaviour,I_SearchAction
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField,Header("モニター拡大時にUIを操作するクラス")] MonitorUIController monitorUIController;
    
    Commander commander;

	int monitorNum;

	public void SetUp(Commander commander)
    {
        this.commander = commander;
        monitorUIController.SetUp(this);
    } 

    public void OnSearchStarted()
    {
        Debug.Log("Commander側でSearchの処理が始まった");
    }
	public void OnSearchCanceled()
    {
		Debug.Log("Commander側でSearchの処理が終わった");
	}
}
