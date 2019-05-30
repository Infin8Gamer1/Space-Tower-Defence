using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class WorldCursor : MonoBehaviour {

    private MeshRenderer meshRenderer;

    [Range(0.4f, 4f)]
    public float defaultDistance = 2.5f;

    public Vector3 SourceDetectedScale = new Vector3(1,1,1);
    private Vector3 SourceLostScale;

    public LayerMask mask;

    public GameObject Arrow;

    private bool showArrow = false;
    private Transform arrowTarget = null;

    public void ShowArrow(Transform arrowTarget)
    {
        showArrow = true;

        this.arrowTarget = arrowTarget;

        Arrow.SetActive(true);
    }

    public void HideArrow()
    {
        showArrow = false;

        Arrow.SetActive(false);
    }

    public Transform getArrowTarget()
    {
        return arrowTarget;
    }

    // Use this for initialization
    void Start () {
        meshRenderer = gameObject.GetComponentInChildren<MeshRenderer>();

        GazeManager.Instance.subscribedComponents.Add(this);

        SourceLostScale = transform.localScale;

        Arrow.SetActive(false);
    }

    void OnSourceDetected(InteractionSourceDetectedEventArgs args)
    {
        transform.localScale = SourceDetectedScale;
    }

    void OnSourceLost(InteractionSourceLostEventArgs args)
    {
        transform.localScale = SourceLostScale;
    }

    // Update is called once per frame
    void Update () {
        Vector3 headPosition = Camera.main.transform.position;
        Vector3 gazeDirection = Camera.main.transform.forward;

        RaycastHit hitInfo;

        if (Physics.Raycast(headPosition, gazeDirection, out hitInfo, 25.0f, mask.value))
        {
            if (Vector3.Distance(hitInfo.point, headPosition) <= defaultDistance)
            {
                transform.position = hitInfo.point;

                transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
            } else
            {
                transform.position = (gazeDirection * defaultDistance) + headPosition;

                transform.rotation = Quaternion.FromToRotation(Vector3.up, gazeDirection);
            }
        }
        else
        {
            transform.position = (gazeDirection * defaultDistance) + headPosition;

            transform.rotation = Quaternion.FromToRotation(Vector3.up, gazeDirection);
        }

        if (showArrow && arrowTarget != null)
        {
            Arrow.transform.LookAt(arrowTarget);
        }
	}
}
