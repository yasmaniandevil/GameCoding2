using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//not monobehavipr bc we want a plain c# class not an actual gameobject
//monobehavior is for scripts attached to gameobjects in unity
//this script is just data and isnt attached to anything
//json needs a simple class

//without this JsonUtility.FromJson won’t work because Unity won’t recognize them
[System.Serializable] //tells unity how to store and load data
//allows the class to be saved and loaded
public class EnemyD //change to enemy stats for class
{
    //templatye for enemy states
    //will be used inside json file
    public string name;
    public int health;
    public float speed;
    public float detectionRange;
    public float attackRange;
    public float attackCooldown;
}

[System.Serializable]
public class EnemyDataBase
{
    //container for multiple enemies
    //unity will read json into this structure
    //must match name in json
    public List<EnemyD> enemiesList = new List<EnemyD>();
}
