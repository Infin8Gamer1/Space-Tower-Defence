using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{

    public float DeathTime = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DeathTime -= Time.deltaTime;
        if (DeathTime <= 0.0f)
        {
            Destroy(gameObject);
        }
    }
}
