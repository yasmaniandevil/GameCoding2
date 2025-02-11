using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTransform : MonoBehaviour
{

    public GameObject spherePrefab;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void SwapToSphere()
    {
        //first check if assigned
        if(spherePrefab == null)
        {
            Debug.Log("sphere is not in inspector");
            return;
        }

        //instattae the prefab same position and rotation
        GameObject newPlayer = Instantiate(spherePrefab, transform.position, transform.rotation);

        //for later in lesson make sphere child of gamemanager
        newPlayer.transform.SetParent(GameManager.Instance.transform);

        GameManager.Instance.UpdatePlayerReference(newPlayer);

        //transfer rigidbody velocity from current player to new player
        //helps keep momentum consisten btwn frames
        Rigidbody newRb = newPlayer.GetComponent<Rigidbody>(); 
        if(newRb != null && rb != null)
        {
            newRb.velocity = rb.velocity;
        }

        Destroy(gameObject);
          
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Switch"))
        {
            SwapToSphere();

        }
    }
}
