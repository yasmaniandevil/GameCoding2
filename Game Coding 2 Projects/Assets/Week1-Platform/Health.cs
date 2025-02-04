using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    
    public int lives = 3;
    public int currentHealth;

    public Image[] heartImages;
    

    public Transform player;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = lives;
        UpdateHeart();
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    private void UpdateHeart()
    {
        for(int i = 0; i < heartImages.Length; i++)
        {
            if(i < currentHealth)
            {
                heartImages[i].gameObject.SetActive(false);
            }
        }
    }

    public void LoseHeart()
    {
        currentHealth = Mathf.Max(currentHealth - 1, 0);
        UpdateHeart();

        if(currentHealth <= 0)
        {
            Debug.Log("player has died");
        }
    }
}
