using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    public HashSet<string> choiceFlags = new HashSet<string>();

    //DO THIS FIRST
    //public int charisma = 3;
    //public int logic = 2;
    //public int empathy = 5;

    //dictionary is like a labeled body, the label is called a key (usually string or int)
    //inside the box is a value (can be anything, int string object list)
    
    //instead of hard coding above we can make dynamic and flexible
    //add new stats without rewriting
    //loop through all stats
    //you could load them from JSON
    public Dictionary<string, int> stats = new Dictionary<string, int>();

    private void Awake()
    {
        if(Instance == null) //instance hasnt been set yet
        {
            DontDestroyOnLoad(gameObject); 
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        //base stats later in lesson
        stats["Logic"] = 2;
        stats["Empathy"] = 2;
        stats["Charisma"] = 2;
    }

    public void Update()
    {
        //Debug.Log("choice flags: " + choiceFlags.ToString());
    }

    public int GetStat(string statName)
    {
        if(stats.ContainsKey(statName)) 
               return stats[statName];
        return 0;
    }

    public void IncreaseStat(string _statName, int amount)
    {
        //contains key checks to see if it exits in our dictionary
        //doesnt return stat value it just says it exists
        //stats["Logic] gives us the int value tied to the string

        //if the stat key does not exist then create it and set it to 0
        if (!stats.ContainsKey(_statName))
        {
            stats[_statName] = 0;
        }
        
        //if it does exist just add
        stats[_statName] += amount;

        Debug.Log($"Increased {_statName} by {amount}. New Value {stats[_statName]}");
    }


    public void AddChoiceFlag(string flagName)
    {
        choiceFlags.Add(flagName);
        
    }

    public bool HasChoiceFlag(string flagName)
    {
        return choiceFlags.Contains(flagName);
    }
}
