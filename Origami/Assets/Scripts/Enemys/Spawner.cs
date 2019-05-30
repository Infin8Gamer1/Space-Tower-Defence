using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyType
{
    public GameObject Prefab;

    [Range(0,100)]
    public int spawnRangeMin = 0;
    [Range(0,100)]
    public int spawnRangeMax = 100;

    public bool ShouldSpawn(int number)
    {
        return (number >= spawnRangeMin && number <= spawnRangeMax);
    }
}

[System.Serializable]
public class Wave
{
    [Min(0)]
    public int WaveNumber = 0;

    [Range(0.1f, 15f)]
    public float StartWaitTime = 5f;

    [Range(1,200)]
    public int NumberOfEnemysInWave = 10;

    [Range(0.1f, 10f)]
    public float SpawnTime = 0.5f;

    [Range(0f, 3f)]
    public float SpawnTimeVariance = 0.2f;

    [Tooltip("leave blank to use default enemy types")]
    public List<EnemyType> EnemyTypes = new List<EnemyType>();
}

public class Spawner : MonoBehaviour
{
    public List<EnemyType> defaultEnemyTypes = new List<EnemyType>();

    public List<Wave> waves = new List<Wave>();

    private int enemiesSpawnedInWave;

    private int curentWaveIndex;

    private bool LoopGoing = false;

    private bool WaveSpawned = false;

    private bool paused = false;

    // Start is called before the first frame update
    void Start()
    {
        curentWaveIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //GM.CurrentWave = curentWaveIndex + 1;
        if (!paused)
        {
            if (LoopGoing == false)
            {
                StartCoroutine(SpawnLoop());
            }

            if (WaveSpawned && GameManager.Instance.EnemiesKilledInWave >= enemiesSpawnedInWave)
            {
                enemiesSpawnedInWave = 0;
                GameManager.Instance.EnemiesKilledInWave = 0;

                if ((curentWaveIndex + 1) < waves.Count)
                {
                    curentWaveIndex += 1;
                    WaveSpawned = false;
                    LoopGoing = false;
                }
                else
                {
                    GameManager.Instance.Win();
                    paused = true;
                }

            }
        }
        
    }

    IEnumerator SpawnLoop()
    {
        if (paused)
        {
            yield break;
        }

        LoopGoing = true;

        if (curentWaveIndex > waves.Count)
        {
            GameManager.Instance.Win();
            paused = true;
            yield break;
        }

        //wait for wave start seconds
        yield return new WaitForSeconds(waves[curentWaveIndex].StartWaitTime);

        //spawn zomibies in wave
        //wait for spawn time and repeat until all zombies for wave have been spawned
        for (int x = 0; x < waves[curentWaveIndex].NumberOfEnemysInWave; x++)
        {
            spawnEnemy((waves[curentWaveIndex].EnemyTypes.Count > 0) ? waves[curentWaveIndex].EnemyTypes : defaultEnemyTypes);
            
            yield return new WaitForSeconds(Mathf.Clamp(Random.Range(waves[curentWaveIndex].SpawnTime - waves[curentWaveIndex].SpawnTimeVariance, waves[curentWaveIndex].SpawnTime + waves[curentWaveIndex].SpawnTimeVariance), 0.0f, 200f));
        }

        WaveSpawned = true;
    }

    // spawns an enemy based on the enemy level that you selected
    private void spawnEnemy(List<EnemyType> enemies)
    {
        // To check which enemy prefab to instantiate
        int SpawnID = Random.Range(0, 100);

        foreach (EnemyType enemy in enemies)
        {
            if (enemy.ShouldSpawn(SpawnID))
            {
                //spawn this enemy
                Instantiate(enemy.Prefab, transform.position, Quaternion.identity);

                enemiesSpawnedInWave++;

                break;
            }
        }

    }
}
