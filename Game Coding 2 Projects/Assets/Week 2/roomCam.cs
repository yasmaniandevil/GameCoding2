using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class roomCam : MonoBehaviour
{
    //public Transform player;

    //for later in the lesson
    private PlatformPlayer playerScript;

    //the width of one "room" or camera view in world unitys
    public float camWidth = 20f;

    //holds the corizontal center of current cam
    private float currentRoomCenterX;

    //smothing factor for cam movement
    //higher value closer to 1 means the cam moves more quickly to target position
    public float smoothing = .125f;

    //offset between the camera and the players position
    //used to position camera relative to the player
    public Vector3 offset;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    // Start is called before the first frame update
    void Start()
    {
        //initalize the current room center using the cameras starting x position
        //currentRoomCenterX = transform.position.x;

        //original version
        //offset = transform.position - player.position;

        //FindPlayer();

        /*if(playerScript != null)
        {
            //set inital offset
            offset = transform.position - playerScript.transform.position;
        }*/

        InitalizeCamera();
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //check if player script is null if so try to find it
        if(playerScript == null)
        {
            FindPlayer();
        }

        //only proceed if we found the player
        //if statement for later in lesson
        if(playerScript != null)
        {


            float halfWidth = camWidth / 2;
            float leftBound = currentRoomCenterX - halfWidth;
            float rightbBound = currentRoomCenterX + halfWidth;

            //just player before not playerscript
            if (playerScript.transform.position.x < leftBound)
            {
                currentRoomCenterX -= camWidth;

            }
            else if (playerScript.transform.position.x > rightbBound)
            {
                currentRoomCenterX += camWidth;
            }

            //for the y axis smoothly follow player y pos
            float targetY = playerScript.transform.position.y + offset.y;
            //z pos remains as the cameras current z
            //float targetZ = transform.position.z;

            Vector3 targetCamPos = new Vector3(currentRoomCenterX, targetY, transform.position.z);

            transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing);
        }
        
    }

    void FindPlayer()
    { 
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if(playerObject != null)
        {
            playerScript = playerObject.GetComponent<PlatformPlayer>();
            offset = transform.position - playerScript.transform.position;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitalizeCamera();
    }

    private void InitalizeCamera()
    {

        FindPlayer();

        if (playerScript != null)
        {
            //reset offset based on the new player pos
            offset = transform.position - playerScript.transform.position;

            currentRoomCenterX = playerScript.transform.position.x;

            //immediately position the camera to avoid hitter at scene load
            transform.position = playerScript.transform.position + offset;
        }
    }
}
