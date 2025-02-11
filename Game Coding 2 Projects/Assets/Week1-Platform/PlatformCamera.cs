using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformCamera : MonoBehaviour
{
    //ref to the player
    //public Transform player;
    //camera offset from the player
    public Vector3 offset = new Vector3 (0, 5f, -10f);
    //how smoothly the camera follows
    public float followSpeed = 5f;

    private PlatformPlayer playerScript;


    // Start is called before the first frame update
    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerScript = playerObject.GetComponent<PlatformPlayer>();
            offset = transform.position - playerScript.transform.position;
        }

    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (playerScript == null) return;

        if (playerScript != null)        
        {
            //target position for the camera
            Vector3 targetPos = playerScript.transform.position + offset;

            //smoothly move the camera to the target position
            transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);
        }
        
    }
}
