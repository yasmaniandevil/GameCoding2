using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DialogueLine))]
public class DialogueGizmo : Editor
{
    

    void OnSceneGUI()
    {
        DialogueLine line = (DialogueLine)target;

        if (line.nextLine != null)
        {
            DrawConnection(line, line.nextLine, "Flow");
        }

        if (line.choices != null)
        {
            foreach (var choice in line.choices)
            {
                if (choice.nextLine != null)
                {
                    DrawConnection(line, choice.nextLine, "Choice");
                }
            }
        }
    }

    void DrawConnection(DialogueLine from, DialogueLine to, string connectType = "Next")
    {
        if (from == null || to == null) return;

        // Get positions in world space if they have Scene references
        // fallback: position them visually in Scene View
        Vector3 fromPosition = new Vector3(from.editorPosition.x, from.editorPosition.y, 0);
        Vector3 toPosition = new Vector3(to.editorPosition.x, to.editorPosition.y, 0);

        // Draw a line between them
        Handles.color = Color.green;
        Handles.DrawLine(fromPosition, toPosition);

        // Optional: Draw a label at the midpoint
        Vector3 midPoint = (fromPosition + toPosition) / 2;
        Handles.Label(midPoint, $"{from.name} ➔ {to.name}");


    }
}
