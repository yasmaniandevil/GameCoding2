using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interact : MonoBehaviour
{
    public UnityEvent OnInteract;

    public Canvas canvas;

    public DialogueManager dialogueManager;
    //if start on trigger is true
    public bool startOnTrigger;

    public DialogueObject startingDialouge;
    


    // Start is called before the first frame update
    private void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interaction();
        }


    }

    private void Interaction()
    {
        Debug.Log("Interacting");
        OnInteract.Invoke();
    }

    private void OnEnable()
    {
        DiscoPlayer.OnInteract += Interaction;
        DiscoPlayer.OnEnterInteractable += ShowUI;
        DiscoPlayer.OnExitInteractable += HideUI;
    }

    private void OnDisable()
    {
        DiscoPlayer.OnInteract -= Interaction;
        DiscoPlayer.OnEnterInteractable -= ShowUI;
        DiscoPlayer.OnExitInteractable -= HideUI;
    }

    void ShowUI()
    {
        canvas.enabled = true;
        

        if(startOnTrigger && dialogueManager != null)
        {
            dialogueManager.StartingDialogue(startingDialouge);
            //has talked = true so npc only triggers once
        }
    }

    void HideUI()
    {
        canvas.enabled = false;
        
    }

}
