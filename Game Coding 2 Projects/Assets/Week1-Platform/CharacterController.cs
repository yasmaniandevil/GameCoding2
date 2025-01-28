using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    //player movement speed
    public float moveSpeed = 6f;
    //jump height
    public float jumpHeight = 2f;
    //gravity strength
    public float gravity = -9f;
    //how long you can hold jump to go higher
    public float jumpHoldTime = .2f;

    private CharacterController controller;
    //stores vertical movement (gravity and jumping)
    private Vector3 velocity;
    private bool isJumping;
    private float jumpTimer;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(moveX, 0, moveZ) * moveSpeed;
        //controller.Move(move * Time.deltaTime);

        

        
    }
}
