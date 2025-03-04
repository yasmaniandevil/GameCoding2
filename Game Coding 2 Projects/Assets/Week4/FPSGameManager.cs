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

    private int score = 0;
    public int gameLength = 40;
    public float timer = 0;

    public TextMeshPro displayText;

    bool inGame = true;

    const string DIR_DATA = "/Data/";
    const string FILE_HIGH_SCORE = "highScore.txt";
    string PATH_HIGH_SCORE;

    public const string PREF_HIGH_SCORE = "hsScore";

    public List<int> highScoreList = new List<int>();
    public int Score
    {
        get
        {
            return score;
        }
        set
        {
            score = value;
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
        timer = 0;
        PATH_HIGH_SCORE = Application.dataPath + DIR_DATA + FILE_HIGH_SCORE;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (inGame)
        {
            timer += Time.deltaTime;
            displayText.text = "Timer: " + (gameLength - (int)timer);
        }

        if(timer >= gameLength && inGame)
        {
            inGame = false;
            Debug.Log("Game Over");
            //scenemanager.loadScene ("game over screen
            //or turn on UI turn off game time.time = 0);
            UpdateHighScores();
        }
    }

    private void UpdateHighScores()
    {
        //take the highscores out of the file and put them on the list
        if(highScoreList.Count == 0)
        {
            //if we already have high scores
            if (File.Exists(PATH_HIGH_SCORE))
            {
                //get the high scores from the file as a string
                string fileContents = File.ReadAllText(PATH_HIGH_SCORE);

                //split the string up into an array
                string[] fileSplit = fileContents.Split('\n');

                //go thru all the strings that are numbers
                for(int i = 1; i < fileSplit.Length -1; i++)
                {
                    highScoreList.Add(Int32.Parse(fileSplit[i]));
                }
            }
            else
            {
                //add a place holder high score
                highScoreList.Add(0);
            }
        }
        //what is diff between count and length?
        //insert our score into the high score list if its large enough
        for(int i = 0; i < highScoreList.Count; i++)
        {
            if(highScoreList[i] < Score)
            {
                highScoreList.Insert(i, Score);
                break;
            }
        }

        //if we have more than 5 high Scores
        if(highScoreList.Count > 5)
        {
            //cut it to 5 high scores
            highScoreList.RemoveRange(5, highScoreList.Count - 5);
        }

        //make a string of all our high scores
        string highScoreText = "High Scores:\n";

        for(int i = 0; i < highScoreList.Count; i++)
        {
            highScoreText += highScoreList[i] + "\n";
        }

        //display high scores
        displayText.text = highScoreText;

        File.WriteAllText(PATH_HIGH_SCORE, highScoreText);
        Debug.Log(highScoreText);
    }
}
