﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class WorldCursor : MonoBehaviour {

    private MeshRenderer meshRenderer;

    [Range(0.4f, 4f)]
    public float defaultDistance = 2.5f;

    // Use this for initialization
    void Start () {
        meshRenderer = gameObject.GetComponentInChildren<MeshRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 headPosition = Camera.main.transform.position;
        Vector3 gazeDirection = Camera.main.transform.forward;

        RaycastHit hitInfo;

        if (Physics.Raycast(headPosition, gazeDirection, out hitInfo))
        {
            transform.position = hitInfo.point;

            transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
        }
        else
        {
            transform.position = (gazeDirection * defaultDistance) + headPosition;

            transform.rotation = Quaternion.FromToRotation(Vector3.up, gazeDirection);
        }
	}
}