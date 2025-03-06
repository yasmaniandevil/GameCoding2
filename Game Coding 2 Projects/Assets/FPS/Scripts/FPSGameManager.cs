using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using System.IO;

public class FPSGameManager : MonoBehaviour
{
    public static FPSGameManager Instance;

    //players current score
    public int score = 0;
    //game time settings
    public int gameLength = 40; //total time in seconds
    public float timer = 0; //tracks elapsed time

    public TextMeshProUGUI displayText;
    //keeps track if game is running
    bool inGame = true;

    //file path var for saving high scores
    //const is used to define var that never changes we use cont to represent fixed file paths
    const string DIR_DATA = "/Data/"; //subdirectory for storing data
    const string FILE_HIGH_SCORE = "highScore.txt"; //file name
    string PATH_HIGH_SCORE; //full path to the high score file

    
    //list to store high scores
    public List<int> highScoreList = new List<int>();

    public TextMeshProUGUI healthText;
    FPSHEALTH playerHealth;

    //property to safely update score and log it when it changes
    //we use get and set instead of public int score bc 
    //prevents modification of score from outside the class
    //allows you to add extra logic when changing the score
    public int Score
    {
        get
        {
            return score; //read current score
        }
        set
        {
            score = value; //update current score
            Debug.Log("score change");
        }
    }

    void Awake()
    {
        //checks if there already a game manager
        if (Instance == null)
        {
            //if not assigns the current one
            Instance = this;
            //keeps game manager alive when switching scenes
            DontDestroyOnLoad(gameObject);//persist across scenes
          
        }
        else
        {
            Destroy(gameObject); //destroy duplicate gamemanagers if they exist
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        timer = 0; //reset game timer

        //construct the full file path for saving high scores
        PATH_HIGH_SCORE = Application.dataPath + DIR_DATA + FILE_HIGH_SCORE;
        Debug.Log("high score file path: " + PATH_HIGH_SCORE);

        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<FPSHEALTH>();
        playerHealth.onHealthChanged += UpdateHealthText;
        UpdateHealthText(playerHealth.maxHealth, playerHealth.maxHealth);
    }

    private void OnDestroy()
    {
        playerHealth.onHealthChanged -= UpdateHealthText;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        //if game is still running update timer
        if (inGame)
        {
            timer += Time.deltaTime;
            displayText.text = "Timer: " + (gameLength - (int)timer);
        }

        //end game when timer reaches the game length
        if(timer >= gameLength && inGame)
        {
            inGame = false;
            Debug.Log("Game Over");
            //scenemanager.loadScene ("game over screen
            //or turn on UI turn off game time.time = 0);
            UpdateHighScores(); //save high scores when game ends
        }
    }

    private void UpdateHighScores()
    {
        //take the highscores out of the file and put them on the list
      
        //load high scores from the file if it exists
        if (File.Exists(PATH_HIGH_SCORE))
        {
          Debug.Log("high score file found! reading data");

          //get the high scores from the file as a string
          string fileContents = File.ReadAllText(PATH_HIGH_SCORE);
          //n means move to next line
          Debug.Log("File Contents Before Processing:\n" + fileContents);

          //split the string up into an array
          string[] fileSplit = fileContents.Split('\n');

            //clear the existinghigh score list before adding new scores
            highScoreList.Clear(); //if we dont will keep adding scores indefinitely
          

          //convert each string into an integer and add it to high score list
          foreach (string scoreString in fileSplit)
          {
             if (int.TryParse(scoreString, out int scoreValue))
             {
                  highScoreList.Add(scoreValue);
                  Debug.Log("loaded score: " + scoreValue);
             }
          }
        }

        //.count returns numbers of elements in the list
        //length returns total sizer of the array including unused elements

        //if high score list is empty add a default score (prevents errors)
        if(highScoreList.Count == 0)
        {
            highScoreList.Add(0);
        }

        Debug.Log("Current High Score List Before Adding New Score: " + string.Join(", ", highScoreList));

        
        //insert the new score into the correct position in the high score list
        if (!highScoreList.Contains(Score))//prevents duplicate scores that it inst already in list before inserting it
        {
            //goes through each score in the list to find where new score should go
            for(int i = 0; i < highScoreList.Count; i++)
            {
                //current list 100, 90, 80 new Score is 85 85 > 80 so it should go before
                if(highScoreList[i] < Score) //find where the new score fits
                {
                    //Moves everything after i one position down and puts Score at index i.
                    highScoreList.Insert(i, Score);
                    Debug.Log("Inserted new score: " + Score + " at position " + i);
                    break; //stops checking once the score is inserted w/o would continue even after score is added
                }
            }

        }

        //if we have more than 5 high Scores
        if (highScoreList.Count > 5)
        {
            //cut it to 5 high scores
            Debug.Log("High score list is too long. Trimming...");
            highScoreList.RemoveRange(5, highScoreList.Count - 5);
        }

        Debug.Log("Final High Score List Before Saving: " + string.Join(", ", highScoreList));

        //make a string of all our high scores
        string highScoreText = "High Scores:\n";

        for (int i = 0; i < highScoreList.Count; i++)
        {
            highScoreText += highScoreList[i] + "\n";
        }
        

        //display high scores
        displayText.text = highScoreText;
        
        //write high score to the file
        File.WriteAllText(PATH_HIGH_SCORE, highScoreText);
        Debug.Log("high scores written");
    }

    public void UpdateHealthText(float oldhealth, float newHealth)
    {
        healthText.text = "Health: " + newHealth;
    }
}
