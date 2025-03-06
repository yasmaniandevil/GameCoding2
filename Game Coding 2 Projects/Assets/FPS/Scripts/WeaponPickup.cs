using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    //the prefab that will be instantiated when picked up
    public GameObject weaponPrefab;
    //the transform socket to which the weapon will be parented to the player
    public Transform weaponSocket;

 

    private void OnTriggerEnter(Collider other)
    {
        //if player
        if (other.CompareTag("Player"))
        {
            //instantiate and parent directly to weapon socket
            GameObject newWeapon = Instantiate(weaponPrefab, weaponSocket.position, Quaternion.identity, weaponSocket);

            //reset local pos and rotation to ensure it fits correctly in the socket
            newWeapon.transform.localPosition = Vector3.zero;
            newWeapon.transform.localRotation = Quaternion.identity;

            //adds it to the list
            other.GetComponent<WeaponManager>().AddWeapon(newWeapon);
            
            //destroys the weapon pick up game object
            Destroy(gameObject);
            
        }
    }
}
