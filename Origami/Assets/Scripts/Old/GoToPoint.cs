using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR.WSA.Input;

public class GoToPoint : MonoBehaviour {

    private NavMeshAgent agent;

    private Vector3 goal = new Vector3(0,0,0);

    private bool selectingPos = false;

    // Bit shift the index of the layer (8) to get a bit mask
    int layerMask = 1 << 8;

    void OnSelect(TappedEventArgs args)
    {
        if (args.tapCount == 1)
        {
            selectingPos = !selectingPos;

            if (selectingPos)
            {
                GetComponent<MeshRenderer>().material.color = new Color(1, 0, 0);
            }
            else
            {
                GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1);
            }
        }
    }

	// Use this for initialization
	void Start () {
        agent = GetComponent<NavMeshAgent>();
        if (agent != null && agent.isOnNavMesh)
        {
            agent.destination = goal;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (agent != null && agent.isOnNavMesh)
        {
            agent.destination = goal;
        }

        if (selectingPos)
        {
            // Do a raycast into the world that will only hit the Spatial Mapping mesh.
            var headPosition = Camera.main.transform.position;
            var gazeDirection = Camera.main.transform.forward;

            RaycastHit hitInfo;
            if (Physics.Raycast(headPosition, gazeDirection, out hitInfo,
                30.0f, layerMask))
            {
                goal = hitInfo.point;
            }
        }

	}
}


