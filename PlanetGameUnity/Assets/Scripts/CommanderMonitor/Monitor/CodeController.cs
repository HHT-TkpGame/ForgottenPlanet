using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class CodeController : MonoBehaviour
{
	const int MAXCODES = 10;
	const int MAXCLUES = 5;
	///<summary>
	///下に入れるのは10択くらいの暗号のイメージの配列 
	///そこからMonitorControllerからメソッドを呼んで暗号を何番目にするかを決める
	///同じものは出ないように
	/// </summary>

	[SerializeField, Header("答えを入れる配列")] string[] answerArray = new string[MAXCODES];
	[SerializeField, Header("暗号を入れる配列")] Sprite[] cipherArray= new Sprite[MAXCODES];


	int[] cipherNumArray=new int[MAXCLUES];

	List<int> availableIndices;

	public void SetCodeList()
	{
		availableIndices = Enumerable.Range(0, cipherArray.Length).ToList();
		
	}
	public (Sprite,string) SetClueCipher()
	{
		if (availableIndices.Count == 0)
		{
			return (null,null);
		}

		int rand=Random.Range(0,availableIndices.Count);
		int chosen = availableIndices[rand];
		availableIndices.RemoveAt(rand);

		return (cipherArray[chosen], answerArray[chosen]);
	}
}
