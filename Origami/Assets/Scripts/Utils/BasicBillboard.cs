using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBillboard : MonoBehaviour
{

    [Range(0.4f, 4f)]
    public float defaultDistance = 3f;

    public float rotationSpeed = 0.5f;
    public float positionSpeed = 0.5f;

    public LayerMask mask;

    private Vector3 DesiredPos;


    // Start is called before the first frame update
    void Start()
    {
        Vector3 headPosition = Camera.main.transform.position;
        Vector3 gazeDirection = Camera.main.transform.forward;

        DesiredPos = (gazeDirection * defaultDistance) + headPosition;

        transform.position = DesiredPos;
    }

    // Update is called once per frame
    void Update()
    {
        //preform raycast to get desired position and rotation
        Vector3 headPosition = Camera.main.transform.position;
        Vector3 gazeDirection = Camera.main.transform.forward;

        RaycastHit hitInfo;

        if (Physics.Raycast(headPosition, gazeDirection, out hitInfo, 25.0f, mask.value))
        {
            if (Vector3.Distance(hitInfo.point, headPosition) <= defaultDistance)
            {
                DesiredPos = hitInfo.point;
            }
            else
            {
                DesiredPos = (gazeDirection * defaultDistance) + headPosition;
                
            }
        }
        else
        {
            DesiredPos = (gazeDirection * defaultDistance) + headPosition;
        }

        //set the y to the same as the cameras y
        DesiredPos.y = Camera.main.transform.position.y;

        //lerp pos to desired value
        transform.position = Vector3.Lerp(transform.position, DesiredPos, Time.deltaTime * positionSpeed);

        //use look at to look at the main camera
        transform.LookAt(Camera.main.transform);
    }
}
