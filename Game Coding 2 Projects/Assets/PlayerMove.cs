using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Vector3 playerMovementInput;
    private Vector2 playerMouseInput;
    //ref to players rigidbody
    private Rigidbody rb;
    //reference to camera
    public Camera playerCamera;
    public float moveSpeed;
    public float cameraLookSpeed;
    public float jumpForce;

    private float camLock;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
        //get axis vs get axis raw, the former has smoothing input 
        playerMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        playerMouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        MovePlayer();
        MovePlayerCamera();

        if(playerMouseInput.y > camLock)
        {
            playerMouseInput.y = camLock;
        }
        else if (playerMouseInput.y < -camLock)
        {
            playerMouseInput.y = -camLock;
        }
    }

    private void MovePlayer()
    {
        Vector3 MoveVector = transform.TransformDirection(playerMovementInput) * moveSpeed;
        rb.velocity = new Vector3(MoveVector.x, rb.velocity.y, MoveVector.z);

        if(Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void MovePlayerCamera()
    {

        float xRotation = playerMouseInput.y * cameraLookSpeed;

        transform.Rotate(0f, playerMouseInput.x * cameraLookSpeed, 0f);
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
