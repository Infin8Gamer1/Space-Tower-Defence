using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.WSA.Input;

public class ToggleButton : MonoBehaviour
{
    public bool StartingState = false;

    [Header("Pressed Settings")]
    public Material pressed;
    public Vector3 pressedScale;

    [Header("Unpressed Settings")]
    public Material unPressed;
    public Vector3 unPressedScale;

    [Header("Events")]
    public UnityEvent OnClicked;
    public UnityEvent OnPressed;
    public UnityEvent OnUnPressed;

    private bool state = false;

    void Start()
    {
        state = StartingState;

        if (state == true)
        {
            GetComponent<MeshRenderer>().material = pressed;

            transform.localScale = pressedScale;
        }
        else
        {
            GetComponent<MeshRenderer>().material = unPressed;

            transform.localScale = unPressedScale;
        }
    }

    void OnSelect()
    {
        state = !state;

        OnClicked.Invoke();

        if (state == true)
        {
            OnPressed.Invoke();

            GetComponent<MeshRenderer>().material = pressed;

            transform.localScale = pressedScale;
        } else
        {
            OnUnPressed.Invoke();

            GetComponent<MeshRenderer>().material = unPressed;

            transform.localScale = unPressedScale;
        }
    }
}
