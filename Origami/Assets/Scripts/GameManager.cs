using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    

    public GameObject HomebasePrefab;

    [HideInInspector]
    public GameObject HomebaseRef;

    private bool HomebasePlaced = false;

    [Range(0.3f,3f)]
    public float PortalDistance = 2f;

    public GameObject PortalPrefab;

    [HideInInspector]
    public GameObject PortalRef;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        HomebasePlaced = false;

        //Spawn homebase
        HomebaseRef = Instantiate(HomebasePrefab);

        HomebaseRef.GetComponentInChildren<RotateToFaceObject>().target = Camera.main.gameObject.transform;

        //put homebase in place mode (moves it on raycast)
        HomebaseRef.GetComponent<TapToPlace>().Place();
    }

    // Update is called once per frame
    void Update()
    {
        if (HomebasePlaced == false)
        {
            if (HomebaseRef.GetComponent<TapToPlace>().Placed)
            {
                HomebasePlaced = true;

                PlacePortal();
            }
        }
    }

    void PlacePortal()
    {
        //create an array of potential points
        List<Vector3> PotentialPoints = new List<Vector3>();

        int numPoints = 8;

        for (int i = 0; i < numPoints; i++)
        {
            //make an angle in radians
            //                 Angle In Deg         Convert to Rads
            float angle = (i / numPoints * 360f) * (Mathf.PI / 180f);

            //calculate the direction vector
            Vector3 pointDirection = new Vector3(Mathf.Cos(angle), 0.0f, Mathf.Sin(angle));

            Vector3 point = (pointDirection * PortalDistance) + HomebaseRef.transform.position;

            PotentialPoints.Add(point);
        }

        //check each point for a valid point if there is a valid point add it to the real points array
        List<Vector3> RealPoints = new List<Vector3>();

        foreach (Vector3 x in PotentialPoints)
        {
            NavMeshHit navMeshHit;

            if (NavMesh.SamplePosition(x, out navMeshHit, 0.35f, NavMesh.AllAreas))
            {
                RealPoints.Add(navMeshHit.position);
            }
        }

        int pointIndex = Random.Range(0, RealPoints.Count - 1);

        Vector3 selectedPoint = RealPoints[pointIndex];

        //Spawn Portal
        PortalRef = Instantiate(PortalPrefab, selectedPoint, Quaternion.identity);
    }
}
