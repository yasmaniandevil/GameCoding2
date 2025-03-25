using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conversation : ScriptableObject
{
    public List<Conversation> conversationEntries;
}

[System.Serializable]
public class ConversationEntry
{
    public string speaker;
    [TextArea] public string text;
    public float waitBeforeNext = 1f;
    public DialogueChoice[] choices;
}

[System.Serializable]
public class ConversationChoice
{
    public string choiceText;
    public ConversationChoice nextConvo;
    
}
