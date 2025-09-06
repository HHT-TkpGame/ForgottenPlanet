using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CluesTextSetter :MonoBehaviour
{
    TextAsset clueFile;
    [SerializeField]TMP_Text[] infoTexts=new TMP_Text[5];

	ClueContentDataList LoadTextFile(int truthId,int[] clueIds)
    {
        clueFile = Resources.Load<TextAsset>("truth"+truthId );
        ClueContentDataList clueList = JsonUtility.FromJson<ClueContentDataList>(clueFile.text);
        return clueList;
    }

    public void SetText(int truthId, int[] clueIds)
    {

		ClueContentDataList contentList =LoadTextFile(truthId, clueIds);
        
		for (int i = 0; i < clueIds.Length; i++)
		{
            int index = clueIds[i]-1;
            if (index >= 0 && index < contentList.clues.Length)
            {
				infoTexts[i].text = contentList.clues[index].content;
			}
            else
            {
				Debug.LogWarning($"clueIds[{i}] ‚Ì’l {clueIds[i]} ‚Í”ÍˆÍŠO‚Å‚·");
			}
		}

	}
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [System.Serializable]
    class ClueContentData
    {
        public string content;
    }
    [System.Serializable]
    class ClueContentDataList
    {
        public ClueContentData[] clues;
    }
}
