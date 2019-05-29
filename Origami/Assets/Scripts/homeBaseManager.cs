using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class homeBaseManager : MonoBehaviour
{

    public EnemySpawner SpawnerReference;

    public void nextWave()
    {
        SpawnerReference.waveStart = true;
    }
}
