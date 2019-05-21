using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    private GameObject Target;

    private NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        Target = GameManager.Instance.HomebaseRef;
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
}
