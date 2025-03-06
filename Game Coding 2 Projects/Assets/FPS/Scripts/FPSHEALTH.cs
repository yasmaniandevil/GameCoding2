using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FPSHEALTH : MonoBehaviour
{
    public event Action <float, float> onHealthChanged;

    public float maxHealth = 100;
    public float currentHealth;
    public int addHealth = 5;

    // Start is called before the first frame update
    void Start()
    {
        if (maxHealth <= 0) maxHealth = 100; // Default max health if not set
        currentHealth = maxHealth;
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            ChangeHealth(+20);
        }
    }

    public void ChangeHealth(float amount)
    {
        Debug.Log($"Before Change - Current Health: {currentHealth}, Max Health: {maxHealth}");

        float oldhealth = currentHealth;
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        Debug.Log($"After Change - Current Health: {currentHealth}, Max Health: {maxHealth}");

        onHealthChanged?.Invoke(oldhealth, currentHealth);
        Debug.Log($"Health changed: {oldhealth} to {currentHealth}");


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("HealthPickup"))
        {
            ChangeHealth(+20);
            Debug.Log("health after pickup: " + currentHealth);
            Destroy(other.gameObject);
        }
    }
}
