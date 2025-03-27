using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OptionsChoices : MonoBehaviour
{
    DialogueManager dialogueManager;
    DialogueLine dialogueLine;
    public TextMeshProUGUI dialogueText;


    //did the _ before to show diff var naming conventions
    public void SetUp(DialogueManager _dialogueManager, DialogueLine _dialogueLine, string _dialogueText)
    {
        dialogueManager = _dialogueManager;
        dialogueLine = _dialogueLine;
        dialogueText.text = _dialogueText;
    }

    //dont need for now
    public void SelectOption()
    {
        dialogueManager.UpdateDialogue(dialogueLine);
    }
}
