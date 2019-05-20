using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BankManager : MonoBehaviour
{
    private int balance = 0;

    [Range(0,1000)]
    public int StartingBalance = 500;
    [Range(0f, 0.5f)]
    public float TextUpdateTimer = 0.1f;

    public TextMeshPro Text;

    // Start is called before the first frame update
    void Start()
    {
        balance = StartingBalance;

        StartCoroutine(TextUpdateLoop());
    }

    //Add money to account
    public void AddMoney(int Ammount)
    {
        balance += Ammount;
    }

    //Withdraw Money (Retruns true if there was enough money retruns false if there wasn't)
    public bool WitdrawMoney(int Ammount)
    {
        if (Ammount > balance)
        {
            return false;
        }

        balance -= Ammount;

        return true;
    }

    public int GetBalance()
    {
        return balance;
    }

    IEnumerator TextUpdateLoop()
    {
        var wait = new WaitForSeconds(TextUpdateTimer);
        while (true)
        {
            Text.text = "Balance:\n" + balance;
            yield return wait;
        }
    }
}
