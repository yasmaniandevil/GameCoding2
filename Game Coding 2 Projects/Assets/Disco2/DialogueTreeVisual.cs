using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class DialogueTreeVisual : MonoBehaviour
{
    public DialogueLine[] dialogueLines;

    private void OnDrawGizmos()
    {
        if (dialogueLines == null || dialogueLines.Length == 0) return;

        Gizmos.color = Color.green;

        foreach (DialogueLine line in dialogueLines)
        {
            if (line == null) continue;

            Vector3 fromPosition = new Vector3(line.editorPosition.x, line.editorPosition.y, 0);

            // Draw to next line if exists
            if (line.nextLine != null)
            {
                Vector3 toPosition = new Vector3(line.nextLine.editorPosition.x, line.nextLine.editorPosition.y, 0);
                Gizmos.DrawLine(fromPosition, toPosition);
#if UNITY_EDITOR
                Handles.Label((fromPosition + toPosition) * 0.5f, line.name + " ➔ " + line.nextLine.name);
#endif
            }

            // Draw lines for choices
            if (line.choices != null)
            {
                foreach (DialogueChoice choice in line.choices)
                {
                    if (choice.nextLine != null)
                    {
                        Vector3 toPosition = new Vector3(choice.nextLine.editorPosition.x, choice.nextLine.editorPosition.y, 0);
                        Gizmos.DrawLine(fromPosition, toPosition);
                        #if UNITY_EDITOR
                        Handles.Label((fromPosition + toPosition) * 0.5f, line.name + " ➔ " + choice.nextLine.name);
                        #endif
                    }
                }
            }
        }
    }
}
