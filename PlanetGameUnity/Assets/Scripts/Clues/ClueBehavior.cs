using UnityEngine;

public class ClueBehavior : MonoBehaviour
{
    [SerializeField, Header("�ǂ̕����ɂ���I�u�W�F�N�g��")] int roomNum;
    
    public int RoomNum => roomNum;
    bool hasClue;
    public bool HasClue=> hasClue;
    int clueNum;
    public int ClueNum => clueNum;

    public void GetClue(int clueNum)
    {
		//�肪����������Ă��邩�ǂ����ǂ̎肪����������Ă���
		hasClue = true;
        this.clueNum = clueNum;

        //Debug.Log("�����̃Q�[���I�u�W�F�N�g�̖��O"+name+"�����̃��[���ԍ�:"+roomNum+"�����̎肪����̔ԍ�:"+this.clueNum);
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
