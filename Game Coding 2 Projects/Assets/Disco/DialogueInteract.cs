using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueInteract : MonoBehaviour
{
    public DialogueObject dialogueObj;
    public TextMeshProUGUI dialogueText;

    public GameObject dialogueContainer;
    public Transform dialogueOptionsParent;
    public GameObject dialogueButtonPrefab;

    public Canvas canvas;

    bool optionSelected = false;
    public void StartDialogue()
    {
        StartCoroutine(DisplayDialogue());
    }

    IEnumerator DisplayDialogue()
    {
        canvas.enabled = true;
        //part 1
        /*for(int i = 0; i < dialogueObj.dialogueStringsList.Count; i++)
        {
            Debug.Log(dialogueObj.dialogueStringsList[i]);
            dialogueText.text = dialogueObj.dialogueStringsList[i];
            yield return new WaitForSeconds(1f);
        }*/
        //later in the lesson part 2
        foreach (var dialogue in dialogueObj.SegmentsList)
        {
            //overwrites the previous message
            //dialogueText.text = dialogue.dialogueText;

            //need to append each new line to existing text 
            //optional name
            //dialogueText.text += "<b>Jenny:</b>" + dialogue.dialogueText + "\n";

            //bettter written than above
            GameObject newLine = new GameObject("DailogueLine");
            newLine.transform.SetParent(dialogueOptionsParent, false);//adds under scroll view
            dialogueText.text += "<b>Jenny:</b>" + dialogue.dialogueText + "\n";

            //part 3
            //if there are options the wait will not happen
            if (dialogue.dialogueChoicesList.Count == 0)
            {
                yield return new WaitForSeconds(dialogue.dialogueDisplayTime);
            }
            else
            {
                dialogueContainer.SetActive(true);
                //open options panel
                foreach(var option in dialogue.dialogueChoicesList)
                {
                    GameObject newButton = Instantiate(dialogueButtonPrefab, dialogueOptionsParent);
                    newButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = option.dialogueChoice;

                }

                while (!optionSelected)
                {
                    yield return null;
                }

            }
                yield return new WaitForSeconds(dialogue.dialogueDisplayTime);
        }
        dialogueContainer.SetActive(false);
        canvas.enabled = false;
        
    }

    public void OptionSelected(DialogueObject selectedObj)
    {
        optionSelected = true;
    }
}
