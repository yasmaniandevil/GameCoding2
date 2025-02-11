using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    
    public int lives = 3;
    public TextMeshProUGUI livesText;

    private int startingLives;

    public void OnEnable()
    {
        //subscribing to the event
        //this script listens for the event
        //when onplayerloselife is triggered the loselife function runs in the health script
        GameManager.OnPlayerLoseLife += LoseLife;
    }

    private void OnDisable()
    {
        //unsubscribing
        GameManager.OnPlayerLoseLife -= LoseLife;
    }
    // Start is called before the first frame update
    void Start()
    {
        startingLives = lives;
        
        UpdateHealthText();
    }

    // Update is called once per frame
    private void LoseLife()
    {
        //lives -= 1;
        lives--;
        Debug.Log("Player has lost a life");
        UpdateHealthText();

        if(lives <= 0)
        {
            lives = 0;
            Debug.Log("Game over!");
            
            //add whatever logic you would like
        }
    }

    private void UpdateHealthText()
    {
        livesText.text = "Health: " + lives.ToString();
    }

    public void ResetHealth()
    {
        lives = startingLives;
        UpdateHealthText();

    }


}
