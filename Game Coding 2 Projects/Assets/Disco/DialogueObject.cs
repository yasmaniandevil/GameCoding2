using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueObject", menuName = "NPC Dialogue Object", order = 0)]
public class DialogueObject : ScriptableObject
{
    //actual dialogue
    //public List<string> dialogueStringsList = new List<string>();

    public DialogueObject endDialogue; //optional to link to other dialogues

    public List<DialogueSegment> SegmentsList = new List<DialogueSegment>();

}

//second part of lesson
//make it seralizable to public/avail for above
//making our own data type
[System.Serializable]
public struct DialogueSegment
{
    public string dialogueText;
    public float dialogueDisplayTime;

    public List<DialogueChoice> dialogueChoicesList;
}

//part 3 of lesson
[System.Serializable]
public struct DialogueChoice
{
    public string dialogueChoice;
    public DialogueObject followOnDialogue;

}
