using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToFaceObject : MonoBehaviour
{ 
    public Transform target;
    public bool useMainCameraAsTarget = false;

    [Range(0.01f, 5f)]
    public float strength = 0.5f;

    public bool UseX = false;
    public bool UseY = true;
    public bool UseZ = false;

    void Start()
    {
        if (useMainCameraAsTarget)
        {
            target = Camera.main.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);

        if (!UseX) targetRotation.x = transform.rotation.x;
        if (!UseY) targetRotation.y = transform.rotation.y;
        if (!UseZ) targetRotation.z = transform.rotation.z;

        float str = Mathf.Min(strength * Time.deltaTime, 1);

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, str);
    }
}
