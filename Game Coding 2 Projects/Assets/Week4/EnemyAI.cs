using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.IO;

public class EnemyAI : MonoBehaviour
{
    //defines diff states and switches between them
    public enum EnemyState { Idle, Patrol, Chase, Attack}
    private EnemyState currentState;

    //references
    public Transform player;
    private NavMeshAgent agent;

    //patrol settings
    public Transform[] patrolPoints;
    private int currentPatrolIndex;

    //AI settings CHANGE THIS TO BELOW
    /*public float detectionRange = 10f;
    public float attackRange = 1.5f;
    public float attackCoolDown = 2f;*/

    //enemy stats loaded from json
    public string enemyType; // Name of the enemy in the JSON
    private int health;
    private float speed;
    private float detectionRange;
    private float attackRange;
    private float attackCooldown;


    float lastAttackTime;
    int collisionCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        //load enemy data from JSON
        LoadEnemyData(enemyType);

        //apply loaded stats
        agent.speed = speed;

        currentState = EnemyState.Patrol; //start with patrolling
        MoveToNextPatrolPoint();
        
    }


    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        //switch statement is like multiple choice descion maker in programming, instead of a bunch if else statements
        //checks a variable and decides what code to run based off of its value

        //switch statement determines what behavior the enemy should perform based on its currentState
        //switch statement checks current state of enemy and decides which behavior to execute
        switch(currentState)
        { 
            case EnemyState.Idle:
                IdleBehavior();
                //break makes sure program doesnt check other cases once a match is found
                break;

                //moves between waypoints if player is detected it switches to chase
            case EnemyState.Patrol:
                PatrolBehavior();
                //if enemy within detection will switch to chase
                if (distanceToPlayer <= detectionRange) ChangeState(EnemyState.Chase);
                break;

                //moves toward player if close enough switches to attack
                //if player escapes switches back to patrol
            case EnemyState.Chase:
                ChaseBehavior();
                if(distanceToPlayer <= attackRange) ChangeState(EnemyState.Attack);
                else if(distanceToPlayer > detectionRange) ChangeState(EnemyState.Patrol);
                break;

                //attacks player if player moves away switches back to chase
            case EnemyState.Attack:
                AttackBehavior();
                if(distanceToPlayer > attackRange) ChangeState(EnemyState.Chase);
                break;
        }
    }

    //simply updates enemys current state
    void ChangeState(EnemyState newState)
    {
        currentState = newState;
    }

    void IdleBehavior()
    {
        //you can add an animation if you want
    }

    void PatrolBehavior()
    {
        //enemy follows fath to target (patrol point)
        //it waits until it reaches patrol point
        //once it reaches the point it moves to next location

        //pathPending is true if unity is still calculating path
        //if false means path has been fully calculated and enemy is actually moving towards target
        //ensures enemy only switches patrol points after reaching the target
        //if enmy is close enough to patrol point, .5 it moves to next one
        if(!agent.pathPending && agent.remainingDistance < .5f)
        {
            MoveToNextPatrolPoint();
        }
    }
    private void MoveToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;

        //.set destination moves to next patrol point
        agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        //updates the index
        //if currentpatrolindex is 0 it moves it to 1
        //loops back around
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
    }

    //when player is in range enemy follows them
    void ChaseBehavior()
    {
        agent.SetDestination(player.position);
        //Debug.Log("chase called");
    }

    //if player within attack range enemy attacks
    //uses cooldown to prevent spamming attacks
    void AttackBehavior()
    {
        if(Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            //Debug.Log("Enemy attacked player");
            //logic to reduce players health call from game manager
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.CompareTag("Bullet"))
        {

            //why couldnt i declare collisioncount locally?
            collisionCount++;
            Debug.Log("collision counts: " + collisionCount);
            Debug.Log("Enemy hit");

            if(collisionCount == 3)
            {
                Destroy(gameObject);
                FPSGameManager.Instance.Score++;
            }
        }
    }

    private void LoadEnemyData(string enemyName)
    {
        string path = Application.dataPath + "/Data/enemyData.json";
        if(File.Exists(path))
        {
            string json = File.ReadAllText(path);
            EnemyDataBase enemyDB = JsonUtility.FromJson<EnemyDataBase>(json);

            foreach(EnemyD enemy in enemyDB.enemiesList)
            {
                if(enemy.name == enemyName)
                {
                    health = enemy.health;
                    speed = enemy.speed;
                    detectionRange = enemy.detectionRange;
                    attackRange = enemy.attackRange;
                    attackCooldown = enemy.attackCooldown;
                    Debug.Log($"Loaded enemy: {enemy.name}");
                    return;
                }
            }

        }
        else
        {
            Debug.Log("enemy json file not found");
        }
    }

}
