using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class GazeGestureManager : MonoBehaviour {

    public static GazeGestureManager Instance { get; private set; }

    public GameObject FocusedObject { get; private set; }

    GestureRecognizer recognizer;

    public GameObject Cursor;

	// Use this for initialization
	void Awake () {
        Instance = this;

        recognizer = new GestureRecognizer();
        recognizer.SetRecognizableGestures(GestureSettings.Tap | GestureSettings.DoubleTap);

        recognizer.Tapped += (args) =>
        {
            if (FocusedObject != null)
            {
                FocusedObject.SendMessageUpwards("OnSelect", args, SendMessageOptions.DontRequireReceiver);
            }
        };

        recognizer.StartCapturingGestures();


        InteractionManager.InteractionSourceDetected += (args) =>
        {
            if (args.state.source.kind == InteractionSourceKind.Hand)
            {
                Cursor.transform.localScale = new Vector3(1.0f, 0.5f, 1.0f);
            }
        };

        InteractionManager.InteractionSourceLost += (args) =>
        {
            if (args.state.source.kind == InteractionSourceKind.Hand)
            {
                Cursor.transform.localScale = new Vector3(0.5f, 0.25f, 0.5f);
            }

            if (FocusedObject != null)
            {
                FocusedObject.SendMessageUpwards("OnSourceLost", args, SendMessageOptions.DontRequireReceiver);
            }
        };

        InteractionManager.InteractionSourcePressed += (args) =>
        {
            if (FocusedObject != null)
            {
                FocusedObject.SendMessageUpwards("OnSourcePressed", args, SendMessageOptions.DontRequireReceiver);
            }
        };

        InteractionManager.InteractionSourceReleased += (args) =>
        {
            if (FocusedObject != null)
            {
                FocusedObject.SendMessageUpwards("OnSourceReleased", args, SendMessageOptions.DontRequireReceiver);
            }
        };

        InteractionManager.InteractionSourceUpdated += (args) =>
        {
            if (FocusedObject != null)
            {
                FocusedObject.SendMessageUpwards("OnSourceUpdated", args, SendMessageOptions.DontRequireReceiver);
            }
        };

    }
	
	// Update is called once per frame
	void Update () {
        GameObject oldFocusObject = FocusedObject;

        Vector3 headPosition = Camera.main.transform.position;
        Vector3 gazeDirection = Camera.main.transform.forward;

        RaycastHit hitInfo;
        if (Physics.Raycast(headPosition, gazeDirection, out hitInfo))
        {
            FocusedObject = hitInfo.collider.gameObject;
        }
        else
        {
            FocusedObject = null;
        }

        if (FocusedObject != oldFocusObject)
        {
            if (FocusedObject != null)
            {
                FocusedObject.SendMessageUpwards("OnCursorEnter", SendMessageOptions.DontRequireReceiver);
            }

            if (oldFocusObject != null)
            {
                oldFocusObject.SendMessageUpwards("OnCursorLeave", SendMessageOptions.DontRequireReceiver);
            }

            recognizer.CancelGestures();
            recognizer.StartCapturingGestures();
        }
	}
}
