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

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SwapToSphere()
    {
        if(spherePrefab == null)
        {
            Debug.Log("sphere is not in inspector");
            return;
        }

        GameObject newPlayer = Instantiate(spherePrefab, transform.position, Quaternion.identity);

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
