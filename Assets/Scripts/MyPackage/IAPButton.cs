using System;
using System.Collections;
using System.Collections.Generic;
using CandyKitSDK;
using UnityEngine;
using UnityEngine.UI;

public class IAPButton : MonoBehaviour
{
    public CkProduct product;
    public Button Button;
    // Start is called before the first frame update
    void Start()
    {
        Button.onClick.AddListener(BuyProduct);
    }

    // Update is called once per frame
    void BuyProduct()
    {
        CandyKit.BuyProduct(product.ID, OnComplete);
    }

    private void OnComplete(bool success)
    {
        product.Logic?.Invoke();
    }
}
