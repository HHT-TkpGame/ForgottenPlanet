using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldManager : MonoBehaviour
{
    [SerializeField] InputField inputField;
    [SerializeField] TMP_Text displayText;

public void DisplayInputText()
    {
        displayText.text = inputField.text;
    }
}
