using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerMovement : MonoBehaviour
{
    //separate speed for move and look for better editor UI/UX
    public float speed = .2f;
    public float lookSpeed = 5f;
    public float jumpForce = 50f;
    public GameObject myCam;
    //camLock controls up/down look maximums
    public float camLock = 90;

    //public for debug purposes only
    public bool jumped = false;
    public bool canJump = true;

    Rigidbody myRB;
    Vector3 myLook;

    public int score = 0;

 /*   void Awake()
    {
        //hides cursor and locks to middle of game screen
        Cursor.lockState = CursorLockMode.Locked;
    }*/

    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody>();
        myLook = Vector3.zero;
        jumped = false;
        canJump = true;
    }

    // Update is called once per frame
    void Update()
    {
        //get the player look, pull the direction the camera is facing
        //TransformDirection() translates the vector from local (player) space to world (scene) space
        Vector3 playerLook = myCam.transform.TransformDirection(Vector3.forward);
        Debug.DrawRay(transform.position, playerLook * 3f, Color.magenta);

        //look is cumulative, so instead of setting it to a new value
        //we add a delta (difference in look angle)
        //multiply by Time.deltaTime since we're in Update() to standardize look speed across any framerate
        myLook += DeltaLook() * Time.deltaTime;

        //clamp our look to a reasonable range so the player is always in view
        //and we can't infinitely rotate cam look
        if (myLook.y > camLock)
        {
            myLook.y = camLock;
        }
        else if (myLook.y < -camLock)
        {
            myLook.y = -camLock;
        }

        //Debug.Log("look dir:" + myLook);

        //apply our look rotations to the player AND the camera
        transform.rotation = Quaternion.Euler(0f, myLook.x, 0f);
        myCam.transform.rotation = Quaternion.Euler(-myLook.y, myLook.x, 0f);

        if (Input.GetKey(KeyCode.Space))
        {
            jumped = true;
        }
    }

    void FixedUpdate()
    {
        //find our player's desired direction
        //again use TransformDirection() to translate to world space
        Vector3 myDir = transform.TransformDirection(Dir());
        //Debug.Log("input dir: " + myDir);
        //use AddForce so this works with the physics engine
        //all physics engine code should go in FixedUpdate() not Update()
        myRB.AddForce(myDir * speed);
        if (jumped && canJump)
        {
            Jump();
        }
    }
    Vector3 Dir()
    {
        //first we declare a placeholder vector
        Vector3 moveDir = Vector3.zero;
        //reference the built in unity input manager axes for faster / controller friendly inputs
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        //combine the two axes into one vector, our desired walk direction
        moveDir = new Vector3(x, 0, z);

        //if(Input.GetKey(KeyCode.Space))
        //{
        //    moveDir.y = JumpVal();
        //}

        //pass out our combined moveDir vector based off the hor/vert axes
        return moveDir;
    }
    Vector3 DeltaLook()
    {
        //declare our placeholder vector
        Vector3 deltaLook = Vector3.zero;
        //get our look deltas - note, these return not a normalized vector (-1,1)
        //but a difference in current vs. previous position, frame by frame
        float rotY = Input.GetAxisRaw("Mouse Y") * lookSpeed;
        float rotX = Input.GetAxisRaw("Mouse X") * lookSpeed;

        //combining them here
        deltaLook = new Vector3(rotX, rotY, 0);
        return deltaLook;
    }
    void Jump()
    {
        myRB.AddForce(Vector3.up * jumpForce);
        jumped = false;
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Ground") { canJump = true; }
    }

    void OnCollisionExit(Collision collision)
    {
        canJump = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Death")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            score++;
        }
    }
}
