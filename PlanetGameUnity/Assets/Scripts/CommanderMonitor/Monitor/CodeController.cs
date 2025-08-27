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

	[SerializeField, Header("暗号を入れる配列")] Sprite[] cipherArray= new Sprite[MAXCODES];
	[SerializeField, Header("答えを入れる配列")] string[] answerArray = new string[MAXCODES];

	int[] cipherNumArray=new int[MAXCLUES];

	public (Sprite,string) SetClueCipher(int count)
	{
		bool numTaken=false;
		int rand;
		//ランダムをとりあえず一回は確認するためにdoWhile
		do
		{
			rand = Random.Range(0, cipherArray.Length);
			foreach(var cipherNum in cipherNumArray)
			{
				if (rand == cipherNum)
				{
					numTaken = true;
				}
			}
		}
		while (numTaken);

		cipherNumArray[count] = rand;
		return (cipherArray[rand],answerArray[rand]);
	}
}
