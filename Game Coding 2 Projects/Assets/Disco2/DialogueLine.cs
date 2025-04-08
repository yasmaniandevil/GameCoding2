using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Line")]
//SO is a data container, allows you to store large quantites of shared data independent from script instances
    public class DialogueLine : ScriptableObject
    {
        //who is speaking
        public string speakerName;

        public Sprite speakerSprite;

        //all the sentences
        [TextArea]public List<string> dialogueLinesList = new List<string>();


        //next line if there are no choices
        public DialogueLine nextLine;
        //choices if there are any
        //multiple choices are store inside of array
        public DialogueChoice[] choices;

        //for later
        public bool isPlayer;

        //you can add a unity event UnityEvent OnChoose
    }
    //each individual choice a player can make
    //we have this seperate class inside SO for when we want to have choices
    //a choice is its own object bc it can contain, choice text, path it leads to (next line) optional stat and reward
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
    }



