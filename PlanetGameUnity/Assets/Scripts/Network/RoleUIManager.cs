using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoleUIManager : MonoBehaviour
{
    [SerializeField] Button sendButton;
    [SerializeField] Button checkButton;
    [SerializeField] Button reselectButton;
    [SerializeField] Button startButton;

    /////試運転用
    [SerializeField] GameObject selfCursor;
    RectTransform selfCursorRect;
    [SerializeField] GameObject otherCursor;
    RectTransform otherCursorRect;
    [SerializeField] GameObject selfState;
    GameObject selfReadyState;
    [SerializeField] GameObject otherState;
    GameObject otherReadyState;
    Vector3 selfLeftPos = new Vector3(-400, 250, 0);
    Vector3 selfRightPos = new Vector3(0, 250, 0);
    Vector3 otherLeftPos = new Vector3 (-250, 250, 0);
    Vector3 otherRightPos = new Vector3(150, 250, 0);
    /////

    RoleSelectManager roleSelectManager;
    IRoleSelect roleSelect;
    
    public bool IsLocked {  get; private set; }
    public bool IsCommander {  get; private set; }
    /// <summary>
    /// 依存関係設定
    /// </summary>
    /// <param name="roleSelectManager"></param>
    /// <param name="roleSelect"></param>
    public void Initialized(RoleSelectManager roleSelectManager, IRoleSelect roleSelect)
    {
        this.roleSelectManager = roleSelectManager;
        this.roleSelect = roleSelect;
    }

    void Start()
    {
        selfCursorRect = selfCursor.GetComponent<RectTransform>();
        selfReadyState = selfState.transform.Find("State").gameObject;
        otherCursorRect = otherCursor.GetComponent<RectTransform>();
        otherReadyState = otherState.transform.Find("State").gameObject;

        ChangeButtonVisible(reselectButton, false);
        ChangeButtonVisible(checkButton, false);
        ChangeButtonVisible(startButton, false);
        ChangeState(selfReadyState, false);
        ChangeState(otherReadyState, false);
    }

    void Update()
    {
        //後でInputSystem用にHandlerみたいなクラス作る
        if (IsLocked){ return; }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveCursor(selfCursorRect);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveCursor(selfCursorRect);
        }
    }
    /// <summary>
    /// カーソル移動。プレイヤーの入力から呼ばれる
    /// </summary>
    /// <param name="isCommander"></param>
    void MoveCursor(RectTransform rect)
    {
        if (IsCommander)
        {
            rect.transform.localPosition = selfRightPos;
            IsCommander = false;
        }
        else
        {
            rect.transform.localPosition = selfLeftPos;
            IsCommander = true;
        }
    }
    /// <summary>
    /// カーソル移動。サーバーから受け取った値を使う
    /// </summary>
    /// <param name="isCommander"></param>
    void MoveCursor(bool isCommander)
    {
        if (isCommander)
        {
            otherCursorRect.transform.localPosition = otherLeftPos;
        }
        else
        {
            otherCursorRect.transform.localPosition = otherRightPos;
        }
    }
    public void ToNext()
    {
        Debug.Log("すたーと");
    }
    /// <summary>
    /// サーバーに選択情報送信
    /// ボタンから呼ばれる
    /// </summary>
    public void SendRole()
    {
        if(IsLocked) { return; }
        SelectionData data = new SelectionData(PlayerIdManager.Id,true,IsCommander);
        StartCoroutine(roleSelect.PostSelection(data, onSuccess: () =>
        {
            IsLocked = true;
            ChangeState(selfReadyState, IsLocked);
            ChangeButtonVisible(sendButton, false);
            MatchingManager.IsCommander = IsCommander;
            Debug.Log("成功");
        },
        onError: (err) =>
        {
            Debug.Log("失敗" +  err);
        }
        ));
    }
    /// <summary>
    /// 再選択のリクエストを送信
    /// ボタンから呼ばれ、ホストしか押せない
    /// </summary>
    public void SendReselection()
    {
        if (!IsLocked) { return; }
        StartCoroutine(roleSelect.PostReselection(onSuccess:()=>
            {
                Debug.Log("再選択");
                UnlockSelect();
                ChangeButtonVisible(sendButton, true);
                ChangeButtonVisible(checkButton, false);
                ChangeButtonVisible(reselectButton, false);
                ChangeButtonVisible(startButton, false);
            },
            onError: (err)=>
            {
                Debug.Log("失敗" + err);
            }
        ));
    }
    /// <summary>
    /// 役職衝突してるか判定をサーバーにリクエスト
    /// ボタンから呼ばれ、ホストしか押せない
    /// </summary>
    public void SendConflictCheck()
    {
        if (!IsLocked) { return; }
        StartCoroutine(roleSelect.GetHasConflict(onSuccess: (result) =>
            {
                Debug.Log("成功");
                roleSelectManager.HasConflict = result;
                if (roleSelectManager.HasConflict)
                {
                    UnlockSelect();
                    ChangeButtonVisible(sendButton, true);
                    ChangeButtonVisible(checkButton, false);
                }
                //衝突してなければ再選択ボタンとスタートボタン表示
                else
                {
                    ChangeButtonVisible(reselectButton, true);
                    ChangeButtonVisible(startButton, true);
                }
            },
            onError: (err) =>
            {
                Debug.Log("失敗" + err);
            }
        ));
    }

    /// <summary>
    /// 定期取得される全員の選択情報を反映
    /// </summary>
    /// <param name="dataList"></param>
    public void UpdateUI(SelectionDataList dataList)
    {
        if(dataList.selections == null) { return; }
        foreach (SelectionData data in dataList.selections)
        {
            //自分のロックの状態更新
            if(PlayerIdManager.Id == data.player_id)
            {
                Debug.Log("自分のUI操作");
                roleSelectManager.IsHostButtonLocked = data.is_locked;
            }
            //相手のロックの状態更新
            else
            {
                roleSelectManager.IsGuestButtonLocked = data.is_locked;
                MoveCursor(data.is_commander);
                ChangeState(otherReadyState, data.is_locked);
            }
        }

        //自分が選択した後にサーバーで選択解除されたら
        if (IsLocked)
        {
            if (!roleSelectManager.IsHostButtonLocked && !roleSelectManager.IsGuestButtonLocked)
            {
                UnlockSelect();
                ChangeButtonVisible(sendButton, true);
            }
        }
        //全員がロックしてたらホストのみ確認ボタン表示
        if (roleSelectManager.IsHostButtonLocked && roleSelectManager.IsGuestButtonLocked)
        {
            if(MatchingManager.IsHost)
            {
                ChangeButtonVisible(checkButton, true);
            }
        }
        Debug.Log("UI Updated : " + dataList);
    }
    /// <summary>
    /// 自分と相手の選択を解除
    /// 役職が衝突したり、再選択が押された際に呼ばれる
    /// </summary>
    void UnlockSelect()
    {
        IsLocked = false;
        ChangeState(selfReadyState, false);
        ChangeState(otherReadyState, false);
    }
    /// <summary>
    /// ReadyのUI切り替え
    /// </summary>
    /// <param name="readyState"></param>
    /// <param name="is_locked"></param>
    void ChangeState(GameObject readyState ,bool is_locked)
    {
        readyState.SetActive(is_locked);
    }
    /// <summary>
    /// ボタンの表示状態切り替え
    /// </summary>
    /// <param name="button"></param>
    /// <param name="isVisible"></param>
    void ChangeButtonVisible(Button button, bool isVisible)
    {
        button.gameObject.SetActive(isVisible);
    }
    
}
