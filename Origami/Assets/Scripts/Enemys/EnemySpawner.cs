using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawnSettings
{
    public GameObject Prefab;

    [Range(0, 100)]
    public int SpawnIDMin = 0;

    [Range(0, 100)]
    public int SpawnIDMax = 100;
}

public class EnemySpawner : MonoBehaviour
{
    public bool Spawn = true;

    [Header("Enemys")]
    public List<EnemySpawnSettings> Enemys;

    //----------------------------------
    // Enemies and how many have been created and how many are to be created
    //----------------------------------
    [Header("Wave Settings")]
    public int EnemysPerWave = 10;
    private int numActiveEnemy = 0;

    public float timeBetweenSpawns = 0.2f;

    private float timer = 0.0f;
    private bool waveSpawn = true;

    void Start()
    {
    }

    void Update()
    {
        if (Spawn)
        {
            timer += Time.deltaTime;
        
            if (timer >= timeBetweenSpawns)
            {
                waveSpawnUpdate();

                timer = 0.0f;
            }
            
        }
    }

    private void waveSpawnUpdate()
    {
        //spawns enemies in waves, so once all are dead, spawns more
        if (waveSpawn)
        {
            //spawns an enemy
            spawnEnemy();
        }

        if (numActiveEnemy <= 0)
        {
            // enables the wave spawner
            waveSpawn = true;
        }

        if (numActiveEnemy >= EnemysPerWave)
        {
            // disables the wave spawner
            waveSpawn = false;
        }
    }

    // spawns an enemy based on the enemy level that you selected
    private void spawnEnemy()
    {
        // To check which enemy prefab to instantiate
        int SpawnID = Random.Range(0, 100);

        foreach (EnemySpawnSettings enemySpawn in Enemys)
        {
            if (SpawnID >= enemySpawn.SpawnIDMin && SpawnID <= enemySpawn.SpawnIDMax)
            {
                //spawn this enemy
                Instantiate(enemySpawn.Prefab, transform.position, Quaternion.identity);

                // Increase the total number of enemies spawned and the number of spawned enemies
                numActiveEnemy++;

                break;
            }
        }
        
    }

    // Call this function from the enemy when it "dies" to remove an enemy count
    public void EnemyKilled()
    {
        numActiveEnemy--;
    }

    //enable the spawner based on spawnerID
    public void EnableSpawner()
    {
        Spawn = true;
    }
    //disable the spawner based on spawnerID
    public void DisableSpawner()
    {
        Spawn = false;
    }
}
