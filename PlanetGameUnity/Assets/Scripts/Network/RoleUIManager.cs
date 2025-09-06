using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoleUIManager : MonoBehaviour
{
    [SerializeField] Button sendButton;
    [SerializeField] Button checkButton;
    [SerializeField] Button reselectButton;
    [SerializeField] Button startButton;

    /////���^�]�p
    [SerializeField] GameObject selfCursor;
    RectTransform selfCursorRect;
    [SerializeField] GameObject otherCursor;
    RectTransform otherCursorRect;
    [SerializeField] GameObject selfState;
    GameObject selfReadyState;
    [SerializeField] GameObject otherState;
    GameObject otherReadyState;
    Vector3 selfLeftPos = new Vector3(-400, 230, 0);
    Vector3 selfRightPos = new Vector3(-80, 230, 0);
    Vector3 otherLeftPos = new Vector3 (-250, 230, 0);
    Vector3 otherRightPos = new Vector3(70, 230, 0);
    /////

    RoleSelectManager roleSelectManager;
    IRoleSelect roleSelect;
    GameStateRequester requester;

    bool onClicked;
    public bool IsLocked {  get; private set; }
    public bool IsCommander {  get; private set; }
    /// <summary>
    /// �ˑ��֌W�ݒ�
    /// </summary>
    /// <param name="roleSelectManager"></param>
    /// <param name="roleSelect"></param>
    public void Initialized(RoleSelectManager roleSelectManager, IRoleSelect roleSelect, GameStateRequester requester)
    {
        this.roleSelectManager = roleSelectManager;
        this.roleSelect = roleSelect;
        this.requester = requester;
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
        MoveCursor(selfCursorRect);
    }

    void Update()
    {
        //���InputSystem�p��Handler�݂����ȃN���X���
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
    /// �J�[�\���ړ��B�v���C���[�̓��͂���Ă΂��
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
    /// �J�[�\���ړ��B�T�[�o�[����󂯎�����l���g��
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
        if (onClicked) { return; }
        StartCoroutine(requester.PostState(onSuccess: (prog) =>
        {
            onClicked = true;
        },
        onError: (err) =>
        {
            Debug.LogError(err);
        }
        ));
    }
    /// <summary>
    /// �T�[�o�[�ɑI����񑗐M
    /// �{�^������Ă΂��
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
        },
        onError: (err) =>
        {
            Debug.Log("���s" +  err);
        }
        ));
    }
    /// <summary>
    /// �đI���̃��N�G�X�g�𑗐M
    /// �{�^������Ă΂�A�z�X�g���������Ȃ�
    /// </summary>
    public void SendReselection()
    {
        if (!IsLocked) { return; }
        StartCoroutine(roleSelect.PostReselection(onSuccess:()=>
            {
                UnlockSelect();
                ChangeButtonVisible(sendButton, true);
                ChangeButtonVisible(checkButton, false);
                ChangeButtonVisible(reselectButton, false);
                ChangeButtonVisible(startButton, false);
            },
            onError: (err)=>
            {
                Debug.Log("���s" + err);
            }
        ));
    }
    /// <summary>
    /// ��E�Փ˂��Ă邩������T�[�o�[�Ƀ��N�G�X�g
    /// �{�^������Ă΂�A�z�X�g���������Ȃ�
    /// </summary>
    public void SendConflictCheck()
    {
        if (!IsLocked) { return; }
        StartCoroutine(roleSelect.GetHasConflict(onSuccess: (result) =>
            {
                roleSelectManager.HasConflict = result;
                if (roleSelectManager.HasConflict)
                {
                    UnlockSelect();
                    ChangeButtonVisible(sendButton, true);
                    ChangeButtonVisible(checkButton, false);
                }
                //�Փ˂��ĂȂ���΍đI���{�^���ƃX�^�[�g�{�^���\��
                else
                {
                    ChangeButtonVisible(reselectButton, true);
                    ChangeButtonVisible(startButton, true);
                }
            },
            onError: (err) =>
            {
                Debug.Log("���s" + err);
            }
        ));
    }

    /// <summary>
    /// ����擾�����S���̑I�����𔽉f
    /// </summary>
    /// <param name="dataList"></param>
    public void UpdateUI(SelectionDataList dataList)
    {
        if(dataList.selections == null) { return; }
        foreach (SelectionData data in dataList.selections)
        {
            //�����̃��b�N�̏�ԍX�V
            if(PlayerIdManager.Id == data.player_id)
            {   
                roleSelectManager.IsHostButtonLocked = data.is_locked;
            }
            //����̃��b�N�̏�ԍX�V
            else
            {
                roleSelectManager.IsGuestButtonLocked = data.is_locked;
                MoveCursor(data.is_commander);
                ChangeState(otherReadyState, data.is_locked);
            }
        }

        //�������I��������ɃT�[�o�[�őI���������ꂽ��
        if (IsLocked)
        {
            if (!roleSelectManager.IsHostButtonLocked && !roleSelectManager.IsGuestButtonLocked)
            {
                UnlockSelect();
                ChangeButtonVisible(sendButton, true);
            }
        }
        //�S�������b�N���Ă���z�X�g�̂݊m�F�{�^���\��
        if (roleSelectManager.IsHostButtonLocked && roleSelectManager.IsGuestButtonLocked)
        {
            if(MatchingManager.IsHost)
            {
                ChangeButtonVisible(checkButton, true);
            }
        }
    }
    /// <summary>
    /// �����Ƒ���̑I��������
    /// ��E���Փ˂�����A�đI���������ꂽ�ۂɌĂ΂��
    /// </summary>
    void UnlockSelect()
    {
        IsLocked = false;
        ChangeState(selfReadyState, false);
        ChangeState(otherReadyState, false);
    }
    /// <summary>
    /// Ready��UI�؂�ւ�
    /// </summary>
    /// <param name="readyState"></param>
    /// <param name="is_locked"></param>
    void ChangeState(GameObject readyState ,bool is_locked)
    {
        readyState.SetActive(is_locked);
    }
    /// <summary>
    /// �{�^���̕\����Ԑ؂�ւ�
    /// </summary>
    /// <param name="button"></param>
    /// <param name="isVisible"></param>
    void ChangeButtonVisible(Button button, bool isVisible)
    {
        button.gameObject.SetActive(isVisible);
    }
    
}
