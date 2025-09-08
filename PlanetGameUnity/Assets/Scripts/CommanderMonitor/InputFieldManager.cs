using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldManager : MonoBehaviour
{
    [SerializeField] InputField inputField;
    [SerializeField] TMP_Text displayText;
    MonitorController monitorController;

    AudioSource se;
    public void GetMonitorController(MonitorController monitorController)
    {
        this.monitorController = monitorController;
        se = GetComponent<AudioSource>();
    }

    public void DisplayInputText()
    {
        monitorController.SendCodeText(inputField.text.ToString());    
        
    }
    public void SetDisplayText(int num)
    {
        switch (num)
        {
            case 0:
                displayText.text = "�肪����������Ă��Ȃ�";
				break; 
            case 1:
				displayText.text = "����";
				break;
            case 2:
                se.Play();
                displayText.text = "�s����";
				break;
            case 3:
                displayText.text = "�񓚍ς�";
                break;
        }
    }
}
