using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToFaceObject : MonoBehaviour
{ 
    public Transform target;

    [Range(0.01f, 5f)]
    public float strength = 0.5f;

    public bool UseX = false;
    public bool UseY = true;
    public bool UseZ = false;

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
