using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSPlayer : MonoBehaviour
{
    //add grounding
    //make can jump a bool & function
    //if ground canJump = true if not ground can jump false
    //jump function
    //in update if jump true && can jump call jump function
    //in fixedupdate if(spacebar) can jump = true
    //start game manager
    //respawn

    private Rigidbody rb;
    public float speed = 5;
    public float jumpForce = 5;

    //camera look varrs
    //how fast camera moves
    public float mouseSensitivity;
    public Transform cameraTransform;
    //track camera vertical- up and down movement
    private float yRotation = 0;  
    private float xRotation = 0;

    public bool isRunning;
    public float runningSpeed;

    public float crouchSpeed;
    public bool isCrouching = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        //locking cursor to middle of screen and making it invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame

    private void Update() 
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        //vertical input gets player W/S input +1 or -1
        float verticalInput = Input.GetAxis("Vertical");

        /*rb.velocity = new Vector3(horizontalInput * speed, rb.velocity.y,
            verticalInput * speed);*/

        //moving player based on direction they are facing
        //if you are facing north transform.forward is 0, 0, 1
        //if you rotate to the right (east) transform.forward becomes 1, 0, 0
        rb.velocity = transform.forward * verticalInput * speed + transform.right * horizontalInput
            * speed + Vector3.up * rb.velocity.y;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        CameraLook();

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isRunning = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRunning = false;
        }

        float currentSpeed;
        if (isRunning)
        {
            currentSpeed = runningSpeed;
        }
        else
        {
            currentSpeed = speed;
        }

        //for after we do running
        //rb.velocity = transform.right * horizontalInput * currentSpeed + Vector3.up * rb.velocity.y + transform.forward * verticalInput * currentSpeed;

        if(Input.GetKeyDown(KeyCode.E))
        {
            //flipping the bool so we can toggle it
            isCrouching = !isCrouching;
            Crouch();
        }

       
    }

    private void CameraLook()
    {
        //get and assign mouse inputs
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        //rotation around the y axis (look left and right)
        //when the mouse moves horizontally (x)
        yRotation += mouseX;
        //rotate the player left/rigght y axis rotation
        transform.rotation = Quaternion.Euler(0f, yRotation, 0);


        //Decrease xRotation when moving mouse up so camera tilts up
        //increaste x rotation when moving camera down so camera tilts down
        //rotate around the x axis( up and down)
        xRotation -= mouseY;
        //clamp rotation
        xRotation = Mathf.Clamp(xRotation, -90, 90); //prevent flipping
       
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0, 0);


    }

    public void Crouch()
    {
        if(isCrouching)
        {
            transform.localScale = new Vector3 (1, .5f, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        //otherway to write is
        //transform.localScale = isCrouching ? crouchScale : normalScale;
    }
}
