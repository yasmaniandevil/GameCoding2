using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class DialogueManager : MonoBehaviour
{

    //reference to our scriptable obj
    public DialogueLine startingLine;

    //content of scroll view
    public Transform chatContent; //where messages will be placed
    //template for single chat message
    //can add an image as a parent for a "bubble"
    public GameObject textLinePrefab;
    //template for buttons
    public GameObject choiceButtonPrefab;
    //where choice buttons (above) should go
    public Transform choiceContainer;
    //a button that appears when they are no choices and you can continue to next line
    public Button continueButton;

    //keeps track of dialogue line currently showing
    DialogueLine currentLine;


    //later
    bool dialogueRunning = false;
    public bool endConversation;
    public UnityEvent onEndConversation;
   
    //calls when interaction begins (trigger or key)
    public void StartingDialogue()
    {
        //later in lesson
        if(dialogueRunning) return;
        dialogueRunning = true;
        
        //FIRST
        //starts dialogue flow by calling update dialogue and passing in the first line
        UpdateDialogue(startingLine);

        
    }

    public void UpdateDialogue(DialogueLine line)
    {
        

        //{{{!!!@!!!FIRST THIS
        //sets current line to the passed in line
        currentLine = line;
        Debug.Log("Current Line" + currentLine);
        //ShowLine(currentLine);
        StartCoroutine(DisplayDialogue(currentLine));
        Debug.Log("called corotine on CALL");


        //enables continue button when dialogue begins
        continueButton.enabled = true;
    }


    IEnumerator DisplayDialogue(DialogueLine line)
    {
        
        foreach (string dialogueLine in currentLine.dialogueLinesList)
        {
            //make a new copy of the button
            GameObject dialogueText = Instantiate(textLinePrefab, chatContent);
            //TextMeshProUGUI dialogueTextVar = GetComponentInChildren<TextMeshProUGUI>();
            TextMeshProUGUI dialogueTextVar = dialogueText.GetComponent<TextMeshProUGUI>();
            //set the text of it to whatever string we are currently looping over
            dialogueTextVar.text = dialogueLine;
            yield return new WaitForSeconds(1f);

        }

        //ensure continue button is below all chat
        continueButton.transform.SetAsLastSibling();

        // Clear old choice buttons so they dont stack
        foreach (Transform child in choiceContainer) Destroy(child.gameObject);
        //hides continue button by default
        continueButton.gameObject.SetActive(false);
        //button choices appear after latest chat line
        choiceContainer.transform.SetAsLastSibling();

        //display choices or continue button
        //does this dialogue line even have choices? are there any options in the list?
        //if yes continue
        if (line.choices != null && line.choices.Length > 0)
        {
            //for every choice attached to the line
            foreach (DialogueChoice choice in line.choices)
            {
                //create a button
                GameObject newButtonChoice = Instantiate(choiceButtonPrefab, choiceContainer);
                //set buttons text to say what the choice text is
                //btnObj.GetComponentInChildren<TextMeshProUGUI>().text = choice.choiceText;
                newButtonChoice.GetComponent<OptionsChoices>().SetUp(this, choice.nextLine, choice.choiceText);
                Debug.Log("choice text" + choice.nextLine + choice.choiceText);
                Debug.Log("choice was clicked");

                
            }
        }
        //if there are no choices but theres next line show continue button
        //remove all listeners ensures we dont accidently stack duplicate listeners
        //clicking the button triggers the next line
        else if (line.nextLine != null)
        {
            continueButton.gameObject.SetActive(true);
            //clear everything out that was set to happen when the button was clicked
            //reusing the same button for diff lines if we dont clear it clicking button could trigger old lines
            continueButton.onClick.RemoveAllListeners();
            //when this button is clicked run this code
            continueButton.onClick.AddListener(() =>
            {
                UpdateDialogue(line.nextLine); //continue to next line
                continueButton.gameObject.SetActive(false); //hide button after clicking

                //later in lesson to ensure ending conversation
                if(line.choices == null && line.nextLine == null)
                {
                    onEndConversation.Invoke();
                    dialogueRunning = false;
                }
                
            });
        }
        
    }


    
}
