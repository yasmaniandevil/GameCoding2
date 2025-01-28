using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerCharacterController : MonoBehaviour
{
    //player movement speed
    public float moveSpeed = 6f;
    //jump height
    public float jumpHeight = 2f;
    //gravity strength
    public float gravity = -9.81f;
    //how long you can hold jump to go higher
    public float jumpHoldTime = .2f;

    private CharacterController controller;
    //stores vertical movement (gravity and jumping)
    private Vector3 velocity;
    private bool isJumping;
    private float jumpTimer;

    public bool isGrounded;

    

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //get player input 
        //and create a movement vector X and Z only because Y is controlled by gravity
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        //move the player using chractercontroller
        controller.Move(move * moveSpeed * Time.deltaTime);

        //different way to write it
        /*float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(moveX, 0, moveZ) * moveSpeed;
        controller.Move(move * Time.deltaTime);*/

        isGrounded = controller.isGrounded;

        //Groundcheck
        //if player is touching ground, reset jump related values
        if(isGrounded)
        {
            if(velocity.y <0)
            {
                //small downward force to keep grounded
                velocity.y = -2f;
            }

            //jumping logic
            if(Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("hit space");
                //Jump physics formula
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                isJumping = true;
                //reset jump timer
                jumpTimer = 0f;
            }
        }

        //variable jump height
        //if player is still holding the jump key, extend jump height slightly
        if(isJumping && Input.GetKey(KeyCode.Space) )
        {
            Debug.Log("Jump");
            //track how long jump is held
            jumpTimer += Time.deltaTime;
            if(jumpTimer < jumpHoldTime)
            {
                velocity.y += Mathf.Sqrt(jumpHeight * -2f * gravity) * Time.deltaTime;
            }
        }

        //apply gravity
        //apply gravity over time
        velocity.y += gravity * Time.deltaTime; 
        //apply vertical movement
        controller.Move(velocity * Time.deltaTime); 
        
        if(isGrounded && isJumping)
        {
            isJumping = false;
        }


        if(isGrounded)
        {
            //Debug.Log("chacerter is grounded");
        }

        
    }
}
