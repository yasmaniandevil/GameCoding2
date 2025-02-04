using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    //stores movement input
    private Vector3 PlayerMovementInput;
    //stores velocity for gravity and jumping y axis movement
    private Vector3 velocity;
    private CharacterController controller;
    //negative to pull the player down
    private float gravity = -9.81f;

    //shmooving
    public float moveSpeed;
    public float jumpForce;
    public float sprintSpeed = 3f;

    //how long dash lasts
    public float maxDashDuration = .2f;
    //how far dash moves player
    public float dashDistance = 10f;
    public float dashCoolDown = 10f;
    private bool canDash = true;
    private bool isDashing = false;


    public bool isGrounded;
    public bool isSprinting = false;
    public bool groundPound = false;
    


    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;

        controller = GetComponent<CharacterController>();
         
    }

    // Update is called once per frame
    void Update()
    {
        //different way to write input.getaxis
        //get player input 
        //moves along x left and right and z forward and back
        PlayerMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        
        //MovePlayer();

        if(isGrounded )
        {
            //Debug.Log("Grounded");
        }

        if(Input.GetKey(KeyCode.LeftShift) && isGrounded)
        {
            isSprinting = true;
            //Debug.Log("Sprinting true");
        }

        if(Input.GetKeyDown(KeyCode.Q) && canDash && PlayerMovementInput != Vector3.zero)
        {
            StartCoroutine(Dash());
            Debug.Log("start coroutine");
            Debug.DrawRay(transform.position, Vector3.forward, Color.blue);
        }

        if (!isDashing)
        {
            //remove the other function
            MovePlayer();
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

       
        //sprinting
        float currentSpeed;
        if(isSprinting )
        {
            currentSpeed = sprintSpeed;
            //Debug.Log("CurrentSpeed: " + currentSpeed);
        }
        else
        {
            currentSpeed = moveSpeed;
            //Debug.Log("No Shift Speed: " + currentSpeed);
        }

        //dash

        //move player on the x and z
        controller.Move(PlayerMovementInput * currentSpeed * Time.deltaTime);
        //apply vertical movement (gravity and jumping)
        controller.Move(velocity * Time.deltaTime);

    }

    private IEnumerator Dash()
    {
        float startTime = Time.time;

        Debug.Log("Dash Started");
        isDashing = true;
        canDash = false;

        //calculate dash direction
        //normalize to ensure 
        Vector3 dashDirection = PlayerMovementInput.normalized;
        //how fast to move per second to cover dashDistance in dashDuration
        float dashSpeed = dashDistance / maxDashDuration;
        //time.time total since game started returns the elapsed time
        //time.deltatime time between frames, frame duration
        //returns the time in seconds since the last frame
        //float startTime = Time.time;

        //comparing time that has elapsed since start of game with
        while(Time.time < startTime + maxDashDuration)
        {
            //dash trail for VFX
            //move player in dash direction
            controller.Move(dashDirection * dashSpeed *  Time.deltaTime);
            yield return null; //wait until the next frame
        }

        isDashing = false;
        Debug.Log("Dash ended starting cooldown");

        //cooldown before allowing the next dash
        yield return new WaitForSeconds(dashCoolDown);

        canDash = true;
        Debug.Log("dash cool down over");

    }

    




}
