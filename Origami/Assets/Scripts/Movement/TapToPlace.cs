using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class TapToPlace : MonoBehaviour
{
    private bool holding = false;

    public Material DefaultMaterial;
    public Material MovingMaterial;

    [Range(0.4f, 3.5f)]
    public float distance = 2.5f;

    [HideInInspector]
    public bool Placed { get; private set; }

    // Bit shift the index of the layer (8) to get a bit mask
    int layerMask = 1 << 8;

    public void Place()
    {
        holding = true;

        Placed = false;

        GetComponent<MeshRenderer>().material = MovingMaterial;
    }

    // Called by GazeGestureManager when the user performs a Select gesture
    void OnSelect()
    {
        if (holding)
        {
            GetComponent<MeshRenderer>().material = DefaultMaterial;

            holding = false;

            Placed = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (holding)
        {
            // Do a raycast into the world that will only hit the Spatial Mapping mesh.
            var headPosition = Camera.main.transform.position;
            var gazeDirection = Camera.main.transform.forward;

            RaycastHit hitInfo;
            if (Physics.Raycast(headPosition, gazeDirection, out hitInfo,
                distance + 0.5f, layerMask))
            {
                // Move this object's parent object to
                // where the raycast hit the Spatial Mapping mesh.
                this.transform.position = hitInfo.point + (Vector3.up * transform.lossyScale.y / 2);

                // Rotate this object's parent object to face the user.
                Quaternion toQuat = Camera.main.transform.localRotation;
                toQuat.x = 0;
                toQuat.z = 0;
                this.transform.rotation = toQuat;
            }
            else
            {
                //Move this object 3 units forward from the head position
                this.transform.position = (gazeDirection * distance) + headPosition;

                // Rotate this object's parent object to face the user.
                Quaternion toQuat = Camera.main.transform.localRotation;
                toQuat.x = 0;
                toQuat.z = 0;
                this.transform.rotation = toQuat;
            }
        }
    }
}
