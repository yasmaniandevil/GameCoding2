using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{


    public DialogueLine startingLine;

    public Transform chatContent;
    public GameObject chatBubblePrefab;
    public GameObject choiceButtonPrefab;
    public Transform choiceContainer;
    public Button continueButton;

    DialogueLine currentLine;

    public void StartingDialogue()
    {
        UpdateDialogue(startingLine);
    }

    public void UpdateDialogue(DialogueLine line)
    {
        currentLine = line;
        ShowLine(currentLine);
    }

    void ShowLine(DialogueLine line)
    {
        if (line == null) {

            Debug.Log("no Line");
            return;

        } 
        // Instantiate chat bubble
        GameObject bubble = Instantiate(chatBubblePrefab, chatContent);
        TextMeshProUGUI textComp = bubble.GetComponentInChildren<TextMeshProUGUI>();
        textComp.text = line.text;

        // Clear old choices
        foreach (Transform child in choiceContainer) Destroy(child.gameObject);
        continueButton.gameObject.SetActive(false);

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
        else if (line.nextLine != null)
        {
            continueButton.gameObject.SetActive(true);
            continueButton.onClick.RemoveAllListeners();
            continueButton.onClick.AddListener(() => {
                ShowLine(line.nextLine);
            });
        }
    }

}
