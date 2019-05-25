using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    public int DamageToBase = 50;

    public GameObject ExplosionPrefab;

    public GameObject DeathParticles;

    private GameObject Target;

    private NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        Target = GameManager.Instance.HomebaseRef;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("HomeBase"))
        {
            Health otherHealth = other.gameObject.GetComponent<Health>();
            if (otherHealth != null)
            {
                otherHealth.RemoveHealth(DamageToBase);
            }

            if (ExplosionPrefab != null)
            {
                Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (agent != null && agent.isOnNavMesh)
        {
            if (Target != null)
            {
                agent.destination = Target.transform.position;
            } else
            {
                Target = GameManager.Instance.HomebaseRef;
            }
        }
    }

    void OnDestroy()
    {
        GameManager.Instance.EnemyDied();
        //Instantiate(DeathParticles, transform.position, Quaternion.identity);
    }
}
