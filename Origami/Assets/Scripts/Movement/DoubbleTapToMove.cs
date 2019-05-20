using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class DoubbleTapToMove : MonoBehaviour {

    private bool placing = false;

    private Material originalMaterial;

    public Material SelectedMaterial;

    // Bit shift the index of the layer (8) to get a bit mask
    int layerMask = 1 << 8;

    void OnSelect(TappedEventArgs args)
    {
        if (args.tapCount == 2)
        {
            // On each Select gesture, toggle whether the user is in placing mode.
            placing = !placing;

            if (placing)
            {
                originalMaterial = GetComponent<MeshRenderer>().material;
                GetComponent<MeshRenderer>().material = SelectedMaterial;
            }
            else
            {
                GetComponent<MeshRenderer>().material = originalMaterial;
            }
        }
    }
	
    void Start()
    {
        originalMaterial = GetComponent<MeshRenderer>().material;
    }

	// Update is called once per frame
	void Update () {
        if (placing)
        {
            // Do a raycast into the world that will only hit the Spatial Mapping mesh.
            var headPosition = Camera.main.transform.position;
            var gazeDirection = Camera.main.transform.forward;

            RaycastHit hitInfo;
            if (Physics.Raycast(headPosition, gazeDirection, out hitInfo,
                30.0f, layerMask))
            {
                // Move this object's parent object to
                // where the raycast hit the Spatial Mapping mesh.
                this.transform.position = hitInfo.point;

                // Rotate this object's parent object to face the user.
                Quaternion toQuat = Camera.main.transform.localRotation;
                toQuat.x = 0;
                toQuat.z = 0;
                this.transform.rotation = toQuat;
            }
        }
    }
}
