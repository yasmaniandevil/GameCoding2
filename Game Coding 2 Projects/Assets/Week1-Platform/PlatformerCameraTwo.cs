using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerCameraTwo : MonoBehaviour
{
    public Transform playerTransform;

    //offset on the z to -10
    private Vector3 offset = new Vector3 (0, 0, -10);

    //var for camera boundaries
    private float minX;
    private float maxX;
    private float minY;
    private float maxY;

    public float mapX = 100f;
    public float mapY = 100f;

    //amount of time for camera to get to the player
    public float cameraTime = 5f;


    // Start is called before the first frame update
    void Start()
    {
        //get original size of camera at start
        minX = transform.position.x;
        minY = transform.position.y;

        //set max boundaries
        maxX = mapX;
        maxY = mapY;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerTransform != null)
        {
            //sets desired position to players transform anf offsets it
            Vector3 desiredPos = playerTransform.position + offset;

            //ensures camera is inside x boundaries
            desiredPos.x = (desiredPos.x < minX) ? minX : desiredPos.x;
            desiredPos.x = (desiredPos.x > maxX) ? maxX : desiredPos.x;

            //check if its inside boundaries on the y
            desiredPos.y = (desiredPos.y < minY) ? minY : desiredPos.y;
            desiredPos.y = (desiredPos.y > maxY) ? maxY : desiredPos.y;

            //smoothly moves camera by settings its transform.position with a lerp
            transform.position = Vector3.Lerp(transform.position, desiredPos, cameraTime * Time.deltaTime);
           
        }
    }
}
