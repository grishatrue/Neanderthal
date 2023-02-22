using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonEnabler : MonoBehaviour
{
    public Button Button;

    public Text SaveNameInputField;
    private string newSaveName;

    public void OnInputFieldChangedOrEndEdit()
    {
        if (SaveNameInputField.text != "")
        {
            newSaveName = SaveNameInputField.text;
            Button.GetComponent<Button>().interactable = true;
        }
        else
        {
            newSaveName = SaveNameInputField.text;
            Button.GetComponent<Button>().interactable = false;
        }
    }
}