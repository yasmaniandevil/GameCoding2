using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public Transform[] spawnPoints;

    public int waveNumber = 1;
    public float timeBetweenWaves = 10f;
    public int enemiesPerWave = 3;

    private int enemiesAlive = 0;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenWaves);
            SpawnEnemies();
        }
    }

    private void SpawnEnemies()
    {
        Debug.Log($"Spawning wave {waveNumber}");
        for(int i = 0; i <enemiesPerWave; i++)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

            GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            enemiesAlive++;

        }

        waveNumber++;
        enemiesPerWave += 2; //increase enemy count every wave
    }

    public void EnemyDied()
    {
        enemiesAlive--;
        if(enemiesAlive <= 0)
        {
            Debug.Log("Wave cleared");
        }
    }
}
