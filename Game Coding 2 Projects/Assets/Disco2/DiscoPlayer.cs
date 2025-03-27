using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscoPlayer : MonoBehaviour
{
    CharacterController cc;
    public float speed = 5f;

    //store references to functions that take no parameters and return nothing
    //placeholder for function //custom function type that we define ourselves
    public delegate void InputEvents();
    //static var to act like global events
    //empty until another script subscribes functions to them
    public static InputEvents OnInteract;
    public static InputEvents OnEnterInteractable;
    public static InputEvents OnExitInteractable;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        cc.Move(move * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("entered trigger");
        //when onEnterInteractable happens i want you to also run my SHOWUI function
        //if anything is subscribed to onEnterInteractable call all of them (whoever is listening)
        if (OnEnterInteractable != null) OnEnterInteractable();
    }

    private void OnTriggerExit(Collider other)
    {
        if(OnExitInteractable != null) OnExitInteractable();
    }
}
