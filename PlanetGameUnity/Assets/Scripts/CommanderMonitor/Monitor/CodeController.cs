using UnityEngine;
using UnityEngine.UI;


public class CodeController : MonoBehaviour
{
	const int MAXCODES = 10;
	const int MAXCLUES = 5;
	///<summary>
	///���ɓ����̂�10�����炢�̈Í��̃C���[�W�̔z�� 
	///��������MonitorController���烁�\�b�h���Ă�ňÍ������Ԗڂɂ��邩�����߂�
	///�������̂͏o�Ȃ��悤��
	/// </summary>

	[SerializeField, Header("�Í�������z��")] Sprite[] cipherArray= new Sprite[MAXCODES];
	[SerializeField, Header("����������z��")] string[] answerArray = new string[MAXCODES];

	int[] cipherNumArray=new int[MAXCLUES];

	int i=-1;
	public (Sprite,string) SetClueCipher(int count)
	{
		bool numTaken=false;
		int rand;
		//�����_�����Ƃ肠�������͊m�F���邽�߂�doWhile


		//cipherNumArray[count] = i;
		i++;
		Debug.Log(i);
		return (cipherArray[i], answerArray[i]);

		//do
		//{
		//	rand = Random.Range(0, cipherArray.Length);
		//	foreach(var cipherNum in cipherNumArray)
		//	{
		//		if (rand == cipherNum)
		//		{
		//			numTaken = true;
		//		}
		//	}
		//}
		//while (numTaken);


	}
}
