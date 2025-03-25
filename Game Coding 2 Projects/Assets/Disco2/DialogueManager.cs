using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{

    //reference to our scriptable obj
    public DialogueLine startingLine;

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
    DialogueLine currentLine;

   
    //calls when interaction begins (trigger or key)
    public void StartingDialogue()
    {
        //starts dialogue flow by calling update dialogue and passing in the first line
        UpdateDialogue(startingLine);
    }

    public void UpdateDialogue(DialogueLine line)
    {
        //sets current line to the passed in line
        currentLine = line;
        ShowLine(currentLine);

        
        //enables continue button when dialogue begins
        continueButton.enabled = true;
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
        if (line.choices != null && line.choices.Length > 0)
        {
            foreach (var choice in line.choices)
            {
                GameObject btnObj = Instantiate(choiceButtonPrefab, choiceContainer);
                btnObj.GetComponentInChildren<TextMeshProUGUI>().text = choice.choiceText;

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
}
