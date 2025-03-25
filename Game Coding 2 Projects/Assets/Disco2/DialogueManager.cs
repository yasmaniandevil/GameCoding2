using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class DialogueManager : MonoBehaviour
{

    //reference to our scriptable obj
    public DialogueObject startingLine;

    //content of scroll view
    public Transform chatContent;
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
    DialogueObject currentLine;

    private int currentIndex = 0;
    private bool waitingForChoice = false;

   
    //calls when interaction begins (trigger or key)
    public void StartingDialogue(DialogueObject convo)
    {
        //starts dialogue flow by calling update dialogue and passing in the first line
        //UpdateDialogue(startingLine);

        currentLine = convo;
        currentIndex = 0;
        StartCoroutine(RunConvo());
    }

    public void UpdateDialogue(DialogueLine line)
    {
        //sets current line to the passed in line
        //currentLine = line;
        //ShowLine(currentLine);

        
        //enables continue button when dialogue begins
        //continueButton.enabled = true;
    }

    void ShowLine(DialogueLine line)
    {
        //no line set up
        //avoid errors if a line wasnt passed in
        if (line == null) {

            Debug.Log("no Line");
            return;

        } 
        // Instantiate chat bubble child it to chatContent
        GameObject bubble = Instantiate(textLinePrefab, chatContent);
        //grab the textmeshpro component from child of bubble
        TextMeshProUGUI textComp = bubble.GetComponentInChildren<TextMeshProUGUI>();
        //updates text inside text mesh pro
        textComp.text = line.text;

        //makes sure continue button always at the bottom of scroll view after adding more chat
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
                GameObject btnObj = Instantiate(choiceButtonPrefab, choiceContainer);
                //set buttons text to say what the choice text is
                btnObj.GetComponentInChildren<TextMeshProUGUI>().text = choice.choiceText;

                //when this button is clicked show the next line of dialogue linked to this choice
                btnObj.GetComponent<Button>().onClick.AddListener(() => {
                    ShowLine(choice.nextLine);
                });
            }
        }
        //if there are no choices but theres another line show continue button
        //remove all listeners ensures we dont accidently stack duplicate listeners
        //clicking the button triggers the next line
        else if (line.nextLine != null)
        {
            continueButton.gameObject.SetActive(true);
            continueButton.onClick.RemoveAllListeners();
            continueButton.onClick.AddListener(() => 
            {
                ShowLine(line.nextLine);
            });
        }
    }


    //fade out gray previous lines for clarity
    //add typewriter effect
    //add speaker name
    //add auto scroll to bottom on new messages

    IEnumerator RunConvo()
    {
        while(currentIndex < currentLine.dialogueEntries.Count)
        {
            DialogueEntries entry = currentLine.dialogueEntries[currentIndex];
            
            ShowMessages(entry);

            if(entry.choices != null && entry.choices.Length > 0)
            {
                ShowChoices(entry);
                waitingForChoice = true;
                yield return new WaitUntil(() => waitingForChoice == false);
            }
            else
            {
                yield return new WaitForSeconds(1f);
                currentIndex++;
            }
        }
    }

    void ShowMessages(DialogueEntries entry)
    {
        GameObject bubble = Instantiate(textLinePrefab, chatContent);
        TextMeshProUGUI textComp = bubble.GetComponentInChildren<TextMeshProUGUI>();
        textComp.text = (entry.isPlayer ? "<b>You:</b> " : "<b>NPC:</b> ") + entry.text;
    }

    void ShowChoices(DialogueEntries entry)
    {
        foreach (Transform child in choiceContainer) Destroy(child.gameObject);

        /*foreach (DialogueChoices choice in entry.choices)
        {
            GameObject btnObj = Instantiate(choiceButtonPrefab, choiceContainer);
            btnObj.GetComponentInChildren<TextMeshProUGUI>().text = choice.choiceText;
            btnObj.GetComponent<Button>().onClick.AddListener(() => {
                foreach (Transform child in choiceContainer) Destroy(child.gameObject);
                ShowMessages(new DialogueEntries { text = choice.choiceText, isPlayer = true });
                //currentLine = choice.nextConvo;
                currentIndex = 0;
                waitingForChoice = false;
                StopAllCoroutines();
                StartCoroutine(RunConvo());
            });
        }*/
    }

}
