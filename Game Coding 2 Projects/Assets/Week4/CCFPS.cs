using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCFPS : MonoBehaviour
{

    private CharacterController cc;
    public float speed;
    private Vector3 playerMovement;
    public float jumpForce;
    private Vector3 velocity;
    private float gravity = -9.81f;

    public bool isGrounded;

    //camera look varrs
    //how fast camera moves
    public float mouseSensitivity;
    public Transform playerCamera;
    //track camera vertical- up and down movement
    private float yRotation = 0;
    private float xRotation = 0;

    //public GameObject weaponPrefab;


    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        MovePlayer();

        CameraLook();

        if (Input.GetKey(KeyCode.E))
        {
            //Instantiate(weaponPrefab);
        }

        
    }

    void MovePlayer()
    {
        isGrounded = cc.isGrounded;

        //ensures player stays grounded
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            
        }

        float inputMoveX = Input.GetAxis("Horizontal");
        float inputMoveZ = Input.GetAxis("Vertical");
        Vector3 move;

        Debug.Log("Input X: " + inputMoveX + ", Input Z: " + inputMoveZ);

        move = (transform.forward * inputMoveZ) + (transform.right * inputMoveX);

        cc.Move(move * speed * Time.deltaTime);

        //handle jumping
        if(isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            velocity.y = jumpForce;
        }

        //apply gravity
        velocity.y += gravity * Time.deltaTime;
        cc.Move(velocity * Time.deltaTime);
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

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0, 0);


    }
}
