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
        Z.GM.Coin += 100;
    }
    public void AddCoin1000()
    {
        Z.GM.Coin += 1000;
    }
    public void AddCoin5000()
    {
        Z.GM.Coin += 5000;
    }
    public void BuyBounceShuriken()
    {
        Debug.Log("Bounce Shuriken bought");
    }
    public void BuyNoAds()
    {
        Debug.Log("No Ads bought");
    }
    public void BuyBundle()
    {
        Debug.Log("Bundle bought");
    }
}
