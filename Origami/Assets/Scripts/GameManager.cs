using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.XR.WSA;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Loading Assets")]
    private bool loading = true;
    public GameObject LoadingBillboardPrefab;
    private GameObject LoadingBillboardRef;

    [Header("HomeBase Assets")]
    public GameObject HomebasePrefab;

    [HideInInspector]
    public GameObject HomebaseRef;

    private bool HomebasePlaced = false;

    [Header("Portal Assets")]
    [Range(0.3f,3f)]
    public float DesiredPortalDistance = 2f;

    public GameObject PortalPrefab;

    [HideInInspector]
    public GameObject PortalRef;

    [Header("General Assets")]
    public GameObject SpatialMapingRef;

    public ScoreManager scoreManager { get; set; }

    public WorldCursor cursorRef;

    public int EnemiesKilledInWave { get; set; }

    [Header("Win / Loss Assets")]
    public GameObject WinBilboard;

    public GameObject LooseBilboard;

    public GameObject PortalExplosion;

    public GameObject HomeBaseExplosionFirst;

    public GameObject HomeBaseExplosionSecond;

    public bool gameEndSequence = false;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        HomebasePlaced = false;

        loading = true;

        EnemiesKilledInWave = 0;

        LoadingBillboardRef = Instantiate(LoadingBillboardPrefab);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Navmesh Size : " + SpatialMapingRef.GetComponent<NavMeshSurface>().size);
        if (!gameEndSequence)
        {
            if (loading == false)
            {
                if (HomebaseRef == null)
                {
                    //Spawn homebase
                    HomebaseRef = Instantiate(HomebasePrefab);

                    HomebaseRef.GetComponentInChildren<RotateToFaceObject>().target = Camera.main.gameObject.transform;

                    //put homebase in place mode (moves it on raycast)
                    HomebaseRef.GetComponent<TapToPlace>().Place();

                    scoreManager = HomebaseRef.GetComponent<ScoreManager>();
                }

                if (HomebasePlaced == false)
                {
                    if (HomebaseRef.GetComponent<TapToPlace>().Placed)
                    {
                        HomebasePlaced = true;

                        PlacePortal();

                        cursorRef.ShowArrow(PortalRef.transform);

                        StartCoroutine(DisableNavCalculations());
                    }
                }
            }
            else
            {
                if (SpatialMapingRef.transform.childCount > 0)
                {
                    loading = false;

                    Destroy(LoadingBillboardRef);
                    LoadingBillboardRef = null;
                }
            }
        }

    }

    IEnumerator DisableNavCalculations()
    {
        yield return new WaitForSeconds(15.0f);

        SpatialMapingRef.GetComponent<NavigationBaker>().enableCalculations = false;

        SpatialMapingRef.GetComponent<SpatialMappingCollider>().freezeUpdates = true;
        SpatialMapingRef.GetComponent<SpatialMappingRenderer>().freezeUpdates = true;
    }

    private void PlacePortal()
    {
        List<Vector3> points = new List<Vector3>();

        //loop until at least one point is found
        float adjustment = 0f;
        while (points.Count == 0)
        {
            points = GetPossiblePortalSpawnPoints(DesiredPortalDistance + adjustment);

            if (points.Count == 0)
            {
                adjustment -= 0.3f;
            }
        } 

        int pointIndex = Random.Range(0, points.Count - 1);

        Vector3 selectedPoint = points[pointIndex];

        //Spawn Portal
        PortalRef = Instantiate(PortalPrefab, selectedPoint, Quaternion.identity);

        //HomebaseRef.GetComponent<homeBaseManager>().SpawnerReference = PortalRef.GetComponent<EnemySpawner>();


        /*//set the portal rotation to be looking at the home base
        Quaternion targetRotation = Quaternion.LookRotation(HomebaseRef.transform.position - PortalRef.transform.position);

        targetRotation.x = 0;
        targetRotation.z = 0;

        PortalRef.transform.rotation = targetRotation;*/
    }

    private List<Vector3> GetPossiblePortalSpawnPoints(float portalDistance)
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

            Vector3 point = (pointDirection * portalDistance) + HomebaseRef.transform.position;

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

        return RealPoints;
    }

    public void EnemyDied()
    {
        PortalRef.GetComponent<EnemySpawner>().EnemyKilled();

        scoreManager.AddScore(1);
    }

    public void Win()
    {
        gameEndSequence = true;
        StartCoroutine(win());
    }

    private IEnumerator win()
    {
        Instantiate(PortalExplosion, PortalRef.transform.position, Quaternion.identity);
        Destroy(PortalRef);
        yield return new WaitForSeconds(3f);

        Instantiate(WinBilboard);

        yield return new WaitForSeconds(10f);

        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        gameEndSequence = false;
        Destroy(HomebaseRef);

        yield return null;
    }

    public void Loose()
    {
        gameEndSequence = true;
        StartCoroutine(loose());
    }

    private IEnumerator loose()
    {
        Instantiate(HomeBaseExplosionFirst, HomebaseRef.transform.position, Quaternion.identity);
        Destroy(HomebaseRef);
        yield return new WaitForSeconds(3f);

        Instantiate(LooseBilboard);

        yield return new WaitForSeconds(10f);

        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        gameEndSequence = false;
        Destroy(PortalRef);

        yield return null;
    }
}
