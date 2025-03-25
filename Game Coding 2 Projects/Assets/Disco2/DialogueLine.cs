using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Line")]
//SO is a data container, allows you to store large quantites of shared data independent from script instances
    public class DialogueLine : ScriptableObject
    {
        [TextArea] public string text;
        public bool isPlayer;
        public DialogueLine nextLine;
        public DialogueChoice[] choices;
    }

    [System.Serializable]
    public class DialogueChoice
    {
        public string choiceText;
        public DialogueLine nextLine;
    }

