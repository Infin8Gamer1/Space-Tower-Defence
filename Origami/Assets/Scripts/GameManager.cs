using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject HomebasePrefab;

    [HideInInspector]
    public GameObject HomebaseRef;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        //Spawn object
        HomebaseRef = Instantiate(HomebasePrefab);

        //put homebase in place mode (moves it on raycast)
        HomebaseRef.GetComponent<TapToPlace>().Place();

        HomebaseRef.GetComponentInChildren<RotateToFaceObject>().target = Camera.main.gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
