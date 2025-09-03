using System.Collections.Generic;
using System.Linq;
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

	[SerializeField, Header("����������z��")] string[] answerArray = new string[MAXCODES];
	[SerializeField, Header("�Í�������z��")] Sprite[] cipherArray= new Sprite[MAXCODES];


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
