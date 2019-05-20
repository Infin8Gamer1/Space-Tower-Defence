using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;
using TMPro;

public class TapToBuyItem : MonoBehaviour
{
    public GameObject ItemPrefab;
    public Mesh ItemMesh;

    [Header("Price")]
    public int cost;
    public TextMeshPro PriceText;
    public BankManager Bank;

    // Called by GazeGestureManager when the user performs a Select gesture
    void OnSelect(TappedEventArgs args)
    {
        //withdraw money from account (if it doesn't bounce then continue)
        if (Bank.WitdrawMoney(cost))
        {
            //Spawn object
            GameObject obj = Instantiate(ItemPrefab);

            //put object in place mode (moves it on raycast)
            obj.GetComponent<TapToPlace>().Place();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        PriceText.text = "$" + cost;

        GetComponent<MeshFilter>().mesh = ItemMesh;
    }

    // Update is called once per frame
    void Update()
    {
        PriceText.text = "$" + cost;
    }
}
