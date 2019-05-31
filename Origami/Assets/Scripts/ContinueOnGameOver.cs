using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueOnGameOver : MonoBehaviour
{
    public void cont()
    {
        GameManager.Instance.continueButton = true;
    }
}
