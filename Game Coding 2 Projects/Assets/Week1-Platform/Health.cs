using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Health : MonoBehaviour
{
    public TextMeshProUGUI healthText;
    public int lives = 3;

    public Transform player;
    // Start is called before the first frame update
    void Start()
    {
        UpdateHealthText();
    }

    // Update is called once per frame
    void Update()
    {
        if(player.transform.position.y <= -2)
        {
            lives--;
            UpdateHealthText();
        }

        if(lives <= 0)
        {
            lives = 0;
            Debug.Log("Game Over");
        }
    }

    private void UpdateHealthText()
    {
        healthText.text = "Health: " + lives.ToString();
    }
}
