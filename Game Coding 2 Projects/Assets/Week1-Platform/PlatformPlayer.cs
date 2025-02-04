using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlatformPlayer : MonoBehaviour
{
    //for physics based interactions rigidbody is better (fall guys)

    //but for precise platforming movement character controoller is better
    //(mario, celeste type games) smoother and easier to fine tune



    private Rigidbody rb;
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public bool isGrounded;

    //later in the lesson
    //layer for ground detection
    public LayerMask groundLayer;
    //empty gameobject at players feet
    public Transform groundCheck;
    //raycast distance for ground detection
    public float groundDistance = 0.3f;

    public GameObject respawnPos;

    //ground pound
    public float groundPoundForce = 20f;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //later in the lesson
        //raycasting is more reliable than oncollisionenter and exit
        isGrounded = Physics.Raycast(groundCheck.position, Vector3.down, groundDistance, groundLayer);
        Debug.DrawRay(groundCheck.position, Vector3.down * groundDistance, Color.red);


        //get player input (WASD)
        //x is left and right z is forward and back y is up and down
        float moveX = Input.GetAxis("Horizontal");
        //float moveZ = Input.GetAxis("Vertical");

        //Apply Movement
        Vector3 move = new Vector3(moveX, 0, 0) * moveSpeed;
        rb.velocity = new Vector3(move.x, rb.velocity.y, 0);

        Respawn();

        //we could also write it this way
        /*playerMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
       
        moveVector = playerMovementInput * moveSpeed;
        rb.velocity = new Vector3(moveVector.x, rb.velocity.y, moveVector.z);*/

        

    }

    private void Update()
    {
        //jump if the player is grounded & space is pressed
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        if(Input.GetMouseButtonDown(0) && !isGrounded)
        {
            GroundPound();
        }

        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            //isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            //isGrounded = false; 
        }
    }

    private void OnDrawGizmos()
    {
        //draw a debug line to visualize the ground check raycast
        if(groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(groundCheck.position, 
                groundCheck.position + Vector3.down * groundDistance);
        }
    }

    public void Respawn()
    {
        if(transform.position.y < -3 && !isGrounded)
        {
            transform.position = respawnPos.transform.position;
            //Debug.Log("respawned player");
        }
    }

    private void GroundPound()
    {
        //you can add an animation here
        ClearForces();
        rb.velocity = new Vector3(rb.velocity.x, -groundPoundForce, rb.velocity.z);
    }

    private void ClearForces()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    
    private void OnTriggerEnter(Collider other)
    {
        
    }
}
