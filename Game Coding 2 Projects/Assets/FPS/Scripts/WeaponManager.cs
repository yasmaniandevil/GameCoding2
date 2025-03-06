using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    //lists to hold all weapon instances the player has picked up
    public List<GameObject> weaponList = new List<GameObject>();
    //index to track currently active weapon
    private int currentWeaponIndex = -1; //start with no weapon


    // Update is called once per frame
    void Update()
    {
        //key to switch weapons
        //weaponList.Count ensures that there is at least one weapon in the list before attemptimg to switch weapons
        if (Input.GetKeyDown(KeyCode.Q) && weaponList.Count > 0)
        {
            //+ 1 increments the currentweaponindex by 1 moving to the next weapon in the list
            //% wrapping effect. if currentweaponindex + 1 equals the length of the list weaponslist.count it resets it to 0
            //if player is using last weapon in the list and presses Q it wraps back around to first weapon
            int nextWeaponIndex = (currentWeaponIndex + 1) % weaponList.Count;
            SwitchWeapon(nextWeaponIndex);
        }
    }

    public void AddWeapon(GameObject weaponPrefab)
    {
        //add the instantiated weapon to the list
        weaponList.Add(weaponPrefab);//add weapon to list
        //prevents multiple active weapons at once
        weaponPrefab.SetActive(false); //start with weapon disabled

        if(weaponList.Count == 1 )//if its the first weapon picked up, activate it
        {
            SwitchWeapon(0);
        }
    }


    //switches to the weapon at specified index
    private void SwitchWeapon(int index)
    {
        //deactivate the currently active weapon if there is one
        if(currentWeaponIndex != -1)
        {
            //ensures when switching weapons the previous one is turned off
            weaponList[currentWeaponIndex].SetActive(false);
        }

        //set the new weapon as active and update the current index
        currentWeaponIndex = index;
        weaponList[currentWeaponIndex].SetActive(true);
    }
}
