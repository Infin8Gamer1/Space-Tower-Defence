using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    private int score = 0;

    [Range(0f, 0.5f)]
    public float TextUpdateTimer = 0.1f;

    public TextMeshPro Text;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;

        StartCoroutine(TextUpdateLoop());
    }

    //Add money to account
    public void AddScore(int Ammount)
    {
        score += Ammount;
    }

    public int GetScore()
    {
        return score;
    }

    IEnumerator TextUpdateLoop()
    {
        var wait = new WaitForSeconds(TextUpdateTimer);
        while (true)
        {
            Text.text = "Score:\n" + score;
            yield return wait;
        }
    }
}
