using UnityEngine;

public class ClueBehavior : MonoBehaviour
{
    [SerializeField, Header("どの部屋にいるオブジェクトか")] int roomNum;
    
    public int RoomNum => roomNum;
    bool hasClue;
    public bool HasClue=> hasClue;
    int clueNum;
    public int ClueNum => clueNum;

    public void GetClue(int clueNum)
    {
		//手がかりを持っているかどうかどの手がかりを持っている
		hasClue = true;
        this.clueNum = clueNum;

        //Debug.Log("自分のゲームオブジェクトの名前"+name+"自分のルーム番号:"+roomNum+"自分の手がかりの番号:"+this.clueNum);
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
