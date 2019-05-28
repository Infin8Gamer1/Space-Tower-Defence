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

    public int waveNumber = 1;

    public float timeBetweenSpawns = 0.2f;

    private float timer = 0.0f;
    private bool waveSpawn = true;

    public bool waveStart = false;

    private bool wave5 = false;
    private bool wave10 = false;
    private bool wave15 = false;
    private bool wave20 = false;
    private bool wave25 = false;


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
            waveNumber += 1;
            if (waveNumber <= 5)
            {
                Enemys[0].SpawnIDMin = 0;
                Enemys[0].SpawnIDMax = 100;
                Enemys[1].SpawnIDMin = 0;
                Enemys[1].SpawnIDMax = 0;
                Enemys[2].SpawnIDMin = 0;
                Enemys[2].SpawnIDMax = 0;
                EnemysPerWave += 2;
            }
            else if (waveNumber > 5 && waveNumber <= 10)
            {
                if (wave5 == false)
                {
                    EnemysPerWave = 10;
                    wave5 = true;
                }
                Enemys[0].SpawnIDMin = 0;
                Enemys[0].SpawnIDMax = 66;
                Enemys[1].SpawnIDMin = 66;
                Enemys[1].SpawnIDMax = 100;
                Enemys[2].SpawnIDMin = 0;
                Enemys[2].SpawnIDMax = 0;

                EnemysPerWave += 3;
            }
            else if (waveNumber > 10 && waveNumber <= 15)
            {
                if (wave10 == false)
                {
                    EnemysPerWave = 10;
                    wave10 = true;
                }
                Enemys[0].SpawnIDMin = 0;
                Enemys[0].SpawnIDMax = 33;
                Enemys[1].SpawnIDMin = 33;
                Enemys[1].SpawnIDMax = 66;
                Enemys[2].SpawnIDMin = 66;
                Enemys[2].SpawnIDMax = 100;
                EnemysPerWave += 3;

            }
            else if (waveNumber > 15 && waveNumber <= 20)
            {
                if (wave15 == false)
                {
                    EnemysPerWave = 5;
                    wave15 = true;
                }
                Enemys[0].SpawnIDMin = 0;
                Enemys[0].SpawnIDMax = 0;
                Enemys[1].SpawnIDMin = 0;
                Enemys[1].SpawnIDMax = 50;
                Enemys[2].SpawnIDMin = 50;
                Enemys[2].SpawnIDMax = 100;
                EnemysPerWave += 2;
            }
            else if (waveNumber > 20 && waveNumber <= 25)
            {
                if (wave20 == false)
                {
                    EnemysPerWave = 10;
                    wave20 = true;
                }
                Enemys[0].SpawnIDMin = 0;
                Enemys[0].SpawnIDMax = 50;
                Enemys[1].SpawnIDMin = 0;
                Enemys[1].SpawnIDMax = 0;
                Enemys[2].SpawnIDMin = 50;
                Enemys[2].SpawnIDMax = 100;
                EnemysPerWave += 2;
            }
            else if (waveNumber > 25 && waveNumber <= 30)
            {
                if (wave25 == false)
                {
                    EnemysPerWave = 15;
                    wave25 = true;
                }
                Enemys[0].SpawnIDMin = 0;
                Enemys[0].SpawnIDMax = 33;
                Enemys[1].SpawnIDMin = 33;
                Enemys[1].SpawnIDMax = 66;
                Enemys[2].SpawnIDMin = 66;
                Enemys[2].SpawnIDMax = 100;
                EnemysPerWave += 2;
            }
            else if (waveNumber > 30)
            {
                EnemysPerWave += (waveNumber / 8);
            }
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
