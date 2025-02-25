using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public GameObject weaponPrefab;
    public Transform weaponSocket;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //adds it to the list
            other.GetComponent<WeaponManager>().AddWeapon(weaponPrefab);
            //makes the weapon a child of parent
            transform.SetParent(other.transform);
            //moves it to correct spot on the player
            transform.localPosition = weaponSocket.position;
            
        }
    }
}
