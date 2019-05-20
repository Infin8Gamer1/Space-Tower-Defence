using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class DragToMove : MonoBehaviour {

    private bool holding = false;
    private bool released = false;

    public Material DefaultMaterial;
    public Material MovingMaterial;

    [Range(0.1f,1.0f)]
    public float holdDelay = 0.75f;

    [Range(0.4f, 3.5f)]
    public float distance = 2.5f;

    // Bit shift the index of the layer (8) to get a bit mask
    int layerMask = 1 << 8;

    void OnSourcePressed(InteractionSourcePressedEventArgs args)
    {
        released = false;

        StartCoroutine(HoldDelay());
    }

    IEnumerator HoldDelay()
    {
        WaitForSeconds wait = new WaitForSeconds(0.75f);
        yield return wait;

        if (!released)
        {
            holding = true;

            GetComponent<MeshRenderer>().material = MovingMaterial;
        }
    }

    void OnSourceReleased(InteractionSourceReleasedEventArgs args)
    {
        GetComponent<MeshRenderer>().material = DefaultMaterial;

        holding = false;
        released = true;
    }

    void OnSourceLost(InteractionSourceLostEventArgs args)
    {
        GetComponent<MeshRenderer>().material = DefaultMaterial;

        holding = false;
        released = true;
    }

    // Update is called once per frame
    void Update () {
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
                this.transform.position = hitInfo.point + (Vector3.up * transform.lossyScale.y/2);

                // Rotate this object's parent object to face the user.
                Quaternion toQuat = Camera.main.transform.localRotation;
                toQuat.x = 0;
                toQuat.z = 0;
                this.transform.rotation = toQuat;
            } else
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
