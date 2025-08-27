using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldManager : MonoBehaviour
{
    [SerializeField] InputField inputField;
    [SerializeField] TMP_Text displayText;
    MonitorController monitorController;

    public void GetMonitorController(MonitorController monitorController)
    {
        this.monitorController = monitorController;
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
                displayText.text = "è‚ª‚©‚è‚ğŒ©‚Â‚¯‚Ä‚¢‚È‚¢";
				break; 
            case 1:
				displayText.text = "³‰ğ";
				break;
            case 2:
				displayText.text = "•s³‰ğ";
				break;
        }
    }
}
