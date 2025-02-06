using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissapearPlatform : MonoBehaviour
{

    public float disappearTime = 2f;
    public bool playerOnPlatform;

    // Update is called once per frame
    void Update()
    {
        if (playerOnPlatform)
        {
            disappearTime -= Time.deltaTime;
            if(disappearTime < 0)
            {
                gameObject.SetActive(false);
            }
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Player"))
        {
            playerOnPlatform = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            
            playerOnPlatform = false;
        }
    }

    public void ResetPlatform()
    {
        gameObject.SetActive(true);
        disappearTime = 2f;
        playerOnPlatform = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("detetced player");
        }
    }


}
