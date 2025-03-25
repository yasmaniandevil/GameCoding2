using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscoPlayer : MonoBehaviour
{
    private UnityEngine.CharacterController cc;
    public float speed = 5f;

    //templatte for any method with no paramaters and void return
    //definies three events other scripts can subscribe to
    public delegate void InputEvents();
    public static InputEvents OnInteract;
    public static InputEvents OnEnterInteractable;
    public static InputEvents OnExitInteractable;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<UnityEngine.CharacterController>();

        //if (OnInteract != null) OnInteract();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float forwardInput = Input.GetAxis("Vertical");
        

        Vector3 move = new Vector3(horizontalInput, 0, forwardInput);

        cc.Move(move * speed * Time.deltaTime);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //when player enters trigger it calls the onenterinteractable event if anything is listening
        if (other.CompareTag("Player"))
        {
            if(OnEnterInteractable != null) OnEnterInteractable();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(OnExitInteractable != null)OnExitInteractable();
        }
    }

}
