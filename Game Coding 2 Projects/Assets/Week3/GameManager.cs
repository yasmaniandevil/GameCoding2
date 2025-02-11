using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //to ensure there is only one game manager
    //static makes the var globally accessible
    public static GameManager Instance;//singleton instance

    //event delgate that notifies subscribers when the player loses a life
    //this creates a gloabl event that other scripts can listen to
    public static event Action OnPlayerLoseLife;

    //default respawnPos
    public GameObject defaultRespawnPos;

    //currentrespawn position, updated by checkpoints
    private GameObject currentRespawnPos;

    //reference to the persistent player object
    private GameObject player;

    //called before start ensures singleton pattern
    void Awake()
    {
        //checks if there already a game manager
        if(Instance == null)
        {
            //if not assigns the current one
            Instance = this;
            //keeps game manager alive when switching scenes
            DontDestroyOnLoad(gameObject);//persist across scenes

            //subscribe to scene loaded event to handle respawn resetting
            //we are subscribing to unitys built in event scenemanager.sceneloaded which tells unity
            //to call our function everytime a new scene is loaded
            //we arent calling the function here just subscriping we are just regustering our function
            //to be called when the event happens
            //we are saying "hey unity, when you load a new scene, call my onsceneload function
            //when the scene is loaded in the game unity automatically calls the function
            //and passes in the two arguments
            SceneManager.sceneLoaded += OnSceneLoad;
        }
        else
        {
            Destroy(gameObject); //destroy duplicate gamemanagers if they exist
        }
    }

    //called when the game manager is destroyed
    private void OnDestroy()
    {
        //unscubscribe from sceneload event to prevent meory leaks
        //if we dont unsubscribe unity will continue to call that function even if the obj has been destroyed
        //ensures when a game manager is destroyed it stops listening for the scene load event
        SceneManager.sceneLoaded -= OnSceneLoad;
    }

    // called after awake, initalizes player and respawn position
    void Start()
    {
        //find the player
        player = GameObject.FindGameObjectWithTag("Player");
        //set inital respawnpoint
        currentRespawnPos = defaultRespawnPos;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

            //later in lesson
            //ResetGame();
        }
    }

    //called when player falls below the fall threshold
    //passing in an argument, so the gamemanager doesnt have to search for the player everytime
    //it can just use the object provided
    public void PlayerFell(GameObject playerObj)
    {
        Debug.Log("player fell losing a life and respawning");
        //the ?. ensures that the event is only triggered if there are subscribers
        OnPlayerLoseLife?.Invoke(); //trigger event for health reduction
        RespawnPlayer(playerObj); //respawn player at last checkpoint
    }

    //moves the player to the current respawn position
    public void RespawnPlayer(GameObject playerVar)
    {
        if(currentRespawnPos != null)
        {
            playerVar.transform.position = currentRespawnPos.transform.position;
            Debug.Log("player respawn at checkpoint");
        }
        else
        {
            Debug.Log("no respawn set");
        }
    }

    //updates the respawnpoint when the player hits a checkpoint
    public void UpdateRespawnPoint(GameObject newRespawn)
    {
        currentRespawnPos = newRespawn;
    }

    //now moving on and creating a second scene later in lesson
    //lets do a game over function
    public void NextScene()
    {
        SceneManager.LoadScene(1);
    }

    //resets the players position to the current respawn point
    private void ResetPlayerPosition()
    {
        if(player != null && currentRespawnPos != null)
        {
            player.transform.position = currentRespawnPos.transform.position;
        }
    }

    //do this after the player starts falling in scene 2
    //called when a new scene is loaded
    private void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        //find the new default respawn point in the new scene
        defaultRespawnPos = GameObject.FindGameObjectWithTag("defaultRespawn");

        if(defaultRespawnPos != null)
        {
            currentRespawnPos = defaultRespawnPos; //reset respawn to default in newscene
            ResetPlayerPosition(); //move player to respawn point
            Debug.Log("rspawn point reset to scene default");
        }
        else
        {
            Debug.Log("no default respawn point found");
        }

    }

    //for later
    public void ResetGame()
    {
        Health playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        if(playerHealth != null )
        {
            playerHealth.ResetHealth();
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void UpdatePlayerReference(GameObject newPlayer)
    {
        player = newPlayer;

    }
}
