using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class DialogueManager : MonoBehaviour
{

    //reference to our scriptable obj
    public DialogueLine currentLine;

    [Header("UI")]
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
    private TextMeshProUGUI dialogueTextVar;


    //later
    bool dialogueRunning = false;
    public bool endConversation;
    public UnityEvent onEndConversation;

    [Header("Dialogue History")]
    public List<string> dialogueHistory = new List<string>();

    //calls when interaction begins (trigger or key)
    public void StartingDialogue()
    {
        //later in lesson
        if(dialogueRunning) return;
        dialogueRunning = true;
        
        //FIRST
        //starts dialogue flow by calling update dialogue and passing in the first line
        UpdateDialogue(currentLine);
    }

    public void UpdateDialogue(DialogueLine line)
    {
        //sets current line to the passed in line
        currentLine = line;
        Debug.Log("Current Line" + currentLine);
        //ShowLine(currentLine);
        StartCoroutine(DisplayDialogue(currentLine));
    }


    IEnumerator DisplayDialogue(DialogueLine line)
    {
        
        foreach (string dialogueLineString in currentLine.dialogueLinesList)
        {
            //make a new copy of the button
            GameObject dialogueText = Instantiate(textLinePrefab, chatContent);
            //TextMeshProUGUI dialogueTextVar = GetComponentInChildren<TextMeshProUGUI>();
            dialogueTextVar = dialogueText.GetComponent<TextMeshProUGUI>();

            string finalLine = SwitchCharacters(currentLine, dialogueLineString);
            dialogueHistory.Add(finalLine);

            //set the text of it to whatever string we are currently looping over
            //dialogueTextVar.text = dialogueLineString;

            //later in lesson
            /*if (!string.IsNullOrEmpty(line.speakerName))
            {

                //dialogueTextVar.text = $"<b>{line.speakerName}:</b> {dialogueLine}";
            }*/

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
        RunChoices(currentLine);
        //if there are no choices but theres next line show continue button
        //remove all listeners ensures we dont accidently stack duplicate listeners
        //clicking the button triggers the next line
        ContinueButton(currentLine);
        
    }

    #region continue button
    public void ContinueButton(DialogueLine line)
    {
        if(line.nextLine != null)
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
                if (line.choices == null && line.nextLine == null)
                {
                    onEndConversation.Invoke();
                    dialogueRunning = false;
                }

            });
        }
    }
    #endregion continue button

    #region switch speakers
    public string SwitchCharacters(DialogueLine line, string _dialogueLine)
    {
        string formattedLine = _dialogueLine;

        switch (line.characters)
        {
            case DialogueLine.Characters.blank:
                Debug.Log("Does nothing");
                dialogueTextVar.text = _dialogueLine;
                break;
            case DialogueLine.Characters.beth:
                Debug.Log("it is beth");
                formattedLine = $"<b> Beth: </b> {_dialogueLine}";
                dialogueTextVar.text = formattedLine;
                dialogueTextVar.color = Color.red;
                Debug.Log("formatted line: " + formattedLine);
                break;
            case DialogueLine.Characters.tori:
                formattedLine = $"<b> tori: </b> {_dialogueLine}";
                dialogueTextVar.text = formattedLine;
                dialogueTextVar.color = Color.blue;
                Debug.Log("formatted line: " + formattedLine);
                break;
            case DialogueLine.Characters.me:
                string me = DialogueLine.Characters.me.ToString();
                formattedLine = $"<b> {me}: </b>  {_dialogueLine}";
                dialogueTextVar.text = formattedLine;
                dialogueTextVar.color = Color.yellow;
                Debug.Log("formatted line: " + formattedLine);
                break;
            case DialogueLine.Characters.you:
                break;
            default:
                dialogueTextVar.text = _dialogueLine;
                break;
            
        }

        return formattedLine;
        
    }
    #endregion switch speakers

    #region run choices
    public void RunChoices(DialogueLine line)
    {
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

                    //@@@@!!!!%%%MAYBE DO THIS ONE FIRST THE HELPER FUNCTION?
                    //checks player stats and returns the current value (stored in playerStat)
                    //int playerStat = GetPlayerStatValue(choice.requiredStat);


                    int playerStat = PlayerStats.Instance.GetStat(choice.requiredStat);
                    //checks if it is greater than or equal to required value
                    //if it is, it sets it to true
                    meetsRequirment = playerStat >= choice.requiredValue;
                }

                //check if they unlocked this path by seeing if they have the required flag
                if (!string.IsNullOrEmpty(choice.requiredFlag))
                {
                    //keep meetsrequirment true if it is already true and they have the required flag
                    meetsRequirment &= PlayerStats.Instance.HasChoiceFlag(choice.requiredFlag);
                    //above is shorthand for
                    //meetsRequirment = meetsRequirment && PlayerStats.Instance.HasChoiceFlag(choice.requiredFlag);
                    if (!meetsRequirment)
                    {
                        Debug.Log($"player has not unlocked {choice.requiredFlag} path");

                    }

                    if (meetsRequirment)
                    {
                        Debug.Log($"player starts on {choice.requiredFlag} path");
                    }
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

                    //if there is a reward flag add reward path
                    if (!string.IsNullOrEmpty(choice.rewardFlag))
                    {
                        PlayerStats.Instance.AddChoiceFlag(choice.rewardFlag);
                        Debug.Log("unlocked new path " + choice.rewardFlag);
                    }

                    dialogueHistory.Add($"Choice: {choice.choiceText}");
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
    }
    #endregion run choices


}
