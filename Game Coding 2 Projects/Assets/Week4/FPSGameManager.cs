using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FPSGameManager : MonoBehaviour
{
    public static FPSGameManager Instance;


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
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
