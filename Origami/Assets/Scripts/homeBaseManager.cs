using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeBaseManager : MonoBehaviour
{

    public void GameOver()
    {
        GameManager.Instance.Loose();
    }
}
