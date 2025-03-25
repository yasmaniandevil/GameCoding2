using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class UIInteract : MonoBehaviour
{
    public UnityEvent OnInteract;
    private Canvas canvas;
    
    // Start is called before the first frame update
    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        //canvas.enabled = false;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    private void OnEnable()
    {
        DiscoPlayer.OnInteract += Interact;
        DiscoPlayer.OnEnterInteractable += ShowUI;
        DiscoPlayer.OnExitInteractable += HideUI;
    }

    private void OnDisable()
    {
        DiscoPlayer.OnInteract -= Interact;
        DiscoPlayer.OnEnterInteractable -= ShowUI;
        DiscoPlayer.OnExitInteractable -= HideUI;
    }

    void Interact()
    {
        Debug.Log("Interacting");
        OnInteract.Invoke();

    }

    void ShowUI()
    {
        canvas.enabled=true;
    }

    void HideUI()
    {
        canvas.enabled=false;
    }
}
