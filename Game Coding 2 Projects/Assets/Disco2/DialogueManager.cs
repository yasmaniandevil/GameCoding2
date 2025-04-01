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
                //grab the text on button
                TextMeshProUGUI buttonText = newButtonChoice.GetComponentInChildren<TextMeshProUGUI>();
                //newButtonChoice.GetComponentInChildren<TextMeshProUGUI>().text = choice.choiceText;
                
                //create a bool set to true
                //meet requirment is always true unless the required stat string isnt empty then we check it
                bool meetsRequirment = true;
                //if requried stat field is not empty
                //if there is no required stat we skip this if statement
                if (!string.IsNullOrEmpty(choice.requiredStat))
                {
                    //checks player stats and returns the current value (stored in playerStat)
                    //@@@@!!!!%%%MAYBE DO THIS ONE FIRST THE HELPER FUNCTION?
                    //int playerStat = GetPlayerStatValue(choice.requiredStat);


                    int playerStat = PlayerStats.Instance.GetStat(choice.requiredStat);
                    //checks if it is greater than or equal to required value
                    //if it is, it sets it to true
                    meetsRequirment = playerStat >= choice.requiredValue;
                }
                
                //update button text
                buttonText.text = choice.choiceText;
                //if it doesnt meet requirment
                if (!meetsRequirment)
                {
                    //add red and say the requried stat and amount
                    //buttonText.text += $" <color=red>({choice.requiredStat} : {choice.requiredValue})</color>";
                    buttonText.text += "<color=red>" + choice.requiredStat + ": " + choice.requiredValue + "</color>";
                    

                }

                //grab the button component of the choice button
                Button buttonComp = newButtonChoice.GetComponent<Button>();
                buttonComp.onClick.AddListener(() =>
                {
                    //if there is reward increase stat
                    if (!string.IsNullOrEmpty(choice.rewardStat))
                    {
                        PlayerStats.Instance.IncreaseStat(choice.rewardStat, choice.rewardAmt);
                    }
                });
                //it is interactable depending on if meets requirment is true or false
                buttonComp.interactable = meetsRequirment;

                //if meet requirment or no stat check is true
                if (meetsRequirment)
                {
                    //let us click new button
                    /*newButtonChoice.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        UpdateDialogue(choice.nextLine);
                    });*/

                    newButtonChoice.GetComponent<OptionsChoices>().SetUp(this, choice.nextLine, choice.choiceText);

                    
                    
                }

                
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

    //helper function returns an int
    //takes name of stat (logic etc) and returns the players current value for that stat
    //we make a function so dialogue logic doesnt need to directly access player states it just calls this method
    /*int GetPlayerStatValue(string statName)
    {
        switch(statName)
        {
            case "charisma": return PlayerStats.Instance.charisma;
            case "logic": return PlayerStats.Instance.logic;
            case "empathy": return PlayerStats.Instance.empathy;
            default: return 0;

        }
    }*/

    /*void ExampleFunction()
    {
        if(PlayerStats.Instance.GetStat("Logic") >= 2)
        {
            Debug.Log("Do something");
        }

        if (PlayerStats.Instance.stats.ContainsKey("Empathy"))
        {
            
        }
    }*/

    
}
