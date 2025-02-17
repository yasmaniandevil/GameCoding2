using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

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


    //ground pound
    public float groundPoundForce = 20f;

    private float fallThreashold = -3;


    //public DissapearPlatform disPlatform;
    //public List<DissapearPlatform> platformList = new List<DissapearPlatform>();

    public bool isOnIce = false;
    //.98 more slippery takes longer to stop
    //.9 less slipper stops faster
    public float iceDecelerate = .98f; //adds slippery effect
    public float iceAccelerate = .5f; //slower acceleration on ice

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
        float moveZ = Input.GetAxis("Vertical");

        

     

        //we could also write it this way
        /*playerMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
       
        moveVector = playerMovementInput * moveSpeed;
        rb.velocity = new Vector3(moveVector.x, rb.velocity.y, moveVector.z);*/

        if(isOnIce )
        {
            //didnt work because we were applying a physics material with zero friction on the ice platform but the player is still manually setting velocity in fixed update
            //movement script is overriding physics based sliding
            //Friction settings on the ice platform don't affect velocity directly when you are explicitly setting rb.velocity.
            
            //ice deceleration slowly reduces velocity over time
            //player keeps moving but gradually slows down
            rb.velocity = new Vector3(rb.velocity.x * iceDecelerate, rb.velocity.y, rb.velocity.z * iceDecelerate);
            //using add force instead of directly setting velocity, allows physics to handle acceleration
            //we add force in the desired direction handles rigidbody movement naturally
            rb.AddForce(new Vector3(moveX * iceAccelerate, 0, moveZ * iceAccelerate), ForceMode.Acceleration);
            //Debug.Log("ice physics");
        }
        else
        {
            //we move regular movement into here
            //i dont know if you are using forward/backwards if youre not turn the z to 0
            Vector3 move = new Vector3(moveX, 0, moveZ) * moveSpeed;
            //same thing here turn move to 0
            rb.velocity = new Vector3(move.x, rb.velocity.y, move.z);
            //Debug.Log("not on ice");

        }
        

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

        

        CheckIfFallen();
        
    }

  


    public void CheckIfFallen()
    {
        if(transform.position.y < fallThreashold && !isGrounded)
        {
            //notify game mananger to handle respawn and life deduction
            GameManager.Instance.PlayerFell(gameObject);
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ice"))
        {
            Debug.Log("Ice");
            isOnIce = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("ice"))
        {
            Debug.Log("is not on ice");
            isOnIce = false;
        }
    }


}
