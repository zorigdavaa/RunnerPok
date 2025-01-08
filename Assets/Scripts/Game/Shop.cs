using System.Collections;
using System.Collections.Generic;
using CandyKitSDK;
using UnityEngine;
using ZPackage;

public class Shop : MonoBehaviour
{
    List<CkProduct> Products = new List<CkProduct>();
    // Start is called before the first frame update
    void Start()
    {
        Products = CandyKit.Settings.Android.ckProducts;
    }
    public void AddCoin()
    {
        Z.GM.Coin++;
    }
}
