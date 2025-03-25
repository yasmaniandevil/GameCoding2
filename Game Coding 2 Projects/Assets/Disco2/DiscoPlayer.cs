using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscoPlayer : MonoBehaviour
{
    CharacterController cc;
    public float speed = 5f;

    public delegate void InputEvents();
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
        if (OnEnterInteractable != null) OnEnterInteractable();
    }

    private void OnTriggerExit(Collider other)
    {
        if(OnExitInteractable != null) OnExitInteractable();
    }
}
