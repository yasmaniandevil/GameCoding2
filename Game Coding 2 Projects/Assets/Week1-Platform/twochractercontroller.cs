using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class twochractercontroller : MonoBehaviour
{
    //stores movement input
    private Vector3 PlayerMovementInput;
    //stores velocity for gravity and jumping y axis movement
    private Vector3 velocity;
    private CharacterController controller;
    public float moveSpeed;
    public float jumpForce;
    //negative to pull the player down
    private float gravity = -9.81f;

    public bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //different way to write input.getaxis
        //get player input 
        //moves along x left and right and z forward and back
        PlayerMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        
        MovePlayer();

        if(isGrounded )
        {
            Debug.Log("Grounded");
        }
        
    }

    private void MovePlayer()
    {
        //Vector3 MoveVector = transform.TransformDirection(PlayerMovementInput);

        isGrounded = controller.isGrounded;

        //if the player is on the ground reset gravity and allow jumping
        if(isGrounded)
        {
            //small downward force to keep grounded
            velocity.y = -1;

            //if space is pressed apply jump force
            if(Input.GetKeyDown(KeyCode.Space))
            {
                //apply upward force
                velocity.y = jumpForce;
            }
        }
        else
        {
            //apply gravity when player is in the air
            velocity.y -= gravity * -2f * Time.deltaTime;
        }

        //move player on the x and z
        controller.Move(PlayerMovementInput * moveSpeed * Time.deltaTime);
        //apply vertical movement (gravity and jumping)
        controller.Move(velocity * Time.deltaTime);
    }
}
