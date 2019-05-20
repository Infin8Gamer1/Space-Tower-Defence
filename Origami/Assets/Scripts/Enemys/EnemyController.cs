using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    public GameObject Target;

    private NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        Target = GameObject.FindGameObjectWithTag("Target");
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
                Target = GameObject.FindGameObjectWithTag("Target");
            }
        }
    }
}
