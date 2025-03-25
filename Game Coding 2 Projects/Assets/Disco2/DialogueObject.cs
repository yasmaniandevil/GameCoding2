using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/OBJ")]
public class DialogueObject : ScriptableObject
{
    public List<DialogueEntries> dialogueEntries;
}

[System.Serializable]
public class DialogueEntries
{
    public string speaker;
    [TextArea] public string text;
    public float waitBeforeNext = 1f;
    public DialogueChoice[] choices;
    public bool isPlayer;
}

[System.Serializable]
public class DialogueChoices
{
    public string choiceText;
    public DialogueChoices nextConvo;
    
}
