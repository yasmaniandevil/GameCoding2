using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interact : MonoBehaviour
{
    //think of events as functions you can run later when something happen
    //instead of hardcoding call this function
    //you can say when this happens...run whatever functions are hooked up here
    //like a universal remove you press invoke() and every connected device turns on
    //unity event lets you hook up things in the inspector
    public UnityEvent OnInteract;

    public Canvas canvas;

    public DialogueManager dialogueManager;
    //if start on trigger is true
    public bool startOnTrigger;
    


    // Start is called before the first frame update
    private void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interaction(); //trigger interaction manually
        }


    }

    private void Interaction()
    {
        Debug.Log("Interacting");
        //lets run every function in my list!
        OnInteract.Invoke(); //trigger unity event
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
            dialogueManager.StartingDialogue();
            //has talked = true so npc only triggers once
        }
    }

    void HideUI()
    {
        canvas.enabled = false;
        
    }

}
