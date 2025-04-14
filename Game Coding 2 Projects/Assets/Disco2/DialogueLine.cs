using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Line")]
//SO is a data container, allows you to store large quantites of shared data independent from script instances
    public class DialogueLine : ScriptableObject
    {
        [Header("Editor Only")]
        public Vector2 editorPosition;

        public string speakerName;
        //list of text to show
        [TextArea]public List<string> dialogueLinesList = new List<string>();

        

        //next line if there are no choices
        public DialogueLine nextLine;
        //choices if there are any
        public DialogueChoice[] choices;

        //for later
        public bool isPlayer;

        //you can add a unity event UnityEvent OnChoose
    }
    //each individual choice a player can make
    [System.Serializable]
    public class DialogueChoice
    {
        public string choiceText; //what the choice says
        public DialogueLine nextLine; //what happens if you pick it

        public string requiredStat;
        public int requiredValue;

        //second part of lesson
        public string rewardStat;
        public int rewardAmt;

        //flags
        public string requiredFlag;
        public string rewardFlag;

    }

