using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class GazeManager : MonoBehaviour
{
    //public static GazeManager Instance { get; private set; }

    #region SINGLETON_PATTERN
    private static GazeManager _instance;

    public static GazeManager Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
        }
    }
    #endregion

    public GameObject FocusedObject { get; private set; }

    private GestureRecognizer recognizer;

    public List<Component> subscribedComponents;

    // Start is called before the first frame update
    void Start()
    {

        recognizer = new GestureRecognizer();
        recognizer.SetRecognizableGestures(GestureSettings.Tap);

        recognizer.Tapped += (args) =>
        {
            if (FocusedObject != null)
            {
                FocusedObject.SendMessageUpwards("OnSelect", SendMessageOptions.DontRequireReceiver);
            }

            foreach (Component x in subscribedComponents)
            {
                x.SendMessage("OnSelect", SendMessageOptions.DontRequireReceiver);
            }
        };

        recognizer.StartCapturingGestures();

        InteractionManager.InteractionSourceDetected += (args) =>
        {
            if (FocusedObject != null)
            {
                FocusedObject.SendMessageUpwards("OnSourceDetected", args, SendMessageOptions.DontRequireReceiver);
            }

            foreach (Component x in subscribedComponents)
            {
                x.SendMessage("OnSourceDetected", args, SendMessageOptions.DontRequireReceiver);
            }
        };

        InteractionManager.InteractionSourceLost += (args) =>
        {
            if (FocusedObject != null)
            {
                FocusedObject.SendMessageUpwards("OnSourceLost", args, SendMessageOptions.DontRequireReceiver);
            }

            foreach (Component x in subscribedComponents)
            {
                x.SendMessage("OnSourceLost", args, SendMessageOptions.DontRequireReceiver);
            }
        };

        InteractionManager.InteractionSourcePressed += (args) =>
        {
            if (FocusedObject != null)
            {
                FocusedObject.SendMessage("OnSourcePressed", args, SendMessageOptions.DontRequireReceiver);
            }

            foreach (Component x in subscribedComponents)
            {
                x.SendMessage("OnSourcePressed", args, SendMessageOptions.DontRequireReceiver);
            }
        };

        InteractionManager.InteractionSourceReleased += (args) =>
        {
            if (FocusedObject != null)
            {
                FocusedObject.SendMessage("OnSourceReleased", args, SendMessageOptions.DontRequireReceiver);
            }

            foreach (Component x in subscribedComponents)
            {
                x.SendMessage("OnSourceReleased", args, SendMessageOptions.DontRequireReceiver);
            }
        };

        InteractionManager.InteractionSourceUpdated += (args) =>
        {
            if (FocusedObject != null)
            {
                FocusedObject.SendMessage("OnSourceUpdated", args, SendMessageOptions.DontRequireReceiver);
            }

            foreach (Component x in subscribedComponents)
            {
                x.SendMessage("OnSourceUpdated", args, SendMessageOptions.DontRequireReceiver);
            }
        };
    }

    // Update is called once per frame
    void Update()
    {
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

                //hide the arrow if it is the target for the arrow
                if (GameManager.Instance.cursorRef.getArrowTarget() == FocusedObject.transform)
                {
                    GameManager.Instance.cursorRef.HideArrow();
                }
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
