using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public List<GameObject> weaponList = new List<GameObject>();
    private int currentWeaponIndex = 0;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SwitchWeapon();
        }
    }

    public void AddWeapon(GameObject weaponPrefab)
    {
        GameObject newWeapon = Instantiate(weaponPrefab, transform);
        newWeapon.SetActive(false);

        weaponList.Add(newWeapon);
        if(weaponList.Count == 1 )//if its the first weapon picked up, activate it
        {
            
        }
    }

    private void SwitchWeapon()
    {
        weaponList[currentWeaponIndex].SetActive(false);
        currentWeaponIndex = (currentWeaponIndex + 1) % weaponList.Count;
        weaponList[currentWeaponIndex].SetActive(true);
    }
}
